using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Utilities;

namespace Core.Application.Specifications.Repositories;

public class AccountSearchSpec : RepositorySpec<Account>
{
    private static Expression<Func<Account, bool>> GetExpression(SearchAccountRequest req, Account creatingAccount)
    {
        var baseSpec = new Specification<Account>();
        if (!string.IsNullOrWhiteSpace(req.Search))
        {
            req.Search = req.Search.Trim();
            baseSpec.And(new AccountByEmailSpec(req.Search));
            baseSpec.Or(new AccountByNameSpec(req.Search));
            baseSpec.Or(new AccountByPhoneSpec(req.Search));
        }

        if (req.AccountStatusId.HasValue)
            baseSpec.And(new AccountByStatusSpec(req.AccountStatusId.Value));

        if (req.RoleId.HasValue)
            baseSpec.And(new AccountByRoleSpec(req.RoleId.Value));

        if (req.BrandId.HasValue)
            baseSpec.And(new AccountByBrandSpec(req.BrandId.Value));

        if (req.ShopId.HasValue)
        {
            baseSpec.And(new AccountByShopSpec(req.ShopId.Value));
            // If the highest role of the current user is shop manager, they can only see themself and employees of their shop
            if (!creatingAccount.HasRole(RoleEnum.Admin) && !creatingAccount.HasRole(RoleEnum.BrandManager))
                baseSpec.And(
                    new Specification<Account>(a => !a.Roles.Contains(new Role { Id = RoleEnum.BrandManager }))
                );
        }

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
            AddIncludes(a => a.WorkingShop!);
        }
    }
}
