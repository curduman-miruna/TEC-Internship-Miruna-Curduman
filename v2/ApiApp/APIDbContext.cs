using Internship.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiApp
{
    public class APIDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<User> Users { get; set; }
        public string DbPath { get; }

        public APIDbContext()
        {
            var path = "D:\\Documents\\GitHub\\TEC-Internship-Miruna-Curduman\\v2\\Database";
            DbPath = Path.Join(path, "Internship.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }
}
