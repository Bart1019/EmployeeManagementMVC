using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Models;
using EmployeeManagement.Application.ViewModels;
using EmployeeManagement.Domain.Models;
using Microsoft.AspNetCore.Hosting;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _employeeRepository = employeeRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetById(id),
                PageTitle = "Welcome From Details"
            };

            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                var newlyCreatedEmployee = new Employee
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    EmailAddress = model.EmailAddress,
                    Department = model.Department,
                    GenderType = model.GenderType,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.AddEmployee(newlyCreatedEmployee);

                return RedirectToAction("Details", new { id = newlyCreatedEmployee.Id });
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(Employee employee)
        {
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Name = employee.Name,
                Surname = employee.Surname,
                EmailAddress = employee.EmailAddress,
                Department = employee.Department,
                GenderType = employee.GenderType,
            };

            return View(employeeEditViewModel);
        }

        /*[HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                var newlyEditedEmployee = new Employee
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    EmailAddress = model.EmailAddress,
                    Department = model.Department,
                    GenderType = model.GenderType,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.UpdateEmployee(newlyEditedEmployee);

                return RedirectToAction("Index", new { id = newlyEditedEmployee.Id });
            }

            return View();
        }*/

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
