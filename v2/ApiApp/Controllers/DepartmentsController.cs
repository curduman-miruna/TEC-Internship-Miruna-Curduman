using ApiApp;
using Internship.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Get()
        {
            var db = new APIDbContext();
            var list = db.Departments.ToList();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            if (Id == 0)
            {
                _logger.Warn("Invalid department Id");
                return BadRequest();
            }

            var db = new APIDbContext();
            Department department = db.Departments.Find(Id);

            if (department == null)
            {
                _logger.Warn($"Department with Id {Id} not found");
                return NotFound();
            }

            _logger.Info($"Department with Id {Id} found");
            return Ok(department);
        }

        [HttpPost]
        public IActionResult AddDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                db.Departments.Add(department);
                db.SaveChanges();

                var dataAsJson = JsonConvert.SerializeObject(department);
                _logger.Info($"Department added: {dataAsJson}");
                return Created("", department);
            }
             _logger.Warn("Invalid department data");
            return BadRequest();

        }
        [HttpPut]
        public IActionResult UpdateDepartment(Department department)
        {

            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                Department updateDepartment = db.Departments.Find(department.DepartmentId);
                updateDepartment.DepartmentName = department.DepartmentName;

                _logger.Info($"Department updated: {department.DepartmentName}");
                db.SaveChanges();
                return NoContent();
            }
            _logger.Warn("Invalid department data");
            return BadRequest();
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteDepartment(int Id)
        {
            if (Id <= 0)
            {
                _logger.Warn($"Invalid department Id: {Id}");
                return BadRequest();
            }
            var db = new APIDbContext();
            Department department = db.Departments.Find(Id);
            if (department == null)
            {
                _logger.Warn($"Department with Id {Id} not found");
                return NotFound();
            }
            db.Departments.Remove(department);
            db.SaveChanges();
            _logger.Info($"Department with Id {Id} deleted");
            return NoContent();
        }
    }
}
