using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagement.Application.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email Address")]
        [Required]
        [EmailAddress]
        [Remote("IsEmailInUse", "Account")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
