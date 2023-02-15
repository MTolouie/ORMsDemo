using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace EF_Core_Demo.Models;

[Dapper.Contrib.Extensions.Table("Companies")]
public class Company
{
    [Dapper.Contrib.Extensions.Key]
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
    [Write(false)]
    public List<Employee>? Employees { get; set; }
    #endregion
}
