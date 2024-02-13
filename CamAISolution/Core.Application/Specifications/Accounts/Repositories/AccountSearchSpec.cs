using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class AccountSearchSpec : RepositorySpec<Account>
{
    private static Expression<Func<Account, bool>> GetExpression(SearchAccountRequest req, Account reqAccount)
    {
        var baseSpec = new Specification<Account>();

        // Exclude the requesting account
        baseSpec.And(new AccountByIdSpec(reqAccount.Id).Not());

        if (!string.IsNullOrWhiteSpace(req.Name))
            baseSpec.And(new AccountByNameSpec(req.Name.Trim()));

        if (!string.IsNullOrWhiteSpace(req.Email))
            baseSpec.And(new AccountByEmailSpec(req.Email.Trim()));

        if (req.AccountStatusId.HasValue)
            baseSpec.And(new AccountByStatusSpec(req.AccountStatusId.Value));

        if (req.RoleId.HasValue)
            baseSpec.And(new AccountByRoleSpec(req.RoleId.Value));

        if (req.BrandId.HasValue)
            baseSpec.And(new AccountByBrandSpec(req.BrandId.Value));

        return baseSpec.GetExpression();
    }

    public AccountSearchSpec(SearchAccountRequest req, Account creatingAccount, bool includeAll = true)
        : base(GetExpression(req, creatingAccount))
    {
        base.ApplyingPaging(req);
        base.ApplyOrderByDescending(a => a.CreatedDate);
        if (includeAll)
        {
            AddIncludes(a => a.Roles);
            AddIncludes(a => a.AccountStatus);
            AddIncludes(a => a.Brand!);
            AddIncludes(a => a.Ward!.District.Province);
            AddIncludes(a => a.ManagingShop!);
        }
    }
}
