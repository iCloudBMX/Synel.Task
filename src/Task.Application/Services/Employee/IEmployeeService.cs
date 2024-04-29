using Task.Application.Contracts;
using Task.Domain.Entities;

namespace Task.Application.Services;

public interface IEmployeeService
{
    Task<Result> AddEmployeeAsync(Employee employee);
    Task<Result> AddEmployeeRangeAsync(List<Employee> employees);
    Task<Result<Grid<Employee>>> RetrieveEmployeesAsync(GridParameters parameters);
    Task<Result<Employee>> RetrieveEmployeeByIdAsync(Guid employeeId);
    Task<Result> ModifyEmployeeAsync(Employee employee);
    Task<Result> RemoveEmployeeAsync(Guid employeeId);
}