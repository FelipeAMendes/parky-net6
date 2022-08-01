using Microsoft.AspNetCore.Authentication.Cookies;
using ParkyWeb.Repositories.Interfaces;
using ParkyWeb.Services;
using ParkyWeb.Services.Interfaces;
using Refit;

namespace ParkyWeb.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<INationalParkService, NationalParkService>();
        services.AddScoped<ITrailService, TrailService>();
        services.AddScoped<IUserService, UserService>();
    }

    public static void AddCookieJwt(this IServiceCollection services)
    {
        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.SlidingExpiration = true;
                options.LoginPath = "/Home/Login";
                options.LogoutPath = "/Home/Logout";
                options.AccessDeniedPath = "/Home/AccessDenied";
            });
    }

    public static void AddApisConfiguration(this IServiceCollection services)
    {
        ConfigureApiNationalPark(services);
        ConfigureApiTrails(services);
        ConfigureApiUsers(services);
    }

    public static void ConfigureApiNationalPark(IServiceCollection services)
    {
        var urlApi = Environment.GetEnvironmentVariable("API_BASE_URL");

        services.AddRefitClient<INationalParkRepository>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(urlApi);
                c.Timeout = TimeSpan.FromSeconds(30);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, _, _, _) => true })
            .AddPolicyHandler((listServices, _) => PollyRetryPolicies.GetRetryPolicy<INationalParkRepository>(listServices))
            .AddPolicyHandler((listServices, _) => PollyRetryPolicies.GetCircuitBreakerPolicy<INationalParkRepository>(listServices));
    }

    public static void ConfigureApiTrails(IServiceCollection services)
    {
        var urlApi = Environment.GetEnvironmentVariable("API_BASE_URL");

        services.AddRefitClient<ITrailRepository>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(urlApi);
                c.Timeout = TimeSpan.FromSeconds(30);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, _, _, _) => true })
            .AddPolicyHandler((listServices, _) => PollyRetryPolicies.GetRetryPolicy<ITrailRepository>(listServices))
            .AddPolicyHandler((listServices, _) => PollyRetryPolicies.GetCircuitBreakerPolicy<ITrailRepository>(listServices));
    }

    public static void ConfigureApiUsers(IServiceCollection services)
    {
        var urlApi = Environment.GetEnvironmentVariable("API_BASE_URL");

        services.AddRefitClient<IUserRepository>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(urlApi);
                c.Timeout = TimeSpan.FromSeconds(30);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, _, _, _) => true })
            .AddPolicyHandler((listServices, _) => PollyRetryPolicies.GetRetryPolicy<IUserRepository>(listServices))
            .AddPolicyHandler((listServices, _) => PollyRetryPolicies.GetCircuitBreakerPolicy<IUserRepository>(listServices));
    }
}