using EF_Core_Demo.Models;

namespace EF_Core_Demo.Repository;

public interface IEmployeeRepository
{
    Employee Find(int id);
    List<Employee> GetAll();

    Employee Add(Employee company);
    Employee Update(Employee company);

    void Remove(int id);

}
