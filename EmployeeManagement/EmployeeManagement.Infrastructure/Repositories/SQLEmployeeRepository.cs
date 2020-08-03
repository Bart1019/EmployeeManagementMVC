using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public SQLEmployeeRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeDbContext.Employees;
        }

        public Employee GetById(int employeeId)
        {
            return _employeeDbContext.Employees.Find(employeeId);
        }

        public Employee AddEmployee(Employee employee)
        {
            _employeeDbContext.Employees.Add(employee);
            _employeeDbContext.SaveChanges();
            return employee;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var updatedEmployee = _employeeDbContext.Employees.Attach(employee);
            updatedEmployee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _employeeDbContext.SaveChanges();

            return employee;
        }

        public Employee DeleteEmployee(int employeeId)
        {
            var deletedEmployee = _employeeDbContext.Employees.Find(employeeId);

            if (deletedEmployee != null)
            {
                _employeeDbContext.Employees.Remove(deletedEmployee);
                _employeeDbContext.SaveChanges();
            }
            
            return deletedEmployee;
        }
    }
}
