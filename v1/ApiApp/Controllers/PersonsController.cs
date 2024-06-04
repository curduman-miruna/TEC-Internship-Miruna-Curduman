using ApiApp;
using Internship.Model;
using Internship.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var db = new APIDbContext();
            var list = db.Persons.Include(x => x.Salary).Include(x => x.Position)
                   .Select(x => new PersonInformation()
                   {
                       Id = x.Id,
                       Name = x.Name,
                       PositionName = x.Position.Name,
                       DepartmentName = x.Position.Department.DepartmentName,
                       Salary = x.Salary.Amount,
                   }).ToList();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            var db = new APIDbContext();
            Person person = db.Persons.FirstOrDefault(x => x.Id == Id);
            Position position = db.Positions.FirstOrDefault(x => x.PositionId == person.PositionId);
            Salary salary = db.Salaries.FirstOrDefault(x => x.SalaryId == person.SalaryId);
            Department department = db.Departments.FirstOrDefault(x => x.DepartmentId == position.DepartmentId);
            person.Position = position;
            person.Position.Department = department;
            person.Salary = salary;
            if (person == null)
                return NotFound();
            else
                return Ok(person);

        }
        [HttpPost]
        public IActionResult Add(Person person)
        {
            if (ModelState.IsValid)
            {
                var db = new APIDbContext();

                var existingPosition = db.Positions.FirstOrDefault(p => p.PositionId == person.PositionId);
                if (existingPosition != null)
                {
                    person.Position = existingPosition;
                }

                var existingSalary = db.Salaries.FirstOrDefault(s => s.SalaryId == person.SalaryId);
                if (existingSalary != null)
                {
                    person.Salary = existingSalary;
                }

                db.Persons.Add(person);
                db.SaveChanges();
                return Created("", person);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult UpdatePerson(Person person)
        {

            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                Person updateperson = db.Persons.Find(person.Id);
                updateperson.Address = person.Address;
                updateperson.Age = person.Age;
                updateperson.Email = person.Email;
                updateperson.Name = person.Name;
                updateperson.PositionId = person.PositionId;
                updateperson.Position = person.Position;
                updateperson.SalaryId = person.SalaryId;
                updateperson.Salary = person.Salary;
                updateperson.Surname = person.Surname;
                db.SaveChanges();
                return NoContent();
            }
            else
                return BadRequest();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletePerson(int Id)
        {
            if(Id <= 0 )
                return BadRequest();
            var db = new APIDbContext();
            Person person = await db.Persons.FindAsync(Id);
            if (person == null)
                return NotFound();
            db.Persons.Remove(person);
            db.SaveChanges();
            return NoContent();
        }
    }
}
