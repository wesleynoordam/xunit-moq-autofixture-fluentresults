using AutoFixture.Xunit2;
using Betatalks.UnitTesting.Api.Controllers;
using Betatalks.UnitTesting.Api.Interfaces;
using Betatalks.UnitTesting.Api.UnitTests.AutoMoq;
using FluentAssertions;
using Moq;
using Xunit;

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