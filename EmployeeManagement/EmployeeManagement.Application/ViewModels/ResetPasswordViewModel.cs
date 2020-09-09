using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EmployeeManagement.Application.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress] 
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmed Password")]
        [Compare("Password", ErrorMessage = "Password and confirmed password must match")]
        public string ConfirmedPassword { get; set; }

        public string Token { get; set; }
    }
}
