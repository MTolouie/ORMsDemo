using Dapper;
using EF_Core_Demo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EF_Core_Demo.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private IDbConnection _db;

    public EmployeeRepository(IConfiguration configuration)
    {
        this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public Employee Add(Employee employee)
    {
        var sql = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                       + "SELECT CAST(SCOPE_IDENTITY() as int); ";
        var id = _db.Query<int>(sql, employee).Single();
        employee.CompanyId = id;
        return employee;
    }

    public Employee Find(int id)
    {
        var sql = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";
        var employee  = _db.Query<Employee>(sql, new { EmployeeId = id }).Single();
        return employee;
    }

    public List<Employee> GetAll()
    {
        var sql = "SELECT * FROM Employees";
        var employees = _db.Query<Employee>(sql).ToList();
        return employees;
    }

    public void Remove(int id)
    {
        var sql = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
        _db.Execute(sql, new { @EmployeeId = id});
    }

    public Employee Update(Employee employee)
    {
        var sql = "UPDATE Employees SET Name = @Name, Title = @Title, Email = @Email, Phone = @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
        _db.Execute(sql, employee);

        var updatedEmployee = Find(employee.EmployeeId);

        return updatedEmployee;
    }
}
