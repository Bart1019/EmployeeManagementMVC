using EmployeeManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using EmployeeManagement.Domain;
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
                new Employee() { Id = 0, Name = "Bartek", Surname = "Runowski", EmailAddress = "bart1019@gmail.com", Department = Department.IT, GenderType = Gender.Male},
                new Employee() { Id = 1, Name = "Adam", Surname = "Suliborski", EmailAddress = "adam1019@gmail.com", Department = Department.HR, GenderType = Gender.Male},
                new Employee() { Id = 2, Name = "Ania", Surname = "Klichy", EmailAddress = "rom1019@gmail.com", Department = Department.IT, GenderType = Gender.Female},
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

        public Employee AddEmployee(Employee employee)
        {
           employee.Id = _employees.Max(x => x.Id) + 1;
           _employees.Add(employee);
           return employee;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var updatedEmployee = _employees.FirstOrDefault(x => x.Id == employee.Id);

            if (updatedEmployee != null)
            {
                updatedEmployee.Id = employee.Id;
                updatedEmployee.Name = employee.Name;
                updatedEmployee.Surname = employee.Surname;
                updatedEmployee.EmailAddress = employee.EmailAddress;
                updatedEmployee.Department = employee.Department;
                updatedEmployee.GenderType = employee.GenderType;
            }

            return updatedEmployee;
        }

        public Employee DeleteEmployee(int employeeId)
        {
            var deleteEmployee = _employees.FirstOrDefault(x => x.Id == employeeId);

            if (deleteEmployee != null)
            {
                _employees.Remove(deleteEmployee);
            }

            return deleteEmployee;
        }
    }
}
