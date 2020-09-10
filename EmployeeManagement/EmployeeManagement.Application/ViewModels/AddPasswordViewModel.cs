using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.ViewModels
{
    public class AddPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "New Password and confirmed password must match.")]
        public string ConfirmedNewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
    }
}