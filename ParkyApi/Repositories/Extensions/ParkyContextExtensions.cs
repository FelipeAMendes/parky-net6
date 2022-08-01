using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ParkyApi.Extensions;
using Polly;
using Polly.Retry;

namespace ParkyApi.Repositories.Extensions;

public static class ParkyContextExtensions
{
    private const int CountRetry = 3;

    private static readonly AsyncRetryPolicy AsyncRetryPolicy = Policy.Handle<SqlException>(ex => SqlExceptionCodes.ErrorCodes.Contains(ex.Number))
        .WaitAndRetryAsync(CountRetry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    public static async Task ExecuteStrategyAndRetry(this ParkyContext _, DbContext dbContext, Func<Task> operation)
    {
        async Task ExecuteStrategy()
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(operation);
        }

        await AsyncRetryPolicy.ExecuteAsync(ExecuteStrategy);
    }

    public static async Task<TResult> ExecuteStrategyAndRetry<TResult>(this ParkyContext _, DbContext dbContext, Func<Task<TResult>> operation)
    {
        async Task<TResult> ExecuteStrategy()
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();

            var result = await strategy.ExecuteAsync(operation);

            return result;
        }

        var executeRetry = await AsyncRetryPolicy.ExecuteAsync(ExecuteStrategy);

        return executeRetry;
    }

    public static async Task<TResult> ExecuteRetry<TResult>(this ParkyContext _, Func<Task<TResult>> operation)
    {
        var executionRetry = await AsyncRetryPolicy.ExecuteAsync(operation);

        return executionRetry;
    }
}