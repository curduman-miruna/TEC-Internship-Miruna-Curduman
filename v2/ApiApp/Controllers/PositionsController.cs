using ApiApp;
using Internship.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var db = new APIDbContext();
            var list = db.Positions.ToList();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            var db = new APIDbContext();
            Position position = db.Positions.Find(Id);
            if (position == null)
                return NotFound();
            else
                return Ok(position);
        }

        [HttpPost]
        public IActionResult AddPosition(Position position)
        {
            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                db.Positions.Add(position);
                db.SaveChanges();
                return Created("", position);
            }
            else
                return BadRequest();
        }

        [HttpPut]
        public IActionResult UpdatePosition(Position position)
        {

            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                Position updatePosition = db.Positions.Find(position.PositionId);
                updatePosition.Name = position.Name;
                updatePosition.DepartmentId = position.DepartmentId;
                updatePosition.Department = position.Department;
                db.SaveChanges();
                return NoContent();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var db = new APIDbContext();
            Position position = db.Positions.Find(Id);
            if (position == null)
                return NotFound();
            else
            {
                db.Positions.Remove(position);
                db.SaveChanges();
                return NoContent();
            }
        }


    }
}
