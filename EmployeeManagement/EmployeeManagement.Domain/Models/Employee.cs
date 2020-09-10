using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Domain.Models
{
    public class Employee
    {
        [Required] public Department? Department { get; set; }

        [Display(Name = "Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        [Required]
        public string EmailAddress { get; set; }

        [Display(Name = "Gender")] [Required] public Gender? GenderType { get; set; }

        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
        public string Name { get; set; }

        public string PhotoPath { get; set; }

        [Required] public string Surname { get; set; }
    }
}