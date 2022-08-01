using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ParkyApi.Models;
using ParkyApi.Models.Interfaces;
using ParkyApi.Repositories.Interfaces;

namespace ParkyApi.Filters;

/*
 var httpStatusCode = (HttpStatusCode) objectResult
    .GetType()
    .GetProperty("StatusCode")
    .GetValue(objectResult, null);
 */

public class TransactionActionFilter : IAsyncActionFilter
{
    private readonly IParkyContext _parkyContext;

    public TransactionActionFilter(IParkyContext parkyContext)
    {
        _parkyContext = parkyContext;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await BeginTransactionAsync();

        var resultContext = await next();

        await SaveChangesAsync(resultContext);
    }

    private async Task BeginTransactionAsync()
    {
        await _parkyContext.BeginTransactionAsync();
    }

    private async Task SaveChangesAsync(ActionExecutedContext resultContext)
    {
        try
        {
            var objectResult = (resultContext.Result as ObjectResult)?.Value;
            
            if (objectResult is IApiResult { Success: true })
            {
                await _parkyContext.CommitAsync();
            }
            else
            {
                await _parkyContext.RollbackAsync();
            }
        }
        catch (Exception)
        {
            const string message = "An unexpected error occurred";

            var apiResultRollback = new ApiResult(false, message);

            resultContext.Result = new BadRequestObjectResult(apiResultRollback);

            await _parkyContext.RollbackAsync();
        }
    }
}