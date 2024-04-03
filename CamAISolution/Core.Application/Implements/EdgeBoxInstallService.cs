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
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class EdgeBoxInstallService(
    IUnitOfWork unitOfWork,
    IBaseMapping mapper,
    IAccountService accountService,
    IEmailService emailService,
    IMessageQueueService messageQueueService,
    IApplicationDelayEventListener applicationDelayEventListener
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

        var ebInstall = mapper.Map<CreateEdgeBoxInstallDto, EdgeBoxInstall>(dto);
        ebInstall.ActivationCode = RandomGenerator.GetAlphanumericString(CodeLength);
        ebInstall.EdgeBoxInstallStatus = EdgeBoxInstallStatus.New;
        await unitOfWork.EdgeBoxInstalls.AddAsync(ebInstall);

        edgeBox.EdgeBoxLocation = EdgeBoxLocation.Installing;
        unitOfWork.EdgeBoxes.Update(edgeBox);
        await unitOfWork.EdgeBoxActivities.AddAsync(
            new EdgeBoxActivity
            {
                Type = EdgeBoxActivityType.EdgeBoxLocation,
                EdgeBoxId = edgeBox.Id,
                Description = $"Lease edge box to Shop#{shop.Id}"
            }
        );

        await unitOfWork.CompleteAsync();

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

        if (ebInstall.ActivationStatus == EdgeBoxActivationStatus.NotActivated)
        {
            if (ebInstall.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Working)
            {
                ebInstall.ActivationStatus = EdgeBoxActivationStatus.Failed;
                await unitOfWork.EdgeBoxActivities.AddAsync(
                    new EdgeBoxActivity
                    {
                        Type = EdgeBoxActivityType.EdgeBoxActivation,
                        EdgeBoxInstallId = ebInstall.Id,
                        Description = "Postpone activation due to unhealthy status"
                    }
                );
                await unitOfWork.CompleteAsync();
            }
            else
            {
                ebInstall.ActivationStatus = EdgeBoxActivationStatus.Pending;
                await unitOfWork.EdgeBoxActivities.AddAsync(
                    new EdgeBoxActivity
                    {
                        Type = EdgeBoxActivityType.EdgeBoxActivation,
                        EdgeBoxInstallId = ebInstall.Id,
                        Description = "Waiting for edge box to confirm activation"
                    }
                );
                if (await unitOfWork.CompleteAsync() > 0)
                {
                    await messageQueueService.Publish(
                        new ActivatedEdgeBoxMessage
                        {
                            Message = "Activate edge box",
                            RoutingKey = ebInstall.EdgeBoxId.ToString("N")
                        }
                    );

                    await applicationDelayEventListener.AddEvent(
                        $"ActivateEdgeBox{ebInstall.Id:N}",
                        new EdgeBoxAfterActivationFailedDelayEvent(
                            TimeSpan.FromMinutes(5),
                            ebInstall.EdgeBoxId,
                            ebInstall.Id
                        ),
                        true
                    );
                }
            }
        }

        return ebInstall;
    }

    public async Task<EdgeBoxInstall> UpdateStatus(Guid edgeBoxInstallId, EdgeBoxInstallStatus status)
    {
        var ebInstall =
            await unitOfWork.EdgeBoxInstalls.GetByIdAsync(edgeBoxInstallId)
            ?? throw new NotFoundException(typeof(EdgeBoxInstall), edgeBoxInstallId);
        return await UpdateStatus(ebInstall, status);
    }

    public async Task<EdgeBoxInstall> UpdateStatus(EdgeBoxInstall edgeBoxInstall, EdgeBoxInstallStatus status)
    {
        if (edgeBoxInstall.EdgeBoxInstallStatus != status)
        {
            await unitOfWork
                .GetRepository<EdgeBoxActivity>()
                .AddAsync(
                    new EdgeBoxActivity
                    {
                        Type = EdgeBoxActivityType.EdgeBoxHealth,
                        Description = $"Update status from {edgeBoxInstall.EdgeBoxInstallStatus} to {status}",
                        EdgeBoxInstallId = edgeBoxInstall.Id
                    }
                );
            edgeBoxInstall.EdgeBoxInstallStatus = status;
            unitOfWork.EdgeBoxInstalls.Update(edgeBoxInstall);
            await unitOfWork.EdgeBoxActivities.AddAsync(
                new EdgeBoxActivity
                {
                    Type = EdgeBoxActivityType.EdgeBoxHealth,
                    EdgeBoxInstallId = edgeBoxInstall.Id,
                    Description = $"Update status from {edgeBoxInstall.EdgeBoxInstallStatus} to {status}"
                }
            );
            await unitOfWork.CompleteAsync();
        }

        return edgeBoxInstall;
    }

    public async Task<EdgeBoxInstall?> GetLatestInstallingByEdgeBox(Guid edgeBoxId)
    {
        return (
            await unitOfWork.EdgeBoxInstalls.GetAsync(
                i => i.EdgeBoxId == edgeBoxId && i.EdgeBox.EdgeBoxLocation != EdgeBoxLocation.Idle,
                o => o.OrderByDescending(i => i.CreatedDate),
                [
                    nameof(EdgeBoxInstall.EdgeBox),
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
}
