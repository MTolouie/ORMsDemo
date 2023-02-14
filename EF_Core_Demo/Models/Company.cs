using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Core_Demo.Models;

public class Company
{
    [Key]
    public int CompanyId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string Address { get; set; }

    [MaxLength(100)]
    public string City { get; set; }

    [MaxLength(100)]
    public string State { get; set; }

    [MaxLength(100)]
    public string PostalCode { get; set; }

    #region relations
    public List<Employee>? Employees { get; set; }
    #endregion
}
