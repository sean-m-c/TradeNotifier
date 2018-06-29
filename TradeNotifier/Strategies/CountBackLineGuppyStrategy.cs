using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeNotifier.Extensions;
using TradeNotifier.Models;
using TradeNotifier.Services;

namespace TradeNotifier.Strategies
{
    // TODO: inject config type variables (e.g candle count allowed, trend candle count, etc.)

    public class CountBackLineGuppyStrategyResult : IStrategyResult
    {
        public CountBackLineGuppyStrategyResult(IEnumerable<ICandle> candles, decimal? longCBL, decimal? longStop, decimal? shortCBL, decimal? shortStop, IPeriod period)
        {
            Candles = candles ?? throw new ArgumentNullException(nameof(candles));
            Period = period ?? throw new ArgumentNullException(nameof(period));

            if (Candles != null && Candles.Count() > 3)
            {
                IsLong = longCBL != null && longCBL > longStop;
                IsLongStop = longStop != null && Candles.Where(c => c.IsOpen).First().Low < longStop;
                IsShort = shortCBL != null && shortCBL < shortStop; ;
                IsShortStop = Candles.Where(c => c.IsOpen).First().High < shortStop;
            }
        }

        public IEnumerable<ICandle> Candles { get; set; }
        public bool IsLong { get; }
        public bool IsLongStop { get; }
        public bool IsShort { get; }
        public bool IsShortStop { get; }
        public IPeriod Period { get; }
    }


    public class CountBackLineGuppyStrategy : IStrategy<CountBackLineGuppyStrategyResult>
    {
        ICandleService _candleService;
        ILogger _logger;

