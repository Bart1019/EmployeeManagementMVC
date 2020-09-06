using System;
using System.Collections.Generic;
using System.Text;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.ViewModels
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Claims{ get; set; }
    }
}
