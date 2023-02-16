using Dapper;
using EF_Core_Demo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EF_Core_Demo.Repository;

public class BonusRepository : IBonusRepository
{

    private IDbConnection _db;

    public BonusRepository(IConfiguration configuration)
    {
        this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }



    public List<Employee> GetEmployeesWithCompany(int companyId)
    {
        var sql = "SELECT E.*,C.* FROM Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId ";

        if (companyId != 0)
            sql += "WHERE E.CompanyId = @CompanyId";

        List<Employee> employees = _db.Query<Employee, Company, Employee>(sql, (emp, comp) =>
        {
            emp.Company = comp;
            return emp;
        }, new { CompanyId = companyId }, splitOn: "CompanyId").ToList();

        return employees;

    }
}
