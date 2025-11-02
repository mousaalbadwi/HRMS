
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Employees
    {
       
        public long Id { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; } 
        public string? Position { get; set; }
        public decimal Salary { get; set; }
       
        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }// Navigation Property
        
        [ForeignKey("Manager")]
        public long ManagerId { get; set; }
        public Employees Manager { get; set; } // Navigation Property


    }
}
