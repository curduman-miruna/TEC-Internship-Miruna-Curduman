using ApiApp;
using Internship.Model;
using Internship.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Threading.Tasks;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        [HttpGet]
        public IActionResult Get()
        {
            _logger.Info("Get all persons");
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
            if (Id == 0)
            {
                _logger.Warn($"Invalid Id: {Id}");
                return BadRequest();
            }
            var db = new APIDbContext();
            Person person = db.Persons.FirstOrDefault(x => x.Id == Id);
            if (person == null)
            {
                _logger.Warn($"Person with Id {Id} not found");
                return NotFound();
            }

            var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(person);
            _logger.Info($"Person with Id {Id} found: {dataAsJson}");
            return Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Person person)
        {
            var db = new APIDbContext();

            var existingPosition = await db.Positions.FindAsync(person.PositionId);
            var existingDepartment = await db.Departments.FindAsync(person.Position.DepartmentId);
            var existingSalary = await db.Salaries.FindAsync(person.SalaryId);

            if (existingPosition != null)
                person.Position = existingPosition;

            if (existingSalary != null)
                person.Salary = existingSalary;

            if (existingDepartment != null)
                person.Position.Department = existingDepartment;

            if (!ModelState.IsValid)
            {
                var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(person);
                _logger.Warn($"Invalid person data: {dataAsJson}");
                return BadRequest();
            }

            var personAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(person);
            _logger.Info($"Adding new person: {personAsJson}");
            db.Persons.Add(person);
            await db.SaveChangesAsync();
            return Created("", person);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePerson(Person person)
        {
            var db = new APIDbContext();
            if (!ModelState.IsValid)
            {
                var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(person);
                _logger.Warn($"Bad Request. Invalid person data: {dataAsJson}");
                return BadRequest();
            }

            var updatePerson = await db.Persons.FindAsync(person.Id);
            if (updatePerson == null)
            {
                _logger.Warn($"Person with Id {person.Id} not found");
                return NotFound();
            }

            updatePerson.Address = person.Address;
            updatePerson.Age = person.Age;
            updatePerson.Email = person.Email;
            updatePerson.Name = person.Name;
            updatePerson.PositionId = person.PositionId;
            updatePerson.SalaryId = person.SalaryId;
            updatePerson.Surname = person.Surname;
            updatePerson.BirthDay = person.BirthDay;
            updatePerson.City = person.City;

            var personAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(person);
            _logger.Info($"Updating person with Id {person.Id}: {personAsJson}");
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletePerson(int Id)
        {
            var db = new APIDbContext();
            if (Id <= 0)
            {
                _logger.Warn($"Invalid Id: {Id}");
                return BadRequest();
            }

            var person = await db.Persons.FindAsync(Id);
            if (person == null)
            {
                _logger.Warn($"Person with Id {Id} not found");
                return NotFound();
            }

            db.Persons.Remove(person);
            await db.SaveChangesAsync();

            _logger.Info($"Person with Id {Id} deleted");
            return NoContent();
        }
    }
}
