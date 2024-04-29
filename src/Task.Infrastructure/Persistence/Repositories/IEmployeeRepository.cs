using Task.Domain.Entities;
namespace Task.Infrastructure.Persistence.Repositories;

public interface IEmployeeRepository
{
    Task<Employee> InsertEmployeeAsync(Employee employee);
    System.Threading.Tasks.Task InsertEmployeeRangeAsync(List<Employee> employees);
    Task<Employee?> SelectEmployeeByIdAsync(Guid id);
    IQueryable<Employee> SelectEmployees();
    Task<Employee> UpdateEmployeeAsync(Employee employee);
    Task<Employee> DeleteEmployeeAsync(Employee employee);
}