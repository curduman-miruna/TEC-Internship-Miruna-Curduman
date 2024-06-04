using Internship.Model;
using Microsoft.EntityFrameworkCore;

namespace Internship
{
    public class APIDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<PersonDetail> PersonDetails { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public string DbPath { get; }

        public APIDbContext()
        {
            var path = "D:\\Documents\\GitHub\\TEC-Internship-Miruna-Curduman\\v1\\Database";
            DbPath = Path.Join(path, "Internship.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }
}
