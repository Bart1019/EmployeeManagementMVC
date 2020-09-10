using System.Linq;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee AddEmployee(Employee employee);
        Employee DeleteEmployee(int employeeId);
        IQueryable<Employee> GetAllEmployees();
        Employee GetById(int employeeId);
        Employee UpdateEmployee(Employee employee);
    }
}