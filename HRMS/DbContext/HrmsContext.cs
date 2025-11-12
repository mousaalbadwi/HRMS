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
        public DbSet<Lookup> Lookup { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//call the base method {Parnt method}

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
           
            modelBuilder.Entity<Lookup>()
               .HasData(
                //MajorCode = 0 : Employee Positions
                new Lookup {Id=1, MajorCode = 0, MinorCode = 0, Name = "Employee Positions" },
                new Lookup {Id=2,MajorCode = 0, MinorCode = 1,Name = "Developer" },
                new Lookup {Id=3,MajorCode = 0, MinorCode = 2,Name = "Manager" },
                new Lookup {Id=4,MajorCode = 0, MinorCode = 3,Name = "HR" },

                //MajorCode = 1 : Departments
                new Lookup {Id=5, MajorCode = 1, MinorCode = 0, Name = "Departments Types" },
                new Lookup {Id=6, MajorCode = 1, MinorCode = 1, Name = "Technical" },
                new Lookup {Id=7, MajorCode = 1, MinorCode = 2, Name = "Adminstrative" },
                new Lookup {Id=8, MajorCode = 1, MinorCode = 3, Name = "Finance" }

                 );

            modelBuilder.Entity<User>().HasIndex(x=> x.Username).IsUnique();// بهذه الطريقة بحلي الUsername من نوع Uniqe 
           
            modelBuilder.Entity<Employees>().HasIndex(x => x.UserId).IsUnique();

            modelBuilder.Entity<User>().HasData (
                new User { Id = 1, Username = "Admin", IsAdmin = true, HashedPassword = "$2a$11$Mdqh5UJBfCw9HB/2s9BGN.BpV6OIa3sque6o8T9Z5B9vVg2L1Srhi" }
                
                );

        }
    }
}
