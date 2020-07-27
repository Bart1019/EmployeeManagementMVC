using EmployeeManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees;

        public EmployeeRepository()
        {
            _employees = new List<Employee>
            {
                new Employee() { Id = 0, Name = "Bartek", Surname = "Runowski", EmailAddress = "bart1019@gmail.com", Department = "IT" },
                new Employee() { Id = 1, Name = "Adam", Surname = "Suliborski", EmailAddress = "adam1019@gmail.com", Department = "HR" },
                new Employee() { Id = 2, Name = "Roman", Surname = "Klichy", EmailAddress = "rom1019@gmail.com", Department = "IT" },
            };
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employees;
        }

        public Employee GetById(int employeeId)
        {
            return _employees.FirstOrDefault(x => x.Id == employeeId);
        }
    }
}
