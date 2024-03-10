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
        ebInstall.EdgeBoxInstallStatus = EdgeBoxInstallStatus.New;
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

        if (ebInstall.EdgeBoxInstallStatus == EdgeBoxInstallStatus.Connected)
        {
            await UpdateStatus(ebInstall, EdgeBoxInstallStatus.Working);

            // TODO: Send message to activate edge box
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
            await unitOfWork.BeginTransaction();
            await unitOfWork
                .GetRepository<EdgeBoxInstallActivity>()
                .AddAsync(
                    new EdgeBoxInstallActivity
                    {
                        Description = $"Update status from {edgeBoxInstall.EdgeBoxInstallStatus} to {status}",
                        NewStatus = status,
                        OldStatus = edgeBoxInstall.EdgeBoxInstallStatus,
                        EdgeBoxInstallId = edgeBoxInstall.Id,
                    }
                );
            edgeBoxInstall.EdgeBoxInstallStatus = status;
            unitOfWork.GetRepository<EdgeBoxInstall>().Update(edgeBoxInstall);

            await unitOfWork.CommitTransaction();
        }
        return edgeBoxInstall;
    }

    public async Task<EdgeBoxInstall?> GetInstallingByEdgeBox(Guid edgeBoxId)
    {
        return (
            await unitOfWork
                .EdgeBoxInstalls
                .GetAsync(
                    i => i.EdgeBoxId == edgeBoxId && i.EdgeBox.EdgeBoxLocation != EdgeBoxLocation.Idle,
                    o => o.OrderByDescending(i => i.ValidUntil),
                    [
                        nameof(EdgeBoxInstall.EdgeBox),
                        $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Brand)}",
                        $"{nameof(EdgeBoxInstall.Shop)}.{nameof(Shop.Ward)}.{nameof(Ward.District)}.{nameof(District.Province)}"
                    ]
                )
        )
            .Values
            .FirstOrDefault();
    }
}