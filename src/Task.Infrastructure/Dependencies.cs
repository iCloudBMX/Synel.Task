using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Task.Infrastructure.Persistence;
using Task.Infrastructure.Persistence.Repositories;

namespace Task.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("SqlServer") ??
            throw new InvalidOperationException("Connection string not found");

        services.AddDbContext<AppDbContext>(options => options
            .UseSqlServer(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .LogTo(Console.WriteLine, LogLevel.Warning));

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }
}