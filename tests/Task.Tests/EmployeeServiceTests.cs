using Microsoft.Extensions.Logging;
using Moq;
using Task.Application.Services;
using Task.Domain;
using Task.Domain.Entities;
using Task.Infrastructure.Persistence.Repositories;

namespace Task.Tests;


public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> employeeRepositoryMock;
    private readonly Mock<ILogger<EmployeeService>> loggerMock;
    private readonly IEmployeeService employeeService;

    public EmployeeServiceTests()
    {
        this.employeeRepositoryMock = new Mock<IEmployeeRepository>();
        this.loggerMock= new Mock<ILogger<EmployeeService>>();

        this.employeeService = new EmployeeService(
            this.employeeRepositoryMock.Object,
            this.loggerMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task ShouldReturnNotFoundMessageIfIdIsInvalid()
    {
        // arrange
        Guid randomEmployeeId = Guid.NewGuid();
        Employee? noEmployee = null;

        this.employeeRepositoryMock.Setup(r =>
            r.SelectEmployeeByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(noEmployee);
        
        // act
        var result = await this.employeeService
            .RetrieveEmployeeByIdAsync(randomEmployeeId);

        // assert
        Assert.Equal(Errors.NotFound, result.Message);
        Assert.False(result.Succeeded);
        Assert.Null(result.Data);
    }
}
