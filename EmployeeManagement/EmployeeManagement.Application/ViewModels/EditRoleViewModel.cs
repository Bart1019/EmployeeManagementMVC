using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.ViewModels
{
    public class EditRoleViewModel
    {
        [Display(Name = "Role Id")] public string Id { get; set; }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "The Role Name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }

        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
    }
}