using Microsoft.EntityFrameworkCore;

namespace Betatalks.UnitTesting.Api;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext()
    { }

    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
    { }

    public virtual DbSet<Employee> Employee { get; set; } = null!;
}
