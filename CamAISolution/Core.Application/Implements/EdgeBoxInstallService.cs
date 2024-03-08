using Core.Application.Exceptions;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;

namespace Core.Application.Implements;

public class EdgeBoxInstallService(IUnitOfWork unitOfWork, IBaseMapping mapper, IAccountService accountService)
    : IEdgeBoxInstallService
{
    public async Task<EdgeBoxInstall> LeaseEdgeBox(CreateEdgeBoxInstallDto dto)
    {
        var edgeBox =
            await unitOfWork.EdgeBoxes.GetByIdAsync(dto.EdgeBoxId)
            ?? throw new NotFoundException(typeof(EdgeBox), dto.EdgeBoxId);
        if (edgeBox.EdgeBoxStatus != EdgeBoxStatus.Active || edgeBox.EdgeBoxLocation != EdgeBoxLocation.Idle)
            throw new BadRequestException("Edge box is not active or idle");

        if (dto.ValidFrom >= dto.ValidUntil)
            throw new BadRequestException("ValidFrom must be smaller than ValidUntil");

        var ebInstall = mapper.Map<CreateEdgeBoxInstallDto, EdgeBoxInstall>(dto);
        ebInstall.EdgeBoxInstallSubscription = EdgeBoxInstallSubscription.Inactive;
        await unitOfWork.EdgeBoxInstalls.AddAsync(ebInstall);

        edgeBox.EdgeBoxLocation = EdgeBoxLocation.Installing;
        unitOfWork.EdgeBoxes.Update(edgeBox);

        await unitOfWork.CompleteAsync();
        return ebInstall;
    }

    public async Task<EdgeBoxInstall> ActivateEdgeBox(ActivateEdgeBoxDto dto)
    {
        var user = accountService.GetCurrentAccount();
        var ebInstall =
            (
                await unitOfWork
                    .EdgeBoxInstalls
                    .GetAsync(
                        i =>
                            i.ActivationCode == dto.ActivationCode
                            && i.ShopId == dto.ShopId
                            && i.Shop.BrandId == user.BrandId
                    )
            )
                .Values
                .FirstOrDefault() ?? throw new NotFoundException("Wrong activation code");

        if (ebInstall.EdgeBoxInstallSubscription == EdgeBoxInstallSubscription.Inactive)
        {
            ebInstall.EdgeBoxInstallSubscription = EdgeBoxInstallSubscription.Activated;
            unitOfWork.EdgeBoxInstalls.Update(ebInstall);
            await unitOfWork.CompleteAsync();

            // TODO: Send message to activate edge box
        }
        return ebInstall;
    }

    public async Task<EdgeBoxInstall> UpdateStatus(
        Guid edgeBoxInstallId,
        EdgeBoxInstallStatus status,
        EdgeBoxInstall? edgeBoxInstall = null
    )
    {
        edgeBoxInstall ??=
            await unitOfWork.GetRepository<EdgeBoxInstall>().GetByIdAsync(edgeBoxInstallId)
            ?? throw new NotFoundException(typeof(EdgeBoxInstall), edgeBoxInstallId);
        if (edgeBoxInstall.EdgeBoxInstallStatus != status)
        {
            await unitOfWork.BeginTransaction();
            await unitOfWork
                .GetRepository<EdgeBoxInstallActivity>()
                .AddAsync(
                    new()
                    {
                        Description = $"Update status from {edgeBoxInstall.EdgeBoxInstallStatus} to {status}",
                        NewStatus = status,
                        OldStatus = edgeBoxInstall.EdgeBoxInstallStatus,
                        EdgeBoxInstallId = edgeBoxInstallId,
                    }
                );
            edgeBoxInstall.EdgeBoxInstallStatus = status;
            unitOfWork.GetRepository<EdgeBoxInstall>().Update(edgeBoxInstall);

            await unitOfWork.CommitTransaction();
        }
        return edgeBoxInstall;
    }
}
