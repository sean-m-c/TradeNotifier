using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradeNotifier.Models;
using TradeNotifier.Strategies;

namespace TradeNotifier.Controllers
{
    public class StrategiesController : Controller
    {
        private readonly IStrategy<CountBackLineGuppyStrategyResult> _guppyCountbackLineStrategy;

        public StrategiesController(IStrategy<CountBackLineGuppyStrategyResult> guppyCountbackLineStrategy)
        {
            _guppyCountbackLineStrategy = guppyCountbackLineStrategy ?? throw new ArgumentNullException(nameof(guppyCountbackLineStrategy));
        }

        public IActionResult Index()
        {
            return View();
        }

        // TODO: abstract factory
        public IActionResult GuppyCountBackLineStrategy()
        {
            IStrategyResult result = _guppyCountbackLineStrategy.GetStrategyResult(new PeriodFourHour());

            return View(result);
        }
    }
}