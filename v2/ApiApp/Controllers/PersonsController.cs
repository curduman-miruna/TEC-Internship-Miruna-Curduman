using ApiApp;
using Internship.Model;
using Internship.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            if (person == null)
                return NotFound();
            else
                return Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Person person)
        {
            var db = new APIDbContext();
            if (!ModelState.IsValid)
                return BadRequest();

            var existingPosition = await db.Positions.FindAsync(person.PositionId);
            var existingSalary = await db.Salaries.FindAsync(person.SalaryId);

            if (existingPosition != null)
                person.Position = existingPosition;

            if (existingSalary != null)
                person.Salary = existingSalary;

            db.Persons.Add(person);
            await db.SaveChangesAsync();
            return Created("", person);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePerson(Person person)
        {
            var db = new APIDbContext();
            if (!ModelState.IsValid)
                return BadRequest();

            var updatePerson = await db.Persons.FindAsync(person.Id);
            if (updatePerson == null)
                return NotFound();

            updatePerson.Address = person.Address;
            updatePerson.Age = person.Age;
            updatePerson.Email = person.Email;
            updatePerson.Name = person.Name;
            updatePerson.PositionId = person.PositionId;
            updatePerson.SalaryId = person.SalaryId;
            updatePerson.Surname = person.Surname;
            updatePerson.BirthDay = person.BirthDay;
            updatePerson.City = person.City;

            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletePerson(int Id)
        {
            var db = new APIDbContext();
            if (Id <= 0)
                return BadRequest();

            var person = await db.Persons.FindAsync(Id);
            if (person == null)
                return NotFound();

            db.Persons.Remove(person);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
