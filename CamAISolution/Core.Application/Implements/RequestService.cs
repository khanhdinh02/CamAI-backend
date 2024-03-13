using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;

namespace Core.Application.Implements;

public class RequestService(IUnitOfWork unitOfWork, IBaseMapping mapper, IAccountService accountService)
    : IRequestService
{
    public async Task<PaginationResult<Request>> GetRequests(SearchRequestRequest req)
    {
        var user = accountService.GetCurrentAccount();
        switch (user.Role)
        {
            case Role.Admin:
                break;
            case Role.BrandManager:
                req.BrandId = user.BrandId;
                break;
            case Role.ShopManager:
                req.ShopId = user.ManagingShop?.Id ?? throw new BadRequestException("Shop manager must have a shop");
                req.BrandId = null;
                break;
            default:
                throw new ForbiddenException(user, typeof(Request));
        }

        return await unitOfWork.Requests.GetAsync(new RequestSearchSpec(req));
    }

    public async Task<PaginationResult<Request>> GetPersonalRequests(SearchPersonalRequestRequest req)
    {
        var user = accountService.GetCurrentAccount();
        return await unitOfWork.Requests.GetAsync(new PersonalRequestSearchSpec(req, user.Id));
    }

    public async Task<Request> GetRequestById(Guid requestId)
    {
        var request =
            (await unitOfWork.Requests.GetAsync(new RequestByIdRepoSpec(requestId))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Request), requestId);

        var user = accountService.GetCurrentAccount();
        if (user.Role == Role.Admin || request.AccountId == user.Id)
            return request;
        if (
            (user.Role == Role.BrandManager && request.Account.BrandId != user.BrandId)
            || (user.Role == Role.ShopManager && (user.ManagingShop == null || request.ShopId != user.ManagingShop.Id))
        )
            throw new ForbiddenException(user, typeof(Request));
        return request;
    }

    public async Task<Request> CreateRequest(CreateRequestDto dto)
    {
        var user = accountService.GetCurrentAccount();
        switch (dto.RequestType)
        {
            case RequestType.Install:
                if (user.Role != Role.BrandManager)
                    throw new ForbiddenException(user, typeof(Request));
                return await CreateInstallRequest(dto, user);
            case RequestType.Remove:
            // TODO [Khanh]: Handle creating other request types
            case RequestType.Repair:
            case RequestType.Other:
                throw new NotImplementedException();
            default:
                throw new BadRequestException("Invalid request type");
        }
    }

    public async Task<Request> UpdateRequest(Guid requestId, UpdateRequestDto dto)
    {
        var request =
            await unitOfWork.Requests.GetByIdAsync(requestId)
            ?? throw new NotFoundException(typeof(Request), requestId);

        if (dto.Reply != null)
            await Reply(request, dto.Reply);
        if (dto.RequestStatus.HasValue)
            await UpdateStatus(request, dto.RequestStatus.Value);

        return request;
    }

    public async Task<Request> UpdateStatus(Request request, RequestStatus status)
    {
        if (request.RequestStatus == status)
            return request;

        await unitOfWork
            .GetRepository<RequestActivity>()
            .AddAsync(
                new RequestActivity
                {
                    RequestId = request.Id,
                    OldStatus = request.RequestStatus,
                    NewStatus = status
                }
            );

        request.RequestStatus = status;
        unitOfWork.Requests.Update(request);

        await unitOfWork.CompleteAsync();
        return request;
    }

    public async Task<Request> Reply(Request request, string reply)
    {
        request.Reply = reply;
        unitOfWork.Requests.Update(request);
        await unitOfWork.CompleteAsync();
        return request;
    }

    private async Task<Request> CreateInstallRequest(CreateRequestDto dto, Account user)
    {
        if (dto.ShopId == null)
            throw new BadRequestException("Shop id is required for install request");
        var shop =
            await unitOfWork.Shops.GetByIdAsync(dto.ShopId) ?? throw new NotFoundException(typeof(Shop), dto.ShopId);
        if (user.BrandId != shop.BrandId)
            throw new ForbiddenException(user, typeof(Shop));

        var request = mapper.Map<CreateRequestDto, Request>(dto);
        request.EdgeBoxId = null;
        request.AccountId = user.Id;
        request.RequestStatus = RequestStatus.Open;
        await unitOfWork.Requests.AddAsync(request);
        await unitOfWork.CompleteAsync();
        return request;
    }
}
