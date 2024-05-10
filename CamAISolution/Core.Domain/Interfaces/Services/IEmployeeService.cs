using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface IEmployeeService
{
    Task<PaginationResult<Employee>> GetEmployees(SearchEmployeeRequest req);
    Task<Employee> GetEmployeeById(Guid id);
    Task<Employee> CreateEmployee(CreateEmployeeDto dto);
    Task<Employee> UpdateEmployee(Guid id, UpdateEmployeeDto dto);
    Task DeleteEmployee(Guid id);
    Task<BulkUpsertTaskResultResponse> UpsertEmployees(Guid actorId, MemoryStream stream, string taskId);
}
