namespace WebAPI.Models
{
    public class Employee: BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string EmployeeNumber { get; set; }
    }
}