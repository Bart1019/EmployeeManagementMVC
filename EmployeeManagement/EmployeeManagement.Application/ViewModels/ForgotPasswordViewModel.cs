using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}