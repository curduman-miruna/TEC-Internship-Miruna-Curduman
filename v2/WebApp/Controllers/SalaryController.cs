using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace WebApp.Controllers
{
    public class SalaryController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _api;

        public SalaryController(IConfiguration config)
        {
            _config = config;
            _api = _config.GetValue<string>("ApiSettings:ApiUrl");
        }

        public async Task<IActionResult> Index()
        {
            List<Salary> list = new List<Salary>();
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/salaries");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<Salary>>(jstring);
                }
            }
            return View(list);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.DeleteAsync($"{_api}/salaries/{Id}");
                if (message.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public IActionResult Add()
        {
            Salary salary = new Salary();
            return View(salary);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Salary salary)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var token = HttpContext.Session.GetString("Token");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var jsonSalary = JsonConvert.SerializeObject(salary);
                    StringContent content = new StringContent(jsonSalary, Encoding.UTF8, "application/json");
                    HttpResponseMessage message = await client.PostAsync($"{_api}/salaries", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
    }
}
