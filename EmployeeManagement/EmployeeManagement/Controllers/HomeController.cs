using System;
using System.Diagnostics;
using System.IO;
using EmployeeManagement.Application.ViewModels;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Authorize(Policy = "AdminRolePolicy")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _employeeRepository = employeeRepository;
            _webHostEnvironment = webHostEnvironment;
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
                var uniqueFileName = ProcessUploadedFile(model);

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

                return RedirectToAction("Details", new {id = newlyCreatedEmployee.Id});
            }

            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _employeeRepository.DeleteEmployee(id);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var employee = _employeeRepository.GetById(id);

            if (employee == null)
            {
                Response.StatusCode = 404;

                return View("EmployeeNotFound", id);
            }

            var homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = employee
            };

            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editedEmployee = _employeeRepository.GetById(id);

            var employeeEditView = new EmployeeEditViewModel
            {
                Id = editedEmployee.Id,
                Name = editedEmployee.Name,
                Surname = editedEmployee.Surname,
                EmailAddress = editedEmployee.EmailAddress,
                Department = editedEmployee.Department,
                GenderType = editedEmployee.GenderType,
                ExistingPhotoPath = editedEmployee.PhotoPath
            };

            return View(employeeEditView);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = _employeeRepository.GetById(model.Id);

                employee.Name = model.Name;
                employee.Surname = model.Surname;
                employee.EmailAddress = model.EmailAddress;
                employee.Department = model.Department;
                employee.GenderType = model.GenderType;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        var photoToBeDeleted = Path.Combine(_webHostEnvironment.WebRootPath, "images",
                            model.ExistingPhotoPath);

                        System.IO.File.Delete(photoToBeDeleted);
                    }

                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                _employeeRepository.UpdateEmployee(employee);

                return RedirectToAction("Index");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Photo.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}