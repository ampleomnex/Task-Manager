﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Function> Functions { get; set; }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EmployeesDetails> Employees { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Epics> Epics { get; set; }

        public DbSet<Tasks>  ETasks { get; set; }

        public DbSet<OptionType> OptionTypes { get; set; }

    }
}