using WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace WebApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _api;

        public PersonController(IConfiguration config)
        {
            _config = config;
            _api = _config.GetValue<string>("ApiSettings:ApiUrl");
        }

        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                List <PersonDetails> personDetails = new List<PersonDetails>();
                HttpResponseMessage message = await client.GetAsync($"{_api}/persondetails");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    personDetails = JsonConvert.DeserializeObject<List<PersonDetails>>(jstring);
                }

                ViewBag.PersonDetails = personDetails;

                message = await client.GetAsync($"{_api}/persons");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    List<PersonInformation> list = JsonConvert.DeserializeObject<List<PersonInformation>>(jstring);
                    
                    return View(list);
                }
            }
            return View(new List<PersonInformation>());
        }

        public async Task<IActionResult> Add()
        {
            Person person = new Person();
            List<Position> positions = new List<Position>();
            List<Salary> salaries = new List<Salary>();
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/positions");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    positions = JsonConvert.DeserializeObject<List<Position>>(jstring);
                }
                message = await client.GetAsync($"{_api}/salaries");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    salaries = JsonConvert.DeserializeObject<List<Salary>>(jstring);
                }
            }
            ViewBag.Positions = positions;
            ViewBag.Salaries = salaries;
            return View(person);
        }


        [HttpPost]
        public async Task<IActionResult> Add(Person person)
        {
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/positions/{person.PositionId}");
                if (message.IsSuccessStatusCode)
                {
                    var positionJson = await message.Content.ReadAsStringAsync();
                    person.Position = JsonConvert.DeserializeObject<Position>(positionJson);
                }

                message = await client.GetAsync($"{_api}/departments/{person.Position.DepartmentId}");
                if (message.IsSuccessStatusCode)
                {
                    var departmentJson = await message.Content.ReadAsStringAsync();
                    person.Position.Department = JsonConvert.DeserializeObject<Department>(departmentJson);
                }

                // Fetch and populate Salary object
                message = await client.GetAsync($"{_api}/salaries/{person.SalaryId}");
                if (message.IsSuccessStatusCode)
                {
                    var salaryJson = await message.Content.ReadAsStringAsync();
                    person.Salary = JsonConvert.DeserializeObject<Salary>(salaryJson);
                }

                if (ModelState.IsValid)
                {
                    var jsonPerson = JsonConvert.SerializeObject(person);
                    StringContent content = new StringContent(jsonPerson, Encoding.UTF8, "application/json");
                    message = await client.PostAsync($"{_api}/persons", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var responseContent = await message.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"API Error: {responseContent}");
                        return View(person);
                    }
                }
            }
            return View(person);
        }


        public async Task<IActionResult> Update(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/persons/{Id}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    Person person = JsonConvert.DeserializeObject<Person>(jstring);
                    return View(person);
                }
            }
            return RedirectToAction("Add");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Person person)
        {
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/positions/{person.PositionId}");
                if (message.IsSuccessStatusCode)
                {
                    var positionJson = await message.Content.ReadAsStringAsync();
                    person.Position = JsonConvert.DeserializeObject<Position>(positionJson);
                }

                message = await client.GetAsync($"{_api}/departments/{person.Position.DepartmentId}");
                if (message.IsSuccessStatusCode)
                {
                    var departmentJson = await message.Content.ReadAsStringAsync();
                    person.Position.Department = JsonConvert.DeserializeObject<Department>(departmentJson);
                }

                // Fetch and populate Salary object
                message = await client.GetAsync($"{_api}/salaries/{person.SalaryId}");
                if (message.IsSuccessStatusCode)
                {
                    var salaryJson = await message.Content.ReadAsStringAsync();
                    person.Salary = JsonConvert.DeserializeObject<Salary>(salaryJson);
                }
            }

            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var token = HttpContext.Session.GetString("Token");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var jsonperson = JsonConvert.SerializeObject(person);
                    StringContent content = new StringContent(jsonperson, Encoding.UTF8, "application/json");
                    HttpResponseMessage message = await client.PutAsync($"{_api}/persons", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(person);
                    }
                }
            }
            return View(person);
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/persons/{Id}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    Person department = JsonConvert.DeserializeObject<Person>(jstring);
                    return View(department);
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.DeleteAsync($"{_api}/persons/{id}");
                return RedirectToAction("Index");
            }
        }
    }
}
