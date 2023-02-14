using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Core_Demo.Models;

public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    public int CompanyId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(100)]
    public string Phone { get; set; }

    [MaxLength(100)]
    public string Title { get; set; }

    #region relations
    public virtual Company? Company { get; set; }

    #endregion
}
