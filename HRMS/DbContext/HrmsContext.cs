using HRMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DbContext
{
    public class HrmsContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public HrmsContext(DbContextOptions<HrmsContext> options) : base(options)
        {
        }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
