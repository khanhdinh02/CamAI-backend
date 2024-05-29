using System.Linq.Expressions;
using System.Net.Mime;
using Core.Application.Events;
using Core.Application.Events.Args;
using Core.Application.Exceptions;
using Core.Application.Specifications.Incidents.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class IncidentService(
    IBlobService blobService,
    IAccountService accountService,
    IEmployeeService employeeService,
    ICameraService cameraService,
    ISupervisorAssignmentService supervisorAssignmentService,
    IEdgeBoxInstallService edgeBoxInstallService,
    IUnitOfWork unitOfWork,
    IBaseMapping mapping,
    IncidentSubject incidentSubject
) : IIncidentService
{
    private static IEnumerable<IncidentType> IncidentTypes =>
        Enum.GetValues<IncidentType>().Where(t => t != IncidentType.Interaction);

    public async Task<Incident> GetIncidentById(Guid id, bool includeAll = false)
    {
        var incident =
            (await unitOfWork.Incidents.GetAsync(new IncidentByIdRepoSpec(id, includeAll))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Incident), id);
        var currentAccount = accountService.GetCurrentAccount();
        return currentAccount.Role switch
        {
            Role.BrandManager when currentAccount.BrandId == incident.Shop.BrandId => incident,
            Role.ShopManager when currentAccount.Id == incident.Shop.ShopManagerId => incident,
            Role.ShopSupervisor when currentAccount.Id == incident.Assignment!.SupervisorId => incident,
            _ => throw new NotFoundException(typeof(Incident), id)
        };
    }

    public async Task<PaginationResult<Incident>> GetIncidents(SearchIncidentRequest searchRequest)
    {
        var account = accountService.GetCurrentAccount();
        switch (account.Role)
        {
            case Role.BrandManager:
                searchRequest.BrandId = account.BrandId;
                break;
            case Role.ShopManager:
                searchRequest.BrandId = null;
                searchRequest.ShopId = account.ManagingShop!.Id;
                break;
            case Role.ShopSupervisor:
                searchRequest.InChargeId = account.Id;
                break;
        }

        var includeShop = account.Role == Role.BrandManager;
        return await unitOfWork.Incidents.GetAsync(new IncidentSearchSpec(searchRequest, includeShop));
    }

    public async Task AssignIncidentToEmployee(Guid id, Guid employeeId)
    {
        var incident = await GetIncidentById(id);
        var employee = await employeeService.GetEmployeeById(employeeId);
        if (incident.ShopId != employee.ShopId)
            throw new BadRequestException("Incident and employee are not in the same shop");

        if (incident.IncidentType == IncidentType.Interaction)
            throw new BadRequestException($"Cannot assign employee for interaction #{incident.Id}");

        incident.EmployeeId = employee.Id;
        incident.Status = IncidentStatus.Accepted;
        incident.AssigningAccountId = accountService.GetCurrentAccount().Id;
        unitOfWork.Incidents.Update(incident);
        await unitOfWork.CompleteAsync();
    }

    public async Task RejectIncident(Guid id)
    {
        var incident = await GetIncidentById(id);
        if (incident.IncidentType == IncidentType.Interaction)
            throw new BadRequestException($"Cannot assign employee for interaction #{incident.Id}");

        // if previously accepted then remove employee id
        incident.Status = IncidentStatus.Rejected;
        incident.EmployeeId = null;
        incident.AssigningAccountId = accountService.GetCurrentAccount().Id;
        unitOfWork.Incidents.Update(incident);
        await unitOfWork.CompleteAsync();
    }

    public async Task AcceptOrRejectAllIncidents(List<Guid> incidentIds, Guid employeeId, bool isAccept)
    {
        if (incidentIds.Count == 0)
            throw new BadRequestException("Incident ids cannot be empty");
        var incidents = await unitOfWork
            .Incidents.GetAsync(
                expression: i =>
                    incidentIds.Contains(i.Id) && i.Shop.ShopManagerId == accountService.GetCurrentAccount().Id,
                takeAll: true
            )
            .ContinueWith(t => t.Result.Values);
        if (isAccept)
        {
            var employee = await employeeService.GetEmployeeById(employeeId);
            if (incidents.Any(i => i.ShopId != employee.ShopId))
                throw new BadRequestException("Incident and employee are not in the same shop");
        }
        var invalidIncidents = incidents
            .Where(i => i.IncidentType == IncidentType.Interaction)
            .Select(i => i.Id)
            .ToList();
        if (invalidIncidents.Count != 0)
            throw new BadRequestException(
                $"Cannot assign employee for interaction {string.Join(", ", invalidIncidents)}"
            );
        foreach (var incident in incidents)
        {
            incident.EmployeeId = isAccept ? employeeId : null;
            incident.Status = isAccept ? IncidentStatus.Accepted : IncidentStatus.Rejected;
            incident.AssigningAccountId = accountService.GetCurrentAccount().Id;
            unitOfWork.Incidents.Update(incident);
        }

        await unitOfWork.CompleteAsync();
    }

    // TODO: squash incident
    // TODO: incident must all have the same employee or unassigned
    // TODO: incident must all be the same type
    // TODO: Phone incident must be 3 mins within each other
    // TODO: Uniform incident must be 10 mins within each other

    public async Task<Incident?> UpsertIncident(CreateIncidentDto incidentDto)
    {
        var incident = (await unitOfWork.Incidents.GetAsync(i => i.Id == incidentDto.Id)).Values.FirstOrDefault();
        var isNewIncident = incident == null;

        var ebInstall = await edgeBoxInstallService.GetLatestInstallingByEdgeBox(incidentDto.EdgeBoxId);
        if (ebInstall == null)
            return null;

        foreach (var camera in incidentDto.Evidences.Select(x => x.Camera))
            await cameraService.CreateCameraIfNotExist(camera);

        await unitOfWork.BeginTransaction();
        if (incident == null)
        {
            incident = mapping.Map<CreateIncidentDto, Incident>(incidentDto);
            incident.ShopId = ebInstall.ShopId;

            var assignment = await supervisorAssignmentService.GetLatestAssignmentByDate(
                incident.ShopId,
                DateTimeHelper.VNDateTime
            );
            incident.AssignmentId = assignment!.Id;
            incident.Evidences = [];
            incident = await unitOfWork.Incidents.AddAsync(incident);
        }

        if (incidentDto.EndTime != null)
            incident.EndTime = incidentDto.EndTime;

        HashSet<Evidence> newEvidences = [];
        foreach (var evidenceDto in incidentDto.Evidences)
        {
            var evidence = mapping.Map<CreateEvidenceDto, Evidence>(evidenceDto);
            evidence.IncidentId = incident.Id;
            var imageDto = new CreateImageDto
            {
                ContentType = MediaTypeNames.Image.Png,
                Filename = evidence.Id.ToString("N") + ".png",
                ImageBytes = evidenceDto.Content
            };
            evidence.Image = await blobService.UploadImage(imageDto, nameof(Incident), incident.Id.ToString("N"));
            await unitOfWork.Evidences.AddAsync(evidence);
            newEvidences.Add(evidence);
        }

        await unitOfWork.CompleteAsync();
        await unitOfWork.CommitTransaction();

        var eventType = isNewIncident ? IncidentEventType.NewIncident : IncidentEventType.MoreEvidence;

        if (!isNewIncident)
            incident.Evidences = newEvidences;

        var shopManager =
            (
                await unitOfWork.Accounts.GetAsync(expression: a =>
                    a.ManagingShop != null && a.ManagingShop.Id == incident.ShopId
                )
            ).Values.FirstOrDefault() ?? throw new NotFoundException("Couldn't find shop manager");
        incidentSubject.Notify(new CreatedOrUpdatedIncidentArgs(incident, eventType, shopManager.Id));
        return incident;
    }

    public async Task<IncidentCountDto> CountIncidentsByShop(
        Guid? shopId,
        DateOnly startDate,
        DateOnly endDate,
        ReportInterval interval,
        IncidentTypeRequestOption type
    )
    {
        // ------ VALIDATION ------

        var user = accountService.GetCurrentAccount();
        shopId = user.Role switch
        {
            Role.BrandManager when shopId == null => throw new BadRequestException("Please specify a shop id"),
            Role.ShopManager => user.ManagingShop?.Id ?? throw new ForbiddenException(user, typeof(Shop)),
            _ => shopId
        };

        var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
        if (user.Role == Role.BrandManager && user.BrandId != shop.BrandId)
            throw new ForbiddenException(user, shop);

        if (startDate > endDate)
            return new IncidentCountDto
            {
                ShopId = shopId.Value,
                Total = 0,
                StartDate = startDate,
                EndDate = endDate,
                Interval = interval,
                Data = []
            };

        // ------ QUERY ------

        var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = endDate.AddDays(1).ToDateTime(TimeOnly.MinValue);

        Expression<Func<Incident, bool>> criteria = type switch
        {
            IncidentTypeRequestOption.Phone
                => i =>
                    i.IncidentType == IncidentType.Phone
                    && i.ShopId == shopId
                    && i.StartTime >= startDateTime
                    && i.StartTime < endDateTime,
            IncidentTypeRequestOption.Uniform
                => i =>
                    i.IncidentType == IncidentType.Uniform
                    && i.ShopId == shopId
                    && i.StartTime >= startDateTime
                    && i.StartTime < endDateTime,
            IncidentTypeRequestOption.Interaction
                => i =>
                    i.IncidentType == IncidentType.Interaction
                    && i.ShopId == shopId
                    && i.StartTime >= startDateTime
                    && i.StartTime < endDateTime,
            IncidentTypeRequestOption.Incident
                => i =>
                    i.IncidentType != IncidentType.Interaction
                    && i.ShopId == shopId
                    && i.StartTime >= startDateTime
                    && i.StartTime < endDateTime,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        // var items = (await unitOfWork.Incidents.GetAsync(criteria, takeAll: true))
        //     .Values.GroupBy(i =>
        //         DateTimeHelper.CalculateTimeForInterval(i.StartTime, interval, startDate.ToDateTime(TimeOnly.MinValue))
        //     )
        //     .Select(group => new IncidentCountItemDto(group.Key, group.Count()))
        //     .ToList();

        var incidents = (await unitOfWork.Incidents.GetAsync(criteria, takeAll: true)).Values;
        var timeSpan = DateTimeHelper.MapTimeSpanFromTimeInterval(interval);
        var items = new List<IncidentCountItemDto>();
        for (var time = startDateTime; time < endDateTime; time += timeSpan)
        {
            var group = incidents.Where(i => i.StartTime >= time && i.StartTime < time + timeSpan).ToList();
            var count = group.Count;
            var average = group
                .Where(i => i.EndTime != null)
                .Select(i => (i.EndTime - i.StartTime)!.Value.TotalSeconds)
                .DefaultIfEmpty(0)
                .Average();
            if (type != IncidentTypeRequestOption.Interaction)
            {
                var byStatus = group.GroupBy(x => x.Status).ToDictionary(x => x.Key, x => x.Count());
                var byType = group.GroupBy(x => x.IncidentType).ToDictionary(x => x.Key, x => x.Count());
                items.Add(new IncidentCountItemDto(time, count, average) { Type = byType, Status = byStatus });
            }
            else
                items.Add(new IncidentCountItemDto(time, count, average));
        }

        var incidentCountDto = new IncidentCountDto
        {
            ShopId = shopId.Value,
            Total = items.Sum(i => i.Count),
            StartDate = startDate,
            EndDate = endDate,
            Interval = interval,
            Data = items
        };
        if (type != IncidentTypeRequestOption.Interaction)
        {
            incidentCountDto.Status = incidents.GroupBy(x => x.Status).ToDictionary(x => x.Key, x => x.Count());
            incidentCountDto.Type = incidents.GroupBy(x => x.IncidentType).ToDictionary(x => x.Key, x => x.Count());
        }
        return incidentCountDto;
    }

    public async Task<IncidentPercentDto> GetIncidentPercent(
        Guid? shopId,
        DateOnly startDate,
        DateOnly endDate,
        ICollection<IncidentType> types
    )
    {
        // ------ VALIDATION ------

        var user = accountService.GetCurrentAccount();
        shopId = user.Role switch
        {
            Role.BrandManager when shopId == null => throw new BadRequestException("Please specify a shop id"),
            Role.ShopManager => user.ManagingShop?.Id ?? throw new ForbiddenException(user, typeof(Shop)),
            _ => shopId
        };

        var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
        if (user.Role == Role.BrandManager && user.BrandId != shop.BrandId)
            throw new ForbiddenException(user, shop);

        if (startDate > endDate)
            return new IncidentPercentDto
            {
                ShopId = shopId.Value,
                Total = 0,
                StartDate = startDate,
                EndDate = endDate
            };

        // ------ QUERY ------

        var incidents = (
            await unitOfWork.Incidents.GetAsync(
                i =>
                    i.ShopId == shopId
                    && types.Contains(i.IncidentType)
                    && i.StartTime >= startDate.ToDateTime(TimeOnly.MinValue)
                    && i.StartTime < endDate.AddDays(1).ToDateTime(TimeOnly.MinValue),
                takeAll: true
            )
        ).Values;

        var total = incidents.Count;
        var statusPercentDictionary = incidents
            .GroupBy(i => i.Status)
            .ToDictionary(
                group => group.Key,
                group =>
                {
                    var groupTotal = group.Count();
                    return new IncidentStatusPercentDto
                    {
                        Status = group.Key,
                        Total = groupTotal,
                        Percent = Calculator.Divide(groupTotal, (double)total)
                    };
                }
            );
        var statusPercent = Enum.GetValues<IncidentStatus>()
            .Select(status =>
            {
                if (!statusPercentDictionary.TryGetValue(status, out var value))
                    return new IncidentStatusPercentDto
                    {
                        Status = status,
                        Total = 0,
                        Percent = 0
                    };
                return value;
            })
            .ToList();

        var percentByTypeDictionary = incidents
            .GroupBy(i => i.IncidentType)
            .ToDictionary(
                group => group.Key,
                group =>
                {
                    var groupTotal = group.Count();
                    var typeStatusPercentDictionary = group
                        .GroupBy(i => i.Status)
                        .ToDictionary(
                            statusGroup => statusGroup.Key,
                            statusGroup =>
                            {
                                var statusGroupTotal = statusGroup.Count();
                                return new IncidentStatusPercentDto
                                {
                                    Status = statusGroup.Key,
                                    Total = statusGroupTotal,
                                    Percent = Calculator.Divide(statusGroupTotal, (double)groupTotal)
                                };
                            }
                        );
                    var typeStatusPercent = Enum.GetValues<IncidentStatus>()
                        .Select(status =>
                        {
                            if (!typeStatusPercentDictionary.TryGetValue(status, out var value))
                                return new IncidentStatusPercentDto
                                {
                                    Status = status,
                                    Total = 0,
                                    Percent = 0
                                };
                            return value;
                        })
                        .ToList();
                    return new IncidentTypePercentDto
                    {
                        Type = group.Key,
                        Total = groupTotal,
                        Percent = Calculator.Divide(groupTotal, (double)total),
                        Statuses = typeStatusPercent
                    };
                }
            );
        var percentByType = IncidentTypes
            .Select(type =>
            {
                if (!percentByTypeDictionary.TryGetValue(type, out var value))
                    return new IncidentTypePercentDto
                    {
                        Type = type,
                        Total = 0,
                        Percent = 0,
                        Statuses = Enum.GetValues<IncidentStatus>()
                            .Select(status => new IncidentStatusPercentDto
                            {
                                Status = status,
                                Total = 0,
                                Percent = 0
                            })
                            .ToList()
                    };
                return value;
            })
            .ToList();

        return new IncidentPercentDto
        {
            ShopId = shopId.Value,
            Total = total,
            Statuses = statusPercent,
            StartDate = startDate,
            EndDate = endDate,
            Types = percentByType
        };
    }

    public async Task<IList<Incident>> GetIncidentsByAssignment(Guid assignmentId)
    {
        var assignment =
            await unitOfWork.SupervisorAssignments.GetByIdAsync(assignmentId)
            ?? throw new NotFoundException(typeof(SupervisorAssignment), assignmentId);
        var startTime = assignment.StartTime;
        var endTime = assignment.EndTime ?? startTime.Date.AddDays(1).AddTicks(-1);
        var account = accountService.GetCurrentAccount();
        Expression<Func<Incident, bool>> criteria = account.Role switch
        {
            Role.ShopManager
                => x => startTime <= x.StartTime && x.StartTime <= endTime && x.ShopId == assignment.ShopId,
            Role.ShopSupervisor
                => x => startTime <= x.StartTime && x.StartTime <= endTime && x.Assignment!.SupervisorId == account.Id,
            _ => throw new ForbiddenException("Account must be shop manager or shop supervisor")
        };
        var incidents = await unitOfWork.Incidents.GetAsync(
            criteria,
            takeAll: true,
            includeProperties: [nameof(Incident.Assignment), "Evidences"]
        );
        return incidents.Values;
    }
}
