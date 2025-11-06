namespace HRMS.DTOs
{

    //DTO: Data Transfer Object
    public class EmployeeDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
       
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public long? PositionId { get; set; }
        public string? PositionName { get; set; }
        public decimal Salary { get; set; }

        public long DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public long ManagerId { get; set; }
        public string? ManagerName { get; set; }

    }
}
