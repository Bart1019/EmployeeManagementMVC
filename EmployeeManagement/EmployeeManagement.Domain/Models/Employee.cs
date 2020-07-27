using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Domain.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string Department { get; set; }
        public Enum GenderType { get; set; }
    }
}
