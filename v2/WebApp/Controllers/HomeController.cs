using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Diagnostics;
using WebApp.Models.Auth;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly string _api;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _api = _config.GetValue<string>("ApiSettings:AuthUrl");
        }

        public IActionResult Index()
        {
            ViewBag.IsUserLoggedIn = IsUserLoggedIn();
            return View();
        }

        [HttpGet]
        public IActionResult Auth()
        {
            var model = new AuthViewModel
            {
                Login = new LoginViewModel(),
                Register = new RegisterViewModel()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                LoginViewModel loginModel= model.Login;
                using HttpClient client = new HttpClient();
                HttpResponseMessage message = await client.PostAsJsonAsync($"{_api}/login", loginModel);
                if (message.IsSuccessStatusCode)
                {
                    var token = await message.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("Token", token);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Invalid login attempt: {errorContent}");
                }
            }
            return View("Auth", model);
        }


        [HttpPost]
        public async Task<IActionResult> Register(AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
               using HttpClient client = new HttpClient();
                {
                    RegisterViewModel registerModel = model.Register;
                    HttpResponseMessage message = await client.PostAsJsonAsync($"{_api}/register", registerModel);
                    if (message.IsSuccessStatusCode)
                    {
                        var jstring = await message.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<RegisterViewModel>(jstring);
                        return RedirectToAction("Auth", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid registration attempt.");
                    }
                    return RedirectToAction("Auth", "Home");
                }
            }

            return View("Auth", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Auth", "Home");
        }

        public bool IsUserLoggedIn()
        {
            var token = HttpContext.Session.Get("Token");
            return token != null;
        }
    }
}
