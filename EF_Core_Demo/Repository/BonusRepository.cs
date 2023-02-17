using Dapper;
using EF_Core_Demo.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace EF_Core_Demo.Repository;

public class BonusRepository : IBonusRepository
{

    private IDbConnection _db;

    public BonusRepository(IConfiguration configuration)
    {
        this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public void AddCompanyWithEmployees(Company company)
    {
        var sqlForCompany = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                       + "SELECT CAST(SCOPE_IDENTITY() as int); ";
        var id = _db.Query<int>(sqlForCompany, company).Single();
        company.CompanyId = id;

        var sqlForEmployees = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);";
        //+ "SELECT CAST(SCOPE_IDENTITY() as int); ";

        //foreach (var employee in company.Employees)
        //{
        //    employee.CompanyId = company.CompanyId;
        //    _db.Execute(sqlForEmployees,employee);
        //}

        company.Employees.Select(emp => { emp.CompanyId = id; return emp; }).ToList();

        _db.Execute(sqlForEmployees, company.Employees);


    }


    public void AddCompanyWithEmployeesWithTransaction(Company company)
    {
        
        try
        {
            using var transaction = new TransactionScope();

            var sqlForCompany = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                       + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            var id = _db.Query<int>(sqlForCompany, company).Single();
            company.CompanyId = id;

            var sqlForEmployees = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);";

            company.Employees.Select(emp => { emp.CompanyId = id; return emp; }).ToList();

            _db.Execute(sqlForEmployees, company.Employees);

            transaction.Complete();
        }
        catch (Exception ex)
        {
            Console.WriteLine("/////////////////////////////");
            Console.WriteLine(ex.Message);
            Console.WriteLine("/////////////////////////////");
        }

    }

    public List<Company> FIlterCompaniesByName(string name)
    {
        var sql = "SELECT * FROM Companies WHERE Name Like '%'+ @name +'%'";
        var companies = _db.Query<Company>(sql, new { name = name }).ToList();
        return companies;
    }

    public List<Company> GetAllCompaniesWithEmployees()
    {
        var sql = "SELECT C.*,E.* FROM Employees AS E INNER JOIN Companies AS C ON C.CompanyId = E.CompanyId ";

        var companyDict = new Dictionary<int, Company>();

        var company = _db.Query<Company, Employee, Company>(sql, (c, e) =>
        {
            if (!companyDict.TryGetValue(c.CompanyId, out var currentCompany))
            {
                currentCompany = c;
                companyDict.Add(c.CompanyId, currentCompany);
            }
            currentCompany.Employees.Add(e);
            return currentCompany;
        }, splitOn: "EmployeeId");

        return company.Distinct().ToList();
    }

    public Company GetCompanyWithEmployees(int companyId)
    {
        var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId; ";
        sql += "SELECT * FROM Employees WHERE CompanyId = @CompanyId;";

        using var List = _db.QueryMultiple(sql, new { CompanyId = companyId });

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

    public void RemoveRange(int[] companyIds)
    {
        var sql = "DELETE FROM Companies WHERE CompanyId IN @companyIds";
        _db.Execute(sql, new { companyIds = companyIds });
    }
}
