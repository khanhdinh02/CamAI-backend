using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class AccountSearchSpec : RepositorySpec<Account>
{
    private static Expression<Func<Account, bool>> GetExpression(SearchAccountRequest req)
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
            baseSpec.And(new AccountByShopSpec(req.ShopId.Value));

        return baseSpec.GetExpression();
    }

    public AccountSearchSpec(SearchAccountRequest req)
        : base(GetExpression(req))
    {
        base.ApplyingPaging(req);
        base.ApplyOrderByDescending(a => a.CreatedDate);
        AddIncludes(a => a.Roles);
        AddIncludes(a => a.AccountStatus);
        AddIncludes(nameof(Account.Brand));
        AddIncludes(a => a.Ward!.District.Province);
        AddIncludes(a => a.ManagingShop!);
        AddIncludes(a => a.WorkingShop!);
    }
}
