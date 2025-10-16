namespace HRMS.DTOs
{

    //DTO: Data Transfer Object
    public class EmployeeDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; } 
        public string? Position { get; set; }

    }
}
