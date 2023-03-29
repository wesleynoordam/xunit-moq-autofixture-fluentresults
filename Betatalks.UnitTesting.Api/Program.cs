using Betatalks.UnitTesting.Api.Interfaces;
using Betatalks.UnitTesting.Api.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Betatalks.UnitTesting.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("EmployeeDatabase");
        var connection = new SqlConnection(connectionString);

        builder.Services
            .AddDbContext<DbContext, EmployeeDbContext>(options => options
            .UseSqlServer(connection));

        builder.Services.AddControllers();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        var app = builder.Build();

        app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}