using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface IRequestService
{
    Task<PaginationResult<Request>> GetRequests(SearchRequestRequest req);
    Task<PaginationResult<Request>> GetPersonalRequests(SearchPersonalRequestRequest req);
    Task<Request> GetRequestById(Guid requestId);
    Task<Request> CreateRequest(CreateRequestDto dto);
    Task<Request> UpdateRequest(Guid requestId, UpdateRequestDto dto);
    Task<Request> UpdateStatus(Request request, RequestStatus status);
    Task<Request> Reply(Request request, string reply);
}
