using System.Net.Mime;
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

namespace Core.Application.Implements;

public class IncidentService(
    IBlobService blobService,
    IAccountService accountService,
    IEmployeeService employeeService,
    ICameraService cameraService,
    IEdgeBoxInstallService edgeBoxInstallService,
    IUnitOfWork unitOfWork,
    IBaseMapping mapping
) : IIncidentService
{
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
        }

        var includeShop = account.Role == Role.BrandManager;
        return await unitOfWork.Incidents.GetAsync(new IncidentSearchSpec(searchRequest, includeShop));
    }

    public async Task AssignIncidentToEmployee(Guid id, Guid employeeId)
    {
        // TODO: should we limit the time or have some rule when assigning incident
        var incident = await GetIncidentById(id);
        var employee = await employeeService.GetEmployeeById(employeeId);
        if (incident.ShopId != employee.ShopId)
            throw new BadRequestException("Incident and employee are not in the same shop");
        incident.EmployeeId = employee.Id;
        incident.Status = IncidentStatus.Accepted;
        unitOfWork.Incidents.Update(incident);
        await unitOfWork.CompleteAsync();
    }

    public async Task RejectIncident(Guid id)
    {
        var incident = await GetIncidentById(id);
        // if previously accepted then remove employee id
        incident.Status = IncidentStatus.Rejected;
        incident.EmployeeId = null;
        unitOfWork.Incidents.Update(incident);
        await unitOfWork.CompleteAsync();
    }

    public async Task<Incident> UpsertIncident(CreateIncidentDto incidentDto)
    {
        var incident = await unitOfWork.Incidents.GetByIdAsync(incidentDto.Id);

        var ebInstall = await edgeBoxInstallService.GetLatestInstallingByEdgeBox(incidentDto.EdgeBoxId);
        foreach (var cameraId in incidentDto.Evidences.Select(x => x.CameraId))
            await cameraService.CreateCameraIfNotExist(cameraId, ebInstall!.ShopId);

        await unitOfWork.BeginTransaction();
        if (incident == null)
        {
            incident = mapping.Map<CreateIncidentDto, Incident>(incidentDto);
            incident.ShopId = ebInstall!.ShopId;
            incident.Evidences = [];
            incident = await unitOfWork.Incidents.AddAsync(incident);
        }

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
        }

        await unitOfWork.CompleteAsync();
        await unitOfWork.CommitTransaction();
        return incident;
    }

    public async Task<IncidentCountDto> CountIncidentsByShop(
        Guid? shopId,
        DateOnly startDate,
        ReportTimeRange timeRange
    )
    {
        var user = accountService.GetCurrentAccount();
        shopId = user.Role switch
        {
            Role.BrandManager when shopId == null => throw new BadRequestException("Please specify a shop id"),
            Role.ShopManager => user.ManagingShop?.Id,
            _ => shopId
        };

        var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
        if (
            (user.Role == Role.BrandManager && user.BrandId != shop.BrandId)
            || (user.Role == Role.ShopManager && user.ManagingShop?.Id != shopId)
        )
            throw new ForbiddenException(user, shop);

        var (endDate, interval, groupByKey) = timeRange switch
        {
            ReportTimeRange.Day
                => (
                    startDate.AddDays(1),
                    ReportInterval.Hour,
                    (Func<Incident, DateTime>)(
                        i => new DateTime(DateOnly.FromDateTime(i.StartTime), new TimeOnly(i.StartTime.Hour))
                    )
                ),
            ReportTimeRange.Week => (startDate.AddDays(7), ReportInterval.Day, i => i.StartTime.Date),
            ReportTimeRange.Month => (startDate.AddDays(30), ReportInterval.Day, i => i.StartTime.Date),
            _ => throw new ArgumentOutOfRangeException(nameof(timeRange), timeRange, null)
        };

        var items = (
            await unitOfWork.Incidents.GetAsync(i =>
                i.ShopId == shopId
                && i.StartTime >= startDate.ToDateTime(TimeOnly.MinValue)
                && i.EndTime < endDate.ToDateTime(TimeOnly.MinValue)
            )
        )
            .Values.GroupBy(groupByKey)
            .Select(group => new IncidentCountItemDto(group.Key, group.Count()))
            .ToList();

        return new IncidentCountDto
        {
            Total = items.Sum(i => i.Count),
            StartDate = startDate,
            EndDate = endDate.AddDays(-1),
            Interval = interval,
            Data = items
        };
    }
}
