using ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerV2.Infrastructure;

public static class InfrastructureDependencyWebApi
{
    public static IServiceCollection AddInfrastructureWebApi(this IServiceCollection services,
    string expenseTrackerConnection) => services
    .AddScoped(_ => new ExpenseTrackerDbContext(expenseTrackerConnection));
}
