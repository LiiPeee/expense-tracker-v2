using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Infrastructure.Repository;
using BudgetTracker.Core.Infrastructure.Services;
using BudgetTracker.Infrastructure.Persistence.Repository;
using BudgetTracker.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetTracker.Infrastructure;

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
        .AddScoped<IResetPasswordRepository, ResetPasswordRepository>()
        .AddScoped<IUnitOfWork, UnitOfWork>()
        .AddScoped<IEmailService, EmailService>();
    }
}


