using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public DbSet<Department> Departments { get; set; }
        public DbSet<Function> Functions { get; set; }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Tasks>? Tasks { get; set; }

        public DbSet<EmployeesDetails> Employees { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<EmployeeTask> EmployeeTasks { get; set; }

    }
}