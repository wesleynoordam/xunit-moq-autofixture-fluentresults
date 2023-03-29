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
