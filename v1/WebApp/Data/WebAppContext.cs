using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class WebAppContext : DbContext
    {
        public WebAppContext (DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        public DbSet<WebApp.Models.Department> Department { get; set; } = default!;
        public DbSet<WebApp.Models.Person> Person { get; set; } = default!;
        public DbSet<WebApp.Models.Position> Position { get; set; } = default!;
        public DbSet<WebApp.Models.Salary> Salary { get; set; } = default!;

    }
}
