using Betatalks.UnitTesting.Api;

namespace Betatalks.UnitTesting.Api.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> Create(Employee employee);
    Task<IList<Employee>> GetAsync();
}