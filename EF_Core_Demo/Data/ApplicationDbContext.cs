using EF_Core_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_Core_Demo.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Company> Companies { get; set; }
}
