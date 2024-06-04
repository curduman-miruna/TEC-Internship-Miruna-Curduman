using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using NuGet.Common;

namespace WebApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _api;

        public DepartmentController(IConfiguration config)
        {
            _config = config;
            _api = _config.GetValue<string>("ApiSettings:ApiUrl");
        }

        public async Task<IActionResult> Index()
        {
            List<Department> list = new List<Department>();
            var token = HttpContext.Session.GetString("Token");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseMessage = await client.GetAsync($"{_api}/departments");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jstring = await responseMessage.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<Department>>(jstring);
                }
            }
            return View(list);
        }

        public IActionResult Add()
        {
            Department department = new Department();
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Department department)
        {

            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("Token");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var jsondepartment = JsonConvert.SerializeObject(department);
                    StringContent content = new StringContent(jsondepartment, Encoding.UTF8, "application/json");
                    HttpResponseMessage message = await client.PostAsync($"{_api}/departments", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "There is an API error");
                        return View(department);
                    }
                }
            }
            return View(department);
        }

        public async Task<IActionResult> Update(int Id)
        {
            var token = HttpContext.Session.GetString("Token");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/departments/{Id}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    Department department = JsonConvert.DeserializeObject<Department>(jstring);
                    return View(department);
                }
            }
            return RedirectToAction("Add");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Department department)
        {

            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("Token");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var jsondepartment = JsonConvert.SerializeObject(department);
                    StringContent content = new StringContent(jsondepartment, Encoding.UTF8, "application/json");
                    HttpResponseMessage message = await client.PutAsync($"{_api}/departments", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(department);
                    }
                }
            }
            return View(department);
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var token = HttpContext.Session.GetString("Token");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/departments/{Id}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    Department department = JsonConvert.DeserializeObject<Department>(jstring);
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
                HttpResponseMessage message = await client.DeleteAsync($"{_api}/departments/{id}");
                return RedirectToAction("Index");
            }
        }
    }
}
