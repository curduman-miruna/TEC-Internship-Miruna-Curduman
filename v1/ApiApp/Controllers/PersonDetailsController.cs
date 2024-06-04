using Microsoft.AspNetCore.Mvc;
using Internship.Model;
using Microsoft.AspNetCore.Authorization;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PersonDetailsController : ControllerBase
    {
       

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var db = new APIDbContext();
            var list = db.PersonDetails.ToList();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            var db = new APIDbContext();
            PersonDetail personDetails = db.PersonDetails.Find(Id);
            if (personDetails == null)
                return NotFound();
            else
                return Ok(personDetails);
        }

        [HttpGet("ByPersonId/{personId}")]
        public async Task<IActionResult> GetByPersonId(int personId)
        {
            var db = new APIDbContext();
            var personDetails = db.PersonDetails.FirstOrDefault(x => x.PersonId == personId);
            if (personDetails == null)
                return NotFound();
            else
                return Ok(personDetails);
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonDetails(PersonDetail personDetails)
        {
            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                var existingPerson = db.Persons.FirstOrDefault(p => p.Id == personDetails.PersonId);
                if (existingPerson != null)
                {
                    personDetails.Person = existingPerson;

                    var existingPosition = db.Positions.FirstOrDefault(p => p.PositionId == personDetails.Person.PositionId);
                    if (existingPosition != null)
                    {
                        personDetails.Person.Position = existingPosition;
                    }

                    var existingDepartment = db.Departments.FirstOrDefault(d => d.DepartmentId == personDetails.Person.Position.DepartmentId);
                    if (existingDepartment != null)
                    {
                        personDetails.Person.Position.Department = existingDepartment;
                    }

                    var existingSalary = db.Salaries.FirstOrDefault(s => s.SalaryId == personDetails.Person.SalaryId);
                    if (existingSalary != null)
                    {
                        personDetails.Person.Salary = existingSalary;
                    }
                }
                db.PersonDetails.Add(personDetails);
                await db.SaveChangesAsync();
                return Created("", personDetails);
            }
            else
                return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonDetails(PersonDetail personDetails)
        {
            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                PersonDetail updatePersonDetails = db.PersonDetails.Find(personDetails.Id);
                updatePersonDetails.BirthDay = personDetails.BirthDay;
                updatePersonDetails.PersonCity = personDetails.PersonCity;
                updatePersonDetails.PersonId = personDetails.PersonId;
                updatePersonDetails.Person = personDetails.Person;
                db.SaveChanges();
                return NoContent();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var db = new APIDbContext();
            PersonDetail personDetails = db.PersonDetails.Find(Id);
            if (personDetails == null)
                return NotFound();
            else
            {
                db.PersonDetails.Remove(personDetails);
                db.SaveChanges();
                return NoContent();
            }
        }

    }
}
