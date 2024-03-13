using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface ICustomEmployeeRepository : IRepository<Employee>
{
    void DeleteEmployeeInShop(Guid shopId);
}
