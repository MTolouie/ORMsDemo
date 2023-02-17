using EF_Core_Demo.Models;

namespace EF_Core_Demo.Repository;

public interface IBonusRepository
{
    public List<Employee> GetEmployeesWithCompany(int companyId);
    public Company GetCompanyWithEmployees(int companyId);
    public List<Company> GetAllCompaniesWithEmployees();
}
