using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Options;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerV2.Application;

public static class ApplicationDependencyWebApi
{
    public static IServiceCollection ConfigureApplicationServicesWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        return services
        .AddScoped<IAuthenticationAppService, AuthenticationAppService>()
        .AddScoped<IContactAppService, ContactAppService>()
        .AddScoped<ITransactionsAppService, TransactionsAppService>()
        .AddScoped<ISubCategoryAppService, SubCategoryAppService>()
        .AddScoped<IPasswordHelper, PasswordHelper>()
        .AddScoped<ICategoryAppService, CategoryAppService>();

    }
}
