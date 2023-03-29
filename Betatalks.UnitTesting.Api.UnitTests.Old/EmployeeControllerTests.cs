using Betatalks.UnitTesting.Api.Controllers;
using Betatalks.UnitTesting.Api.Interfaces;
using Newtonsoft.Json;
using Xunit;

namespace Betatalks.UnitTesting.Api.UnitTests.Old;

public class EmployeeControllerTests
{
    [Fact]
    public async Task CreateAsync_ShouldCallRepositoryAndReturnEmployee()
    {
        var mockEmployeeRepository = new MockEmployeeRepository();
        var sut = new EmployeeController(mockEmployeeRepository);

        var employee = new Employee
        {
            Id = 1,
            Name = "Jake"
        };

        var result = await sut.CreateAsync(employee);

        var resultJson = JsonConvert.SerializeObject(result);
        var expectedJson = JsonConvert.SerializeObject(employee);

        Assert.Equal(resultJson, expectedJson);
    }


    [Fact]
    public async Task GetAsync_RetrieveEmployees()
    {
        var mockEmployeeRepository = new MockEmployeeRepository();
        var sut = new EmployeeController(mockEmployeeRepository);

        var expected = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = "Jake"
            },
            new Employee
            {
                Id = 1,
                Name = "Steve"
            }
        };

        var result = await sut.GetAsync();

        var resultJson = JsonConvert.SerializeObject(result);
        var expectedJson = JsonConvert.SerializeObject(expected);

        Assert.Equal(resultJson, expectedJson);
    }
}

public class MockEmployeeRepository : IEmployeeRepository
{
    public Task<Employee?> Create(Employee employee)
    {
        var newEmployee = new Employee
        {
            Id = employee.Id,
            Name = employee.Name
        };

        return Task.FromResult<Employee?>(newEmployee);
    }

    public Task<IList<Employee>> GetAsync()
    {
        var employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = "Jake"
            },
            new Employee
            {
                Id = 1,
                Name = "Steve"
            }
        };

        return Task.FromResult<IList<Employee>>(employees);
    }
}