using Dapper;
using EF_Core_Demo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EF_Core_Demo.Repository;

public class CompanyRepository : ICompanyRepository
{
    private IDbConnection _db;

    public CompanyRepository(IConfiguration configuration)
    {
        this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public Company Add(Company company)
    {
        var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                       + "SELECT CAST(SCOPE_IDENTITY() as int); ";
        var id = _db.Query<int>(sql, company).Single();
        company.CompanyId = id;
        return company;
    }

    public Company Find(int id)
    {
        var sql = "SELECT * FROM companies WHERE CompanyId = @CompanyId";
        var company = _db.Query<Company>(sql, new { CompanyId = id }).Single();
        return company;
    }

    public List<Company> GetAll()
    {
        var sql = "SELECT * FROM Companies";
        var companies = _db.Query<Company>(sql).ToList();
        return companies;
    }

    public void Remove(int id)
    {
        var sql = "DELETE FROM Companies WHERE CompanyId = @CompanyId";
        _db.Execute(sql, new { @CompanyId = id});
    }

    public Company Update(Company company)
    {
        var sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, " +
                  "State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
        _db.Execute(sql, company);

        var updatedCompany = Find(company.CompanyId);

        return updatedCompany;
    }
}
