using BCD.Application.Common.Interfaces;
using BCD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBCDInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BCDContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });

        services.AddTransient<IBCDContext>(provider => provider.GetRequiredService<BCDContext>());

        services.AddTransient<BCDContextSeeder>();
        return services;
    }
}
