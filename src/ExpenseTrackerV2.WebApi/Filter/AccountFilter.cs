using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTrackerV2.WebApi.Filter;

public class AccountFilter(ILogger<AccountFilter> logger) : IActionFilter
{
    private readonly ILogger<AccountFilter> _logger = logger;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("INICIOU A CRIAÇÂO DA CONTA");

        var request = context.HttpContext.Request;
        var controller = context.RouteData.Values["controller"];
        var method = context.RouteData.Values["action"];

        _logger.LogInformation($"Request feita {request}");
        _logger.LogInformation($"Controller onde foi feito a request {controller}");
        _logger.LogInformation($"Endpoint {method}");

        if (context.ActionArguments.Count > 0)
        {
            var json = JsonSerializer.Serialize(context.ActionArguments.Values);
            _logger.LogInformation($"json {json[0]}");
        }
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

}
