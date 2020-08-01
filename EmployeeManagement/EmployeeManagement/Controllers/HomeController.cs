using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Models;
using EmployeeManagement.Application.ViewModels;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel()
            {
                Employee = _employeeRepository.GetById(id),
                PageTitle = "Welcome From Details"
            };

            return View(homeIndexViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid) return View();
            var newlyCreatedEmployee = _employeeRepository.AddEmployee(employee);
            return RedirectToAction("Details", new { id = newlyCreatedEmployee.Id });

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
