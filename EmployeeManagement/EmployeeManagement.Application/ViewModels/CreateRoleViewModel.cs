using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required] [Display(Name = "Role")] public string RoleName { get; set; }
    }
}