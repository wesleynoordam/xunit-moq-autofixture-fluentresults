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