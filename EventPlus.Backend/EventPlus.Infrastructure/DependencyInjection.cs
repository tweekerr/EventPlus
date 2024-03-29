using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ng.Services;

namespace EventPlus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUserAgentService();

        return services;
    } 
}