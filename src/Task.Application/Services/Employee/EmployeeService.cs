using Microsoft.Extensions.Logging;
using Task.Application.Contracts;
using Task.Application.Extensions;
using Task.Domain;
using Task.Domain.Entities;
using Task.Infrastructure.Persistence.Repositories;

namespace Task.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository employeeRepository;
    private readonly ILogger<EmployeeService> logger;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        ILogger<EmployeeService> logger)
    {
        this.employeeRepository = employeeRepository;
        this.logger = logger;
    }

    public async Task<Result> AddEmployeeAsync(Employee employee)
    {
        try
        {
            await this.employeeRepository.InsertEmployeeAsync(employee);

            return Result.Success();
        }
        catch(Exception exception)
        {
            this.logger.LogError(exception.Message);
            return Result.Fail(Errors.UnableToPerformOperation);
        }
    }

    public async Task<Result> AddEmployeeRangeAsync(List<Employee> employees)
    {
        try
        {
            await this.employeeRepository.InsertEmployeeRangeAsync(employees);

            return Result.Success();
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception.Message);
            return Result.Fail(Errors.UnableToPerformOperation);
        }
    }

    public async Task<Result<Employee>> RetrieveEmployeeByIdAsync(Guid employeeId)
    {
        try
        {
            var maybeEmployee = await this.employeeRepository
                .SelectEmployeeByIdAsync(employeeId);

            if (maybeEmployee is null)
            {
                return Result<Employee>
                    .Fail(Errors.NotFound);
            }

            return Result<Employee>
                .Success(maybeEmployee);
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception.Message);
            
            return Result<Employee>
                .Fail(Errors.UnableToPerformOperation);
        }
    }

    public async Task<Result<Grid<Employee>>> RetrieveEmployeesAsync(
        GridParameters parameters)
    {
        try
        {
            if (parameters.Order is null)
            {
                parameters.Order = new Order() { Property = nameof(Employee.LastName) };
            }

            var employees = await this.employeeRepository
                .SelectEmployees()
                .GridAsync(parameters);

            return Result<Grid<Employee>>
                .Success(employees);
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception.Message);

            return Result<Grid<Employee>>
                .Fail(Errors.UnableToPerformOperation);
        }
    }

    public async Task<Result> ModifyEmployeeAsync(Employee employee)
    {
        try
        {
            var maybeEmployee = await this.employeeRepository
                .SelectEmployeeByIdAsync(employee.Id);

            if (maybeEmployee is null)
            {
                return Result.Fail(Errors.NotFound);
            }

            await this.employeeRepository
                .UpdateEmployeeAsync(employee);

            return Result.Success();
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception.Message);
            return Result.Fail(Errors.UnableToPerformOperation);
        }
    }

    public async Task<Result> RemoveEmployeeAsync(Guid employeeId)
    {
        try
        {
            var maybeEmployee = await this.employeeRepository
                .SelectEmployeeByIdAsync(employeeId);

            if (maybeEmployee is null)
            {
                return Result.Fail(Errors.NotFound);
            }

            await this.employeeRepository
                .DeleteEmployeeAsync(maybeEmployee);

            return Result.Success();
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception.Message);
            return Result.Fail(Errors.UnableToPerformOperation);
        }
    }
}
