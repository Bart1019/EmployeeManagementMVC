using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.ViewModels
{
    public class EditUserViewModel
    {
        public List<string> Claims { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public string Id { get; set; }

        public List<string> Roles { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public EditUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
    }
}