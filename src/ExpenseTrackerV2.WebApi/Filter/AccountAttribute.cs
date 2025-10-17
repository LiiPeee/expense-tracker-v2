using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTrackerV2.WebApi.Filter;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AccountRequestAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetService<ILogger<AccountFilter>>();

        return new AccountFilter(
            logger
        );
    }
}
