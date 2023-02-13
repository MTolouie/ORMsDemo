using EF_Core_Demo.Models;

namespace EF_Core_Demo.Repository;

public interface ICompanyRepository
{
    Company Find(int id);
    List<Company> GetAll();

    Company Add(Company company);
    Company Update(Company company);

    void Remove(int id);

}
