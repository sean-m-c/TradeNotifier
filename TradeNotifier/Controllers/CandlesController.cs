using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradeNotifier.Models;
using TradeNotifier.Services;

namespace TradeNotifier.Controllers
{
    public class CandlesController : Controller
    {
        private readonly ICandleService _candlesService;

        public CandlesController(ICandleService candlesService)
        {
            _candlesService = candlesService ?? throw new ArgumentNullException(nameof(candlesService));
        }

        public IActionResult Index()
        {
            return View(_candlesService.GetCandlesAsync(new PeriodFourHour()).Result);
        }
    }
}