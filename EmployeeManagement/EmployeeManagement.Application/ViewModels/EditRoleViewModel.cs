using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        [Display(Name = "Role Id")]
        public string Id { get; set; }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "The Role Name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
