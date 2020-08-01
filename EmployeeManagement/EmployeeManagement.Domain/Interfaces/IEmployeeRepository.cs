using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetById(int employeeId);
        Employee AddEmployee(Employee employee);
    }
}
