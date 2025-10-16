namespace HRMS.DTOs
{
    public class SaveEmployeeDTO
    {
        public long Id { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Position { get; set; }
    }
}
