using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace WebApp.Controllers
{
    public class PositionController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _api;

        public PositionController(IConfiguration config)
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
                HttpResponseMessage message = await client.GetAsync($"{_api}/positions");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    List<Position> list = JsonConvert.DeserializeObject<List<Position>>(jstring);
                    return View(list);
                }
            }
            return View(new List<Position>());
        }

        public IActionResult Add()
        {
            Position position = new Position();
            return View(position);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Position position)
        {
            if (ModelState.IsValid)
            {

                using (HttpClient client = new HttpClient())
                {
                    var token = HttpContext.Session.GetString("Token");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var jsonPosition = JsonConvert.SerializeObject(position);
                    StringContent content = new StringContent(jsonPosition, Encoding.UTF8, "application/json");
                    HttpResponseMessage message = await client.PostAsync($"{_api}/positions", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "There is an API Error");
                        return View(position);
                    }
                }
            }
            return View(position);
        }

        public async Task<IActionResult> Update(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await client.GetAsync($"{_api}/positions/{Id}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    Position position = JsonConvert.DeserializeObject<Position>(jstring);
                    return View(position);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Position position)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var token = HttpContext.Session.GetString("Token");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var jsonPosition = JsonConvert.SerializeObject(position);
                    StringContent content = new StringContent(jsonPosition, Encoding.UTF8, "application/json");
                    HttpResponseMessage message = await client.PutAsync($"{_api}/positions", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "There is an API Error");
                        return View(position);
                    }
                }
            }
            return View(position);
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
                HttpResponseMessage message = await client.GetAsync($"{_api}/positions/{Id}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    Position position = JsonConvert.DeserializeObject<Position>(jstring);
                    return View(position);
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
                HttpResponseMessage message = await client.DeleteAsync($"{_api}/positions/{id}");
                if (message.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "There is an API Error");
                    return View();
                }
            }
        }
    }
}
