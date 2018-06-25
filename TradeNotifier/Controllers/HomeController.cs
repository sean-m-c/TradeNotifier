using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using TradeNotifier.Models;
using TradeNotifier.Services;

namespace TradeNotifier.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITradesService _tradesService;

        public HomeController(ITradesService tradesService)
        {
            _tradesService = tradesService ?? throw new ArgumentNullException(nameof(tradesService));
        }

        public IActionResult Index()
        {
            var orders = _tradesService.GetOrders();
            return View(orders);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
