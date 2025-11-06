namespace HRMS.DTOs
{
    public class SaveEmployeeDTO
    {
        public long Id { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public long PositionId { get; set; }
        public decimal Salary { get; set; }

        public long DepartmentId { get; set; }
        public long ManagerId { get; set; }
    }
}
