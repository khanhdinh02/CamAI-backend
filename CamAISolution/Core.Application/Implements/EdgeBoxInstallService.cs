using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Specifications.EdgeBoxInstalls.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Events;
using Core.Domain.Interfaces.Emails;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.Publishers;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class EdgeBoxInstallService(
    IUnitOfWork unitOfWork,
    IBaseMapping mapper,
    IAccountService accountService,
    IEmailService emailService,
    IMessageQueueService messageQueueService,
    IApplicationDelayEventListener applicationDelayEventListener,
    IEdgeBoxService edgeBoxService
) : IEdgeBoxInstallService
{
    private const int CodeLength = 16;

    public async Task<EdgeBoxInstall> LeaseEdgeBox(CreateEdgeBoxInstallDto dto)
    {
        var edgeBox =
            await unitOfWork.EdgeBoxes.GetByIdAsync(dto.EdgeBoxId)
            ?? throw new NotFoundException(typeof(EdgeBox), dto.EdgeBoxId);
        if (edgeBox.EdgeBoxStatus != EdgeBoxStatus.Active || edgeBox.EdgeBoxLocation != EdgeBoxLocation.Idle)
        {
            throw new BadRequestException("Edge box is not active or idle");
        }

        var shop =
            (
                await unitOfWork.Shops.GetAsync(
                    s => s.Id == dto.ShopId,
                    includeProperties: [$"{nameof(Shop.Brand)}.{nameof(Brand.BrandManager)}"]
                )
            ).Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Shop), dto.ShopId);
        if (shop.ShopStatus == ShopStatus.Inactive)
            throw new BadRequestException("Shop is inactive");

        var ebInstall = mapper.Map<CreateEdgeBoxInstallDto, EdgeBoxInstall>(dto);
        ebInstall.ActivationCode = RandomGenerator.GetAlphanumericString(CodeLength);
        ebInstall.EdgeBoxInstallStatus = EdgeBoxInstallStatus.New;
        ebInstall.ActivationStatus = EdgeBoxActivationStatus.NotActivated;
        await unitOfWork.EdgeBoxInstalls.AddAsync(ebInstall);

        await edgeBoxService.UpdateLocation(
            edgeBox.Id,
            EdgeBoxLocation.Installing,
            $"Lease edge box to Shop#{shop.Id}"
        );

        // Send Activation code to brand manager via email
        _ = emailService.SendEmailAsync(
            shop.Brand.BrandManager!.Email, // Because at lease 1 shop exists, Brand Manager should exist
            "Your Edge Box activation code",
            EmailGenerator.GenerateActivationCodeEmail(shop.Brand.BrandManager.Name, ebInstall.ActivationCode)
        );

        return ebInstall;
    }

    public async Task<EdgeBoxInstall> ActivateEdgeBox(ActivateEdgeBoxDto dto)
    {
        var user = accountService.GetCurrentAccount();
        var ebInstall =
            (
                await unitOfWork.EdgeBoxInstalls.GetAsync(
                    i =>
                        i.ActivationCode == dto.ActivationCode
                        && i.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Disabled
                        && i.ShopId == dto.ShopId
                        && i.Shop.BrandId == user.BrandId,
                    includeProperties: [nameof(EdgeBoxInstall.EdgeBox)],
                    disableTracking: false
                )
            ).Values.FirstOrDefault() ?? throw new NotFoundException("Wrong activation code");

        if (ebInstall.EdgeBox.EdgeBoxLocation != EdgeBoxLocation.Occupied)
            throw new BadRequestException(
                "Edge box has not been installed to shop. Please wait for admin to install edge box"
            );

        if (ebInstall.ActivationStatus != EdgeBoxActivationStatus.Activated)
        {
            await unitOfWork.EdgeBoxActivities.AddAsync(
                new EdgeBoxActivity
                {
                    Type = EdgeBoxActivityType.EdgeBoxActivation,
                    EdgeBoxInstallId = ebInstall.Id,
                    Description = $"Manager activates edge box #{ebInstall.EdgeBoxId}"
                }
            );

            await UpdateActivationStatus(
                ebInstall.Id,
                EdgeBoxActivationStatus.Pending,
                "Waiting for edge box to confirm activation"
            );

            await messageQueueService.Publish(
                new ActivatedEdgeBoxMessage
                {
                    Message = "Activate edge box",
                    RoutingKey = ebInstall.EdgeBoxId.ToString("N")
                }
            );

            await applicationDelayEventListener.AddEvent(
                $"ActivateEdgeBox{ebInstall.Id:N}",
                new EdgeBoxAfterActivationFailedDelayEvent(TimeSpan.FromMinutes(5), ebInstall.EdgeBoxId, ebInstall.Id),
                true
            );
        }

        return ebInstall;
    }

    public async Task<EdgeBoxInstall> UpdateStatus(
        Guid edgeBoxInstallId,
        EdgeBoxInstallStatus status,
        string? description = null
    )
    {
        var ebInstall =
            await unitOfWork.EdgeBoxInstalls.GetByIdAsync(edgeBoxInstallId)
            ?? throw new NotFoundException(typeof(EdgeBoxInstall), edgeBoxInstallId);
        return await UpdateStatus(ebInstall, status, description);
    }

    public async Task<EdgeBoxInstall> UpdateStatus(
        EdgeBoxInstall edgeBoxInstall,
        EdgeBoxInstallStatus status,
        string? description = null
    )
    {
        if (edgeBoxInstall.EdgeBoxInstallStatus == status)
            return edgeBoxInstall;

        await unitOfWork.EdgeBoxActivities.AddAsync(
            new EdgeBoxActivity
            {
                Type = EdgeBoxActivityType.EdgeBoxHealth,
                Description = description ?? $"Update status from {edgeBoxInstall.EdgeBoxInstallStatus} to {status}",
                EdgeBoxInstallId = edgeBoxInstall.Id
            }
        );
        edgeBoxInstall.EdgeBoxInstallStatus = status;
        unitOfWork.EdgeBoxInstalls.Update(edgeBoxInstall);
        await unitOfWork.CompleteAsync();

        return edgeBoxInstall;
    }

    public async Task<EdgeBoxInstall> UpdateActivationStatus(
        Guid edgeBoxInstallId,
        EdgeBoxActivationStatus status,
        string? description = null
    )
    {
        var ebInstall =
            await unitOfWork.EdgeBoxInstalls.GetByIdAsync(edgeBoxInstallId)
            ?? throw new NotFoundException(typeof(EdgeBoxInstall), edgeBoxInstallId);

        if (ebInstall.ActivationStatus == status)
            return ebInstall;

        await unitOfWork.EdgeBoxActivities.AddAsync(
            new EdgeBoxActivity
            {
                Type = EdgeBoxActivityType.EdgeBoxActivation,
                Description = description ?? $"Update activation status from {ebInstall.ActivationStatus} to {status}",
                EdgeBoxInstallId = ebInstall.Id
            }
        );

        ebInstall.ActivationStatus = status;
        unitOfWork.EdgeBoxInstalls.Update(ebInstall);

        await unitOfWork.CompleteAsync();
        return ebInstall;
    }

    public async Task<EdgeBoxInstall?> GetLatestInstallingByEdgeBox(Guid edgeBoxId)
    {
        return (
            await unitOfWork.EdgeBoxInstalls.GetAsync(
                i => i.EdgeBoxId == edgeBoxId && i.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Disabled,
                includeProperties:
                [
                    $"{nameof(EdgeBoxInstall.EdgeBox)}.{nameof(EdgeBox.EdgeBoxModel)}",
                    $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Brand)}",
                    $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Ward)}.{nameof(Ward.District)}.{nameof(District.Province)}"
                ],
                pageSize: 1
            )
        ).Values.FirstOrDefault();
    }

    public Task<PaginationResult<EdgeBoxInstall>> GetEdgeBoxInstall(SearchEdgeBoxInstallRequest searchRequest)
    {
        return unitOfWork.EdgeBoxInstalls.GetAsync(new EdgeBoxInstallSearchSpec(searchRequest));
    }

    public async Task<EdgeBoxInstall?> GetCurrentInstallationByShop(Guid shopId)
    {
        var user = accountService.GetCurrentAccount();
        var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
        if (
            (user.Role == Role.BrandManager && shop.BrandId != user.BrandId)
            || (user.Role == Role.ShopManager && shop.ShopManagerId != user.Id)
        )
            throw new ForbiddenException(user, shop);

        return (
            await unitOfWork.EdgeBoxInstalls.GetAsync(
                i => i.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Disabled && i.ShopId == shopId,
                includeProperties:
                [
                    $"{nameof(EdgeBoxInstall.EdgeBox)}.{nameof(EdgeBox.EdgeBoxModel)}",
                    $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Brand)}",
                    $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Ward)}.{nameof(Ward.District)}.{nameof(District.Province)}"
                ],
                takeAll: true
            )
        ).Values.FirstOrDefault();
    }

    public async Task<PaginationResult<EdgeBoxInstall>> GetInstallingByShop(Guid shopId)
    {
        var user = accountService.GetCurrentAccount();
        var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
        if (
            (user.Role == Role.BrandManager && shop.BrandId != user.BrandId)
            || (user.Role == Role.ShopManager && shop.ShopManagerId != user.Id)
        )
        {
            throw new ForbiddenException(user, shop);
        }

        var installs = (
            await unitOfWork.EdgeBoxInstalls.GetAsync(
                i => i.ShopId == shopId,
                includeProperties:
                [
                    $"{nameof(EdgeBoxInstall.EdgeBox)}.{nameof(EdgeBox.EdgeBoxModel)}",
                    $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Brand)}",
                    $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Ward)}.{nameof(Ward.District)}.{nameof(District.Province)}"
                ],
                takeAll: true
            )
        ).Values;

        if (user.Role != Role.Admin)
            foreach (var i in installs)
                i.ActivationCode = null;

        return new PaginationResult<EdgeBoxInstall>
        {
            PageIndex = 0,
            PageSize = installs.Count,
            TotalCount = installs.Count,
            Values = installs
        };
    }

    public async Task<PaginationResult<EdgeBoxInstall>> GetInstallingByBrand(Guid brandId)
    {
        var user = accountService.GetCurrentAccount();
        var brand =
            await unitOfWork.Brands.GetByIdAsync(brandId) ?? throw new NotFoundException(typeof(Brand), brandId);
        if (user.Role == Role.BrandManager && brandId != user.BrandId)
        {
            throw new ForbiddenException(user, brand);
        }

        var installs = (
            await unitOfWork.EdgeBoxInstalls.GetAsync(
                i => i.Shop.BrandId == brandId,
                includeProperties:
                [
                    $"{nameof(EdgeBoxInstall.EdgeBox)}.{nameof(EdgeBox.EdgeBoxModel)}",
                    $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Brand)}",
                    $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Ward)}.{nameof(Ward.District)}.{nameof(District.Province)}"
                ],
                takeAll: true
            )
        ).Values;

        if (user.Role != Role.Admin)
            foreach (var i in installs)
                i.ActivationCode = null;

        return new PaginationResult<EdgeBoxInstall>
        {
            PageIndex = 0,
            PageSize = installs.Count,
            TotalCount = installs.Count,
            Values = installs
        };
    }

    public async Task UninstallEdgeBox(Guid installId)
    {
        var install =
            await unitOfWork.EdgeBoxInstalls.GetByIdAsync(installId)
            ?? throw new NotFoundException(typeof(EdgeBoxInstall), installId);
        var edgeBox =
            await unitOfWork.EdgeBoxes.GetByIdAsync(install.EdgeBoxId)
            ?? throw new NotFoundException(typeof(EdgeBox), install.EdgeBoxId);

        // Make sure that the edge box is being assigned for a shop
        if ((await GetLatestInstallingByEdgeBox(install.EdgeBoxId))?.Id != installId)
            return;

        (var description, edgeBox.EdgeBoxLocation) = install.EdgeBoxInstallStatus switch
        {
            // If edge box is not installed yet, just cancel the installation
            EdgeBoxInstallStatus.New
                => ("Cancel edge box installation", EdgeBoxLocation.Idle),
            _ => ("Uninstall edge box", EdgeBoxLocation.Uninstalling)
        };
        install.UninstalledTime = DateTimeHelper.VNDateTime;
        install.EdgeBoxInstallStatus = EdgeBoxInstallStatus.Disabled;
        unitOfWork.EdgeBoxInstalls.Update(install);
        unitOfWork.EdgeBoxes.Update(edgeBox);
        await unitOfWork.EdgeBoxActivities.AddAsync(
            new EdgeBoxActivity
            {
                Type = EdgeBoxActivityType.EdgeBoxUninstall,
                EdgeBoxId = install.EdgeBoxId,
                EdgeBoxInstallId = installId,
                Description = description
            }
        );
        await unitOfWork.CompleteAsync();
    }

    public async Task<EdgeBoxInstall> GetEdgeBoxInstallById(Guid edgeBoxInstallId)
    {
        var edgeBoxInstall =
            (
                await unitOfWork.EdgeBoxInstalls.GetAsync(
                    expression: ei => ei.Id == edgeBoxInstallId,
                    includeProperties: [nameof(EdgeBoxInstall.EdgeBox), nameof(EdgeBoxInstall.Shop),]
                )
            ).Values.First() ?? throw new NotFoundException(typeof(EdgeBoxInstall), edgeBoxInstallId);
        return edgeBoxInstall;
    }

    public Task<IList<EdgeBoxInstall>> GetAllEdgeBoxInstall()
    {
        return unitOfWork.EdgeBoxInstalls.GetAsync(takeAll: true).ContinueWith(t => t.Result.Values);
    }

    public async Task<EdgeBoxInstall> UpdateIpAddress(EdgeBoxInstall edgeBoxInstall, string ipAddress)
    {
        if (edgeBoxInstall.IpAddress == ipAddress)
            return edgeBoxInstall;

        edgeBoxInstall.IpAddress = ipAddress;
        unitOfWork.EdgeBoxInstalls.Update(edgeBoxInstall);
        await unitOfWork.CompleteAsync();

        return edgeBoxInstall;
    }

    public async Task<EdgeBoxInstall> UpdateLastSeen(EdgeBoxInstall edgeBoxInstall)
    {
        edgeBoxInstall.LastSeen = DateTimeHelper.VNDateTime;
        edgeBoxInstall.NotificationSent = false;
        unitOfWork.EdgeBoxInstalls.Update(edgeBoxInstall);
        await unitOfWork.CompleteAsync();
        return edgeBoxInstall;
    }

    public Task UpdateEdgeBoxHealthByLastSeen(TimeSpan elapsedTime)
    {
        var lastTime = DateTimeHelper.VNDateTime - elapsedTime;
        unitOfWork.EdgeBoxInstalls.UpdateStatusBy(
            EdgeBoxInstallStatus.Unhealthy,
            x =>
                x.LastSeen < lastTime
                && x.EdgeBox.EdgeBoxLocation == EdgeBoxLocation.Occupied
                && x.EdgeBoxInstallStatus == EdgeBoxInstallStatus.Working
        );
        return Task.CompletedTask;
    }

    public async Task<EdgeBoxInstall> UpdateNotificationSent(EdgeBoxInstall edgeBoxInstall, bool notificationSent)
    {
        edgeBoxInstall.LastSeen = DateTimeHelper.VNDateTime;
        edgeBoxInstall.NotificationSent = false;
        unitOfWork.EdgeBoxInstalls.Update(edgeBoxInstall);
        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch (Exception) { }
        return edgeBoxInstall;
    }
}
