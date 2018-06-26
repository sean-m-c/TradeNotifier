using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeNotifier.Models;
using TradeNotifier.Services;

namespace TradeNotifier.Controllers
{
    public class TradesController : Controller
    {
        private readonly ITradesService _tradesService;
        private readonly ICryptowatchService _cryptowatchService;
        private readonly ICandleService _candleService;

        public TradesController(ITradesService tradesService, ICryptowatchService cryptowatchService, ICandleService candleService)
        {
            _tradesService = tradesService ?? throw new ArgumentNullException(nameof(tradesService));
            _cryptowatchService = cryptowatchService ?? throw new ArgumentNullException(nameof(cryptowatchService));
            _candleService = candleService ?? throw new ArgumentNullException(nameof(candleService));
        }


        // GET: Trades
        public ActionResult Index()
        {
            var orders = _tradesService.GetOrders();

            var ohlcs = _cryptowatchService.GetOHLCs(new PeriodFourHour());
            var candles = ohlcs.CalculateCandles();
            var shortCBL = _candleService.CalculateShortCBL(candles.ToArray());
            return View(orders);
        }

        // GET: Trades/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Trades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Trades/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Trades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Trades/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Trades/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}