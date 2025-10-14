using ExpenseTrackerV2.Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerV2.Application;

public static class ApplicationDependencyWebApi
{
    public static IServiceCollection ConfigureApplicationServicesWebApi(this IServiceCollection services) => services
        .AddScoped<OrganzationAppService>();
}
