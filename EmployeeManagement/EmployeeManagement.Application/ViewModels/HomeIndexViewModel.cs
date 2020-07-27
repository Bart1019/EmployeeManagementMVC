﻿using EmployeeManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.ViewModels
{
    public class HomeIndexViewModel
    {
        public Employee Employee { get; set; }
        public string PageTitle { get; set; }
    }
}
