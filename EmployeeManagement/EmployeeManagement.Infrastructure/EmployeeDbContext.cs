using System;
using System.Collections.Generic;
using System.Text;
using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
