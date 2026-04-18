using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using ExpenseTrackerV2.Core.Infrastructure.Repository;
using ExpenseTrackerV2.Core.Infrastructure.Services;
using ExpenseTrackerV2.Infrastructure.Persistence.Repository;
using ExpenseTrackerV2.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerV2.Infrastructure;

public static class InfrastructureDependencyWebApi
{
    public static IServiceCollection AddInfrastructureWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        return services
        .AddScoped<DbSession>()
        .AddScoped<IAccountRepository, AccountRepository>()
        .AddScoped<ITransactionsRepository, TransactionsRepository>()
        .AddScoped<ICategoryRepository, CategoryRepository>()
        .AddScoped<IAddressRepository, AddressRepository>()
        .AddScoped<IContactRepository, ContactRepository>()
        .AddScoped<ISubCategoryRepository, SubCategoryRepository>()
        .AddScoped<IUnitOfWork, UnitOfWork>()
        .AddScoped<IEmailService, EmailService>();
    }
}
