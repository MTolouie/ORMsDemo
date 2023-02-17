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

    public List<Company> GetAllCompaniesWithEmployees()
    {
        var sql = "SELECT C.*,E.* FROM Employees AS E INNER JOIN Companies AS C ON C.CompanyId = E.CompanyId ";

        var companyDict = new Dictionary<int, Company>();

        var company = _db.Query<Company, Employee, Company>(sql, (c, e) => 
        {
          if(!companyDict.TryGetValue(c.CompanyId,out var currentCompany))
            {
                currentCompany = c;
                companyDict.Add(c.CompanyId, currentCompany);
            }
            currentCompany.Employees.Add(e);
            return currentCompany;
        },splitOn:"EmployeeId");

        return company.Distinct().ToList();
    }

    public Company GetCompanyWithEmployees(int companyId)
    {
        var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId; ";
        sql += "SELECT * FROM Employees WHERE CompanyId = @CompanyId;";

        using var List = _db.QueryMultiple(sql, new {CompanyId = companyId });

        Company company = new Company();

        company = List.Read<Company>().ToList().FirstOrDefault();
        company.Employees = List.Read<Employee>().ToList();

        return company;
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
