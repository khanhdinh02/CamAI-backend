﻿using System.Linq.Expressions;
using Core.Application.Specifications;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application;

public class AccountNotificationSearchSpec : RepositorySpec<AccountNotification>
{
    private static Expression<Func<AccountNotification, bool>> GetExpresion(SearchNotificationRequest req)
    {
        var baseCriteria = new AccountNotificationByAccountIdSpec(req.AccountId);
        if (req.NotificationId.HasValue)
            baseCriteria.And(new AccountNotificationByNotificationIdSpec(req.NotificationId.Value));
        if (req.Status.HasValue)
            baseCriteria.And(new AccountNotificationByStatusSpec(req.Status.Value));
        return baseCriteria.GetExpression();
    }

    public AccountNotificationSearchSpec(SearchNotificationRequest req)
        : base(GetExpresion(req))
    {
        AddIncludes(a => a.Status);
        AddIncludes(a => a.Account);
        AddIncludes(a => a.Notification.SentBy);
        ApplyOrderByDescending(a => a.Notification.CreatedDate);
        ApplyingPaging(req);
    }
}
