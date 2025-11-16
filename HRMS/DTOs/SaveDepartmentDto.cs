namespace HRMS.DTOs
{
    public class SaveDepartmentDto
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? FloorNumber { get; set; }
        public long TypeId { get; set; }
       
    }
}
