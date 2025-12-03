using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerV2.Application;

public static class ApplicationDependencyWebApi
{
    public static IServiceCollection ConfigureApplicationServicesWebApi(this IServiceCollection services)
    {
        return services
        .AddScoped<IOrganizationAppService, OrganzationAppService>()
        .AddScoped<IAccountAppService,AccountAppService>()
        .AddScoped<ITransactionAppService, TransactionAppService>();

    }
}
