# Betatalks UnitTesting
This repository contains examples of unit tests and issues which you may encounter. In `Betatalks.UnitTesting.Api.UnitTests.Old` are unit tests written out without packages. In `Betatalks.UnitTesting.Api.UnitTests` is the same exact test, but with multiple packages included.

## Context
<details>
<summary>Code samples</summary>

**EmployeeController.cs**
```csharp
using Betatalks.UnitTesting.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Betatalks.UnitTesting.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpPost]
    public async Task<Employee?> CreateAsync(Employee employee)
    {
        return await _employeeRepository.Create(employee);
    }

    [HttpGet]
    public async Task<IList<Employee>> GetAsync()
    {
        return await _employeeRepository.GetAsync();
    }
}

```

**EmployeeRepository.cs**
```csharp
using Betatalks.UnitTesting.Api;
using Betatalks.UnitTesting.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Betatalks.UnitTesting.Api.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _context;

    public EmployeeRepository(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> Create(Employee employee)
    {
        _context.Employee.Add(employee);
        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<IList<Employee>> GetAsync()
    {
        return await _context.Employee.ToListAsync();
    }
}
```
</details>
<br/><br/>

## Current Unit test

<details>
<summary>Unit test</summary>

**EmployeeControllerTests.cs**
```csharp
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
```

</details>

<br/><br/>

## Situation

<details>
<summary>Issues</summary>

1. Custom Mocking needed.
2. Potential of being reliant on specific data.
3. Checking the same objects is not possible out-of-the-box.

Result: **More time spended on writing unit tests.**
</details>
<br/>
<details>
<summary>Solution</summary>

Use of 3 extra packages simplifies unit testing and writing:
- AutoFixture
- Moq
- FluentAssertions

Nuget packages:
- AutoFixture.AutoMoq
- AutoFixture.Xunit2
- FluentAssertions
</details>
<br/>
<details>
<summary>Example</summary>

```csharp
using AutoFixture.Xunit2;
using Betatalks.UnitTesting.Api.Controllers;
using Betatalks.UnitTesting.Api.Interfaces;
using Betatalks.UnitTesting.Api.UnitTests.AutoMoq;
using FluentAssertions;
using Moq;

namespace Betatalks.UnitTesting.Api.UnitTests;

public class EmployeeControllerTests
{
    [Theory]
    [AutoMoqData]
    public async Task CreateAsync_ShouldCallRepositoryAndReturnEmployee(
        [Frozen] Mock<IEmployeeRepository> employeeRepositoryMock,
        EmployeeController sut,
        Employee employee,
        Employee expected)
    {
        employeeRepositoryMock
            .Setup(x => x.Create(It.IsAny<Employee>()))
            .ReturnsAsync(expected);

        var result = await sut.CreateAsync(employee);

        result
            .Should()
            .BeEquivalentTo(expected);
    }

    [Theory]
    [AutoMoqData]
    public async Task GetAsync_RetrieveEmployees(
        [Frozen] Mock<IEmployeeRepository> employeeRepositoryMock,
        EmployeeController sut,
        IList<Employee> expected)
    {
        employeeRepositoryMock
            .Setup(x => x.GetAsync())
            .ReturnsAsync(expected);

        var result = await sut.GetAsync();

        result
            .Should()
            .BeEquivalentTo(expected);
    }
}
```
</details>