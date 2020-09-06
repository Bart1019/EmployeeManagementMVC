using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Models
{
    public class UserClaim
    {
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
}
