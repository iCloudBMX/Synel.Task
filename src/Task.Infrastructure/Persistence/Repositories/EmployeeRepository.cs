using Microsoft.EntityFrameworkCore;
using Task.Domain.Entities;

namespace Task.Infrastructure.Persistence.Repositories;

internal class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext appDbContext;

    public EmployeeRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public async Task<Employee> InsertEmployeeAsync(Employee employee)
    {
        this.appDbContext.Entry(employee).State = EntityState.Added;

        await this.appDbContext.SaveChangesAsync();

        return employee;
    }

    public async System.Threading.Tasks.Task InsertEmployeeRangeAsync(List<Employee> employees)
    {
        this.appDbContext.AddRange(employees);

        await this.appDbContext.SaveChangesAsync();
    }

    public async Task<Employee?> SelectEmployeeByIdAsync(Guid id) =>
        await this.appDbContext.Employees.FindAsync(id);

    public IQueryable<Employee> SelectEmployees() => this.appDbContext.Employees;

    public async Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
        this.appDbContext.Entry(employee).State = EntityState.Modified;

        await this.appDbContext.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee> DeleteEmployeeAsync(Employee employee)
    {
        this.appDbContext.Entry(employee).State = EntityState.Deleted;

        await this.appDbContext.SaveChangesAsync();

        return employee;
    }
}