using System.Diagnostics;
using FleetCare_Pro.Models;
using Microsoft.AspNetCore.Mvc;

namespace FleetCare_Pro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [Route("Home/StatusCode")]
        public IActionResult StatusCodeHandler(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }
            return View("Error");
        }

        public IActionResult Maintenance()
        {
            return View();
        }
    }
}
