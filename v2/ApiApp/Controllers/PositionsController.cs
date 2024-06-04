using ApiApp;
using Internship.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Get()
        {
            var db = new APIDbContext();
            var list = db.Positions.ToList();

            _logger.Info("Get all positions");
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            if(Id == 0)
            {
                _logger.Warn($"Invalid Id: {Id}");
                return BadRequest();
            }
            var db = new APIDbContext();
            Position position = db.Positions.Find(Id);
            if (position == null)
            {
                _logger.Warn($"Position with Id {Id} not found");
                return NotFound();
            }
            
            var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(position);
            _logger.Info($"Position with Id {Id} found: {dataAsJson}");
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

                var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(position);
                _logger.Info($"Position added: {dataAsJson}");
                return Created("", position);
            }
            var invalidData = Newtonsoft.Json.JsonConvert.SerializeObject(position);
            _logger.Warn($"Invalid data: {invalidData}");
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

                var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(position);
                _logger.Info($"Position updated: {dataAsJson}");
                return NoContent();
            }
            var invalidData = Newtonsoft.Json.JsonConvert.SerializeObject(position);
            _logger.Warn($"Invalid data: {invalidData}");
            return BadRequest();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            if(Id == 0)
            {
                _logger.Warn($"Invalid Id: {Id}");
                return BadRequest();
            }

            var db = new APIDbContext();
            Position position = db.Positions.Find(Id);
            if (position == null)
            {
                _logger.Warn($"Position with Id {Id} not found");
                return NotFound();
            }
            else
            {
                db.Positions.Remove(position);
                db.SaveChanges();

                _logger.Info($"Position with Id {Id} deleted");
                return NoContent();
            }
        }


    }
}
