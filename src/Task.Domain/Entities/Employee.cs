namespace Task.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public string Payroll { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Telephone { get; set; }
    public string Mobile { get; set; }
    public string Address { get; set; }
    public string Address2 { get; set; }
    public string PostCode { get; set; }
    public string Email { get; set; }
    public string StartDate { get; set; }
}