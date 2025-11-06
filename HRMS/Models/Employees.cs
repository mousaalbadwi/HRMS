
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Employees
    {
       
        public long Id { get; set; }
        [MaxLength(50)]//حتى تححدد الطول الاقصى للحقل, و عشان الef ما ياخذ ماكس باي ديفولت و ياخذ الرقم النت بدك ليله عشان تخفف لود
        public string? FName { get; set; }
        [MaxLength(50)]
        public string? LName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; } 
        public decimal Salary { get; set; }
       
        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }// Navigation Property
        
        [ForeignKey("Manager")]
        public long ManagerId { get; set; }
        public Employees Manager { get; set; } // Navigation Property
        
        [ForeignKey("Lookup")]
        public long PositionId { get; set; }// Lookup Table Foreign Key

        public Lookup Lookup { get; set; } // Navigation Property
    }
}
