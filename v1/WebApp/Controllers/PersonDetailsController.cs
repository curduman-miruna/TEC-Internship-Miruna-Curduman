using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PersonDetailsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _api;

        public PersonDetailsController(IConfiguration config)
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
                HttpResponseMessage message = await client.GetAsync($"{_api}/persons");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    List<PersonDetails> list = JsonConvert.DeserializeObject<List<PersonDetails>>(jstring);
                    return View(list);
                }
            }
            return View(new List<PersonInformation>());
        }

        public async Task<IActionResult> Add(int personId)
        {
           
            Person person = new Person();
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/persons/{personId}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    person = JsonConvert.DeserializeObject<Person>(jstring);
                }
            }

            PersonDetails personDetails = new PersonDetails(personId, person);
            return View(personDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PersonDetails personDetails)
        {
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/positions/{personDetails.Person.PositionId}");
                if (message.IsSuccessStatusCode)
                {
                    var positionJson = await message.Content.ReadAsStringAsync();
                    personDetails.Person.Position = JsonConvert.DeserializeObject<Position>(positionJson);
                }

                message = await client.GetAsync($"{_api}/departments/{personDetails.Person.Position.DepartmentId}");
                if (message.IsSuccessStatusCode)
                {
                    var departmentJson = await message.Content.ReadAsStringAsync();
                    personDetails.Person.Position.Department = JsonConvert.DeserializeObject<Department>(departmentJson);
                }

                // Fetch and populate Salary object
                message = await client.GetAsync($"{_api}/salaries/{personDetails.Person.SalaryId}");
                if (message.IsSuccessStatusCode)
                {
                    var salaryJson = await message.Content.ReadAsStringAsync();
                    personDetails.Person.Salary = JsonConvert.DeserializeObject<Salary>(salaryJson);
                }

                personDetails.PersonId = personDetails.Person.Id;

                var json = JsonConvert.SerializeObject(personDetails);
                var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                message = await client.PostAsync($"{_api}/persondetails", data);

                if (message.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","Person");
                }
                else
                {
                    var responseContent = await message.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API Error: {responseContent}");
                }
            }
            return View(personDetails);
        }


        public async Task<IActionResult> Update(int id)
        {
            PersonDetails personDetails = new PersonDetails();
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/persondetails/{id}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    personDetails = JsonConvert.DeserializeObject<PersonDetails>(jstring);
                }
                //get person
                message = await client.GetAsync($"{_api}/persons/{personDetails.PersonId}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    personDetails.Person = JsonConvert.DeserializeObject<Person>(jstring);
                }
            }
            return View(personDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonDetails personDetails)
        {
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/positions/{personDetails.Person.PositionId}");
                if (message.IsSuccessStatusCode)
                {
                    var positionJson = await message.Content.ReadAsStringAsync();
                    personDetails.Person.Position = JsonConvert.DeserializeObject<Position>(positionJson);
                }

                message = await client.GetAsync($"{_api}/departments/{personDetails.Person.Position.DepartmentId}");
                if (message.IsSuccessStatusCode)
                {
                    var departmentJson = await message.Content.ReadAsStringAsync();
                    personDetails.Person.Position.Department = JsonConvert.DeserializeObject<Department>(departmentJson);
                }

                // Fetch and populate Salary object
                message = await client.GetAsync($"{_api}/salaries/{personDetails.Person.SalaryId}");
                if (message.IsSuccessStatusCode)
                {
                    var salaryJson = await message.Content.ReadAsStringAsync();
                    personDetails.Person.Salary = JsonConvert.DeserializeObject<Salary>(salaryJson);
                }

                personDetails.PersonId = personDetails.Person.Id;

                var json = JsonConvert.SerializeObject(personDetails);
                var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                message = await client.PutAsync($"{_api}/persondetails", data);

                if (message.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Person");
                }
                else
                {
                    var responseContent = await message.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API Error: {responseContent}");
                }
            }
            return View(personDetails);
        }
    }
}
