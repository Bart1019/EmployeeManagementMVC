using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.ViewModels
{
    public class HomeDetailsViewModel
    {
        public Employee Employee { get; set; }
        public string PageTitle { get; set; }
    }
}