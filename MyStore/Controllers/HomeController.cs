using Microsoft.AspNetCore.Mvc;
using MyStore.Data;
using MyStore.Models;
using System.Diagnostics;

namespace MyStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbCotext _cotext;

        public HomeController(ApplicationDbCotext cotext)
        {
            _cotext = cotext;
        }

        public IActionResult Index()
        {
            var product= _cotext.Products.OrderByDescending(x=>x.CreatedAt).Take(4).ToList();
            return View(product);
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
