using AspNetCoreMVCApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVCApp.Data
{
    public class MyCompanyDbContext: DbContext
    {
        
        public MyCompanyDbContext(DbContextOptions<MyCompanyDbContext> options): base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<EmployeeJob> EmployeeJob { get; set; }

    }
}