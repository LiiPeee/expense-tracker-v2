using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerV2.Infrastructure;

public static class InfrastructureDependencyWebApi
{
    public static IServiceCollection AddInfrastructureWebApi(this IServiceCollection services) => services
    .AddSingleton<DapperContext>()
    .AddScoped<IOrganizationRepository, OrganizationRepository>();
}
