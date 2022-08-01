using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using Polly;

namespace ParkyApi.Extensions;

public static class WebApplicationExtensions
{
    public static void UseSwaggerConfiguration(this WebApplication app)
    {
        var apiVersionDescriptionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                options.SwaggerEndpoint(
                    url: $"/swagger/{description.GroupName}/swagger.json",
                    name: description.GroupName.ToUpperInvariant());

            options.RoutePrefix = "";
        });
    }

    public static void ExecuteMigrations(this WebApplication app)
    {
        var migrateDbPolicyHandle = Policy.Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

        migrateDbPolicyHandle.Execute(() =>
        {
            using var scope = app.Services.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            applicationDbContext.Database.Migrate();
        });
    }
}