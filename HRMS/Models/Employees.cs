
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

        internal static IEnumerable<object> Where(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
