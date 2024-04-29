using Microsoft.Extensions.DependencyInjection;
using Task.Application.Services;

namespace Task.Application;

public static class Dependencies
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services
            .AddScoped<ICsvService, CsvService>()
            .AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}