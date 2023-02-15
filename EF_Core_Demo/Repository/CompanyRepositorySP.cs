using Dapper;
using EF_Core_Demo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EF_Core_Demo.Repository;

public class CompanyRepositorySP : ICompanyRepository
{
    private IDbConnection _db;

    public CompanyRepositorySP(IConfiguration configuration)
    {
        this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public Company Add(Company company)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@CompanyId",0,DbType.Int32,direction:ParameterDirection.Output);
        parameters.Add("@Name", company.Name,DbType.String);
        parameters.Add("@Address", company.Address, DbType.String);
        parameters.Add("@City", company.City, DbType.String);
        parameters.Add("@State", company.State, DbType.String);
        parameters.Add("@PostalCode", company.PostalCode, DbType.String);

        _db.Execute("usp_AddCompany",parameters,commandType:CommandType.StoredProcedure);
        company.CompanyId = parameters.Get<int>("CompanyId");

        return company;
    }

    public Company Find(int id)
    {
        var company = _db.Query<Company>("usp_GetCompany", new {CompanyId = id }, commandType: CommandType.StoredProcedure).Single();
        return company;
    }

    public List<Company> GetAll()
    {
        var companies = _db.Query<Company>("usp_GetALLCompany",commandType: CommandType.StoredProcedure).ToList();
        return companies;
    }

    public void Remove(int id)
    {
        _db.Execute("usp_RemoveCompany", new { @CompanyId = id},commandType:CommandType.StoredProcedure);
    }

    public Company Update(Company company)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CompanyId",company.CompanyId);
        parameters.Add("@Name", company.Name);
        parameters.Add("@Address", company.Address);
        parameters.Add("@City", company.City);
        parameters.Add("@State", company.State);
        parameters.Add("@PostalCode", company.PostalCode);

        _db.Execute("usp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);
        var updatedCompany = Find(company.CompanyId);

        return updatedCompany;
    }
}
