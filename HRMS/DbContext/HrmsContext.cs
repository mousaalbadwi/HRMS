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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 علاقة الموظف بمديره (Self Reference)
            modelBuilder.Entity<Employees>()
                .HasOne(e => e.Manager)
                .WithMany() // ما في Navigation من المدير للموظفين تحته حالياً
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // 🔸 يمنع مشكلة الـ Cascade Path

            // 🔹 علاقة الموظف بالقسم (Department)
            modelBuilder.Entity<Employees>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade); // اختياري، تقدر تخليه Restrict كمان
        }
    }
}
