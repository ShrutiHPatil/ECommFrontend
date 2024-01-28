using WebApp.Models;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using WebApp.Models;

namespace Login.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly HttpClient _apiClient;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
			_apiClient = new HttpClient();
			_apiClient.BaseAddress = new Uri("https://localhost:7272/api/Login/Login");
		}

		public IActionResult Index()
		{
			var userId = HttpContext.Session.GetInt32("UserId");
			if (HttpContext.Session.Keys.Contains("UserId"))
			{
				
				return View();
			}
			else
			{
				
				return RedirectToAction("Login");
			}
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(loginModel loginModel)
		{
			try
			{
				var response = await _apiClient.PostAsJsonAsync("Login", loginModel);

				if (response.IsSuccessStatusCode)
				{
					var user = await response.Content.ReadAsAsync<UserModel>();

					HttpContext.Session.SetInt32("UserId", user.UserId);
					HttpContext.Session.SetString("Name", user.Username);

					return RedirectToAction("Index");
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					//ModelState.AddModelError(string.Empty, "Invalid username or password");
					ViewBag.InvalidCredentials = true;
				}
				else
				{
					ModelState.AddModelError(string.Empty, "An error occurred while processing your request");
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "An error occurred while processing your request");
			}

			
			return View(loginModel);
		}
		public IActionResult SignOut()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login");
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
