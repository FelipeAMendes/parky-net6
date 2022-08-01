using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyApi.Configurations;
using ParkyApi.Data;
using ParkyApi.Filters;
using ParkyApi.Mappings;
using ParkyApi.Repositories;
using ParkyApi.Repositories.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace ParkyApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptionsConfiguration>();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(connectionString!));

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<TransactionActionFilter>();
        services.AddScoped<IParkyContext, ParkyContext>();
        services.AddScoped<INationalParkRepository, NationalParkRepository>();
        services.AddScoped<ITrailRepository, TrailRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ParkyMappings));

        return services;
    }

    public static IServiceCollection AddJwtToken(this IServiceCollection services)
    {
        var secret = Environment.GetEnvironmentVariable("JwtSecret");
        var key = Encoding.UTF8.GetBytes(secret!);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        return services;
    }
}