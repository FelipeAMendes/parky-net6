using Polly;
using Polly.Extensions.Http;

namespace ParkyWeb.Extensions
{
    public class PollyRetryPolicies
    {
        public static TimeSpan ExponencialBackoffSleep(int retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy<T>(IServiceProvider services)
        {
            var asyncPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount: 2,
                    sleepDurationProvider: ExponencialBackoffSleep,
                    onRetry: LogError<T>(services, "WaitAndRetry"));

            return asyncPolicy;
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy<T>(IServiceProvider services)
        {
            var asyncPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 10,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: LogError<T>(services, "CircuitBreaker"),
                    onReset: () => { });

            return asyncPolicy;
        }

        private static Action<DelegateResult<HttpResponseMessage>, TimeSpan> LogError<T>(IServiceProvider services, string type)
        {
            return (response, waitFor) =>
            {
                var logger = services.GetRequiredService<ILogger<T>>();
                var message = $"{type} - Error ({response.Result?.StatusCode}) {response.Result?.ReasonPhrase}. retry in {waitFor}";
                logger.LogWarning(message: message);
            };
        }
    }
}
