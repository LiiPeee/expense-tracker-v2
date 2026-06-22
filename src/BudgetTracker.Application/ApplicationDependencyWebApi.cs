using BudgetTracker.Application.Service;
using BudgetTracker.Core.Domain.Options;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Domain.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetTracker.Application;

public static class ApplicationDependencyWebApi
{
    public static IServiceCollection ConfigureApplicationServicesWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        return services
        .AddScoped<IAuthenticationAppService, AuthenticationAppService>()
        .AddScoped<IContactAppService, ContactAppService>()
        .AddScoped<ITransactionsAppService, TransactionsAppService>()
        .AddScoped<ISubCategoryAppService, SubCategoryAppService>()
        .AddScoped<IBudgetLimitService, BudgetLimitService>()
        .AddScoped<IPasswordHelper, PasswordHelper>()
        .AddScoped<ICategoryAppService, CategoryAppService>()
        .AddScoped<IStockAppService, StockAppService>();

    }
}


