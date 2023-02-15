using Dapper;
using Dapper.Contrib.Extensions;
using EF_Core_Demo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EF_Core_Demo.Repository;

public class CompanyRepositoryContrib : ICompanyRepository
{
    private IDbConnection _db;

    public CompanyRepositoryContrib(IConfiguration configuration)
    {
        this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public Company Add(Company company)
    {
        var id = _db.Insert(company);
        company.CompanyId = (int)id;
        return company;
    }

    public Company Find(int id)
    {
        var company = _db.Get<Company>(id);
        return company;
    }

    public List<Company> GetAll()
    {
        var companies = _db.GetAll<Company>().ToList();
        return companies;
    }

    public void Remove(int id)
    {
        _db.Delete<Company>(new Company() {CompanyId = id });
    }
    
    public Company Update(Company company)
    {
        _db.Update<Company>(company);

        var updatedCompany = Find(company.CompanyId);

        return updatedCompany;
    }
}
