using Backend.TechChallenge.Api.Features.Users.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Backend.TechChallenge.Api.Configuration;

public static class IocContainerConfiguration
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppConfig>(configuration.GetSection("AppConfig"));

        return services;
    }

    public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddSingleton(Log.Logger);

        return services;
    }
    public static IServiceCollection AddUsersFeature(this IServiceCollection services)
    {
        services.AddSingleton<IUserMoneyCalculator, UserMoneyCalculator>();
        services.AddSingleton<IUserRepository, UserRepository>();

        return services;
    }
}
