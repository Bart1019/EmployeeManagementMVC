using System.Collections.Generic;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.ViewModels
{
    public class UserClaimsViewModel
    {
        public List<UserClaim> Claims { get; set; }

        public string UserId { get; set; }

        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
    }
}