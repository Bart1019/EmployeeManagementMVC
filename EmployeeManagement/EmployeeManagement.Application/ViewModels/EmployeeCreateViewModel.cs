using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Domain;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Application.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required] public Department? Department { get; set; }

        [Display(Name = "Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        [Required]
        public string EmailAddress { get; set; }

        [Display(Name = "Gender")] [Required] public Gender? GenderType { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
        public string Name { get; set; }

        public IFormFile Photo { get; set; }

        [Required] public string Surname { get; set; }
    }
}