        public CountBackLineGuppyStrategy(ICandleService candleService, ILogger<CountBackLineGuppyStrategy> logger)
        {
            _candleService = candleService ?? throw new ArgumentNullException(nameof(candleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public IStrategyResult GetStrategyResult(IEnumerable<ICandle> candles)
        {
            if (candles == null) throw new ArgumentNullException(nameof(candles));
            if (candles.Count() < 3) return null;

            return this.DoGetStrategyResult(candles);
        }


        public IStrategyResult GetStrategyResult(IPeriod period)
        {
            if (period == null) throw new ArgumentNullException(nameof(period));

            IEnumerable<ICandle> candles = _candleService.GetCandlesAsync(period).Result;
            if (candles.Count() < 3) return null;

            return this.DoGetStrategyResult(candles);
        }


        IStrategyResult DoGetStrategyResult(IEnumerable<ICandle> candles)
        {
            if (candles.Count() < 3) return null;

            // TODO: how get guppy/MA?

            CandleAnalysis analysis = DoAnalyzeCandles(candles);

            // TODO: period needs to be redefined - e.g there's a base interval for a period, but also - instances - with open/close times of a period.
            return new CountBackLineGuppyStrategyResult(analysis.Candles, analysis.LongCBL, analysis.LongStop, analysis.ShortCBL, analysis.ShortStop, candles.First().Period);
        }

        CandleAnalysis DoAnalyzeCandles(IEnumerable<ICandle> candlesUnsorted)
        {
            if (candlesUnsorted == null) throw new ArgumentNullException(nameof(candlesUnsorted));

            if (candlesUnsorted.Count() < 3)
            {
                _logger.LogWarning($"Not enough candles in series to run strategy. Returning null. Series given: [{candlesUnsorted.Dump()}].");
                return null;
            }

            // Easier to walk through process when candles are reversed from most recent to least recent.
            // TODO: does this hurt performance?
            ICandle[] candles = candlesUnsorted.OrderByDescending(c => c.OpenTimestamp).ToArray();

            const int longCBLCountbacksMax = 3;
            const int shortCBLCountbacksMax = 3;
            const int trendRangeCount = 14;
            decimal? longCBL = null;
            decimal longStop = candles.First().Close;
            decimal? shortCBL = null;
            decimal shortStop = candles.First().High;
            ICandle currentCandle = null;
            ICandle nextCandle = null;
            ICandle previousCandle = null;
            ICandle swingHighCandle = null;
            ICandle swingLowCandle = null;
            int longCBLCountbacks = 1;
            int shortCBLCountbacks = 1;

            int i = 0;
            while (i < candles.Length && swingHighCandle == null && swingLowCandle == null && shortCBL == null && longCBL == null)
            {
                previousCandle = i > 0 ? candles[i - 1] : null;
                currentCandle = candles[i];
                nextCandle = i < candles.Length - 1 ? candles[i + 1] : null;

                if (!currentCandle.IsOpen)
                {
                    // Look for swing high or short CBL
                    if (swingHighCandle == null)
                    {
                        // look for first preceding swing high.
                        if (previousCandle?.High <= currentCandle.High && currentCandle.High > nextCandle?.High)
                        {
                            swingHighCandle = currentCandle;
                        }
                    }
                    else if (shortCBL == null && shortCBLCountbacks < shortCBLCountbacksMax)
                    {
                        // look for first preceding short CBL preceding swing high.
                        if (currentCandle.Low < previousCandle?.Low)
                        {
                            shortCBLCountbacks += 1;
                            _logger.LogDebug($"Found new candle low to include in CBL count: [{currentCandle.Dump()}]. " +
                               $"Total countback(s): [{shortCBLCountbacks}].");
                        }

                        if (shortCBLCountbacks == 3)
                        {
                            shortCBL = currentCandle.Low;
                            _logger.LogDebug($"Found short CBL in candle low: [{nextCandle?.Dump()}].");
                        }
                    }

                    // Look for most recent swing low or swing low CBL
                    if (swingLowCandle == null)
                    {
                        // look for first preceding swing low.
                        if (previousCandle?.Low <= currentCandle.Low && currentCandle.Low < nextCandle?.Low)
                        {
                            swingLowCandle = currentCandle;
                        }
                    }
                    else if (longCBL == null && longCBLCountbacks < longCBLCountbacksMax)
                    {
                        // look for first long CBL preceding swing high.
                        if (currentCandle.High > previousCandle?.High)
                        {
                            longCBLCountbacks += 1;
                            _logger.LogDebug($"Found new candle high to include in CBL count: [{currentCandle.Dump()}]. " +
                                $"Total countback(s): [{longCBLCountbacks}].");
                        }

                        if (longCBLCountbacks == 3)
                        {
                            longCBL = currentCandle.High;
                            _logger.LogDebug($"Found long CBL in candle high: [{currentCandle.Dump()}].");
                        }
                    }
                }

                if (i < trendRangeCount)
                {
                    if (currentCandle.Low < shortStop)
                    {
                        shortStop = currentCandle.Low;
                    }

                    if (currentCandle.High > longStop)
                    {
                        longStop = currentCandle.High;
                    }
                }


                i += 1;
            }

            CandleAnalysis result = new CandleAnalysis(candles, longCBL, longStop, shortCBL, shortStop);
            _logger.LogDebug($"Calculated result: [{result.Dump()}]. Series given: [{candles.Dump()}].");

            return result;
        }


        IEnumerable<ICandle> RefreshCandles(IPeriod period)
        {
            return _candleService.GetCandlesAsync(period).Result;
        }

        public class CandleAnalysis
        {
            public CandleAnalysis(ICandle[] candles, decimal? longCBL, decimal? longStop, decimal? shortCBL, decimal? shortStop)
            {
                // TODO: guard
                Candles = candles;
                LongCBL = longCBL;
                LongStop = longStop;
                ShortCBL = shortCBL;
                ShortStop = shortStop;
            }

            public ICandle[] Candles { get; }
            public decimal? LongCBL { get; }
            public decimal? LongStop { get; }
            public decimal? ShortCBL { get; }
            public decimal? ShortStop { get; }
        }
    }
}
