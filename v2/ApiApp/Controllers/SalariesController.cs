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
    public class SalariesController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Get()
        {
            _logger.Info("Get all salaries");
            var db = new APIDbContext();
            var list = db.Salaries.ToList();
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
            Salary salary = db.Salaries.Find(Id);
            if (salary == null)
            {
                _logger.Warn($"Salary with Id {Id} not found");
                return NotFound();
            }
            var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(salary);
            _logger.Info($"Salary with Id {Id} found: {dataAsJson}");
            return Ok(salary);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            if (Id == 0)
            {
                _logger.Warn($"Invalid Id: {Id}");
                return BadRequest();
            }
            var db = new APIDbContext();
            Salary salary = db.Salaries.Find(Id);
            if (salary == null)
            {
                _logger.Warn($"Salary with Id {Id} not found");
                return NotFound();
            }

            db.Salaries.Remove(salary);
            db.SaveChanges();

            var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(salary);
            _logger.Info($"Salary with Id {Id} deleted: {dataAsJson}");
            return NoContent();
        }

        [HttpPost]
        public IActionResult Add(Salary salary)
        {
            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                db.Salaries.Add(salary);
                db.SaveChanges();

                var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(salary);
                _logger.Info($"Salary added: {dataAsJson}");
                return Created("", salary);
            }
            var invalidData = Newtonsoft.Json.JsonConvert.SerializeObject(salary);
            _logger.Warn($"Invalid data: {invalidData}");
            return BadRequest();
        }

        [HttpPut]
        public IActionResult Update(Salary salary)
        {
            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                var existingSalary = db.Salaries.FirstOrDefault(s => s.SalaryId == salary.SalaryId); ;
                if(existingSalary == null)
                {
                    _logger.Warn($"Salary not found");
                    return BadRequest();
                }
                existingSalary.Amount = salary.Amount;
                db.SaveChanges();
                _logger.Info($"Salary with id: {salary.SalaryId} was updated to {salary.Amount}");
                return NoContent();

            }
            _logger.Warn($"Bad Request");
            return BadRequest();
        }
    }
}
