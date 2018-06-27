using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeNotifier.Models;
using TradeNotifier.Services;

namespace TradeNotifier.Strategies
{
    public class CountBackLinkGuppyStrategyResult : IStrategyResult
    {
        public CountBackLinkGuppyStrategyResult(decimal longCBL, decimal longStop, decimal shortCBL, decimal shortStop)
        {
            LongCBL = longCBL;
            LongStop = longStop;
            ShortCBL = shortCBL;
            ShortStop = shortStop;
        }

        public decimal LongCBL { get; set; }
        public decimal LongStop { get; set; }
        public decimal ShortCBL { get; set; }
        public decimal ShortStop { get; set; }

        public bool IsLong => throw new NotImplementedException();

        public bool IsShort => throw new NotImplementedException();

        public bool IsStop => throw new NotImplementedException();

        public IPeriod Period => throw new NotImplementedException();
    }


    public class CountBackLineGuppyStrategy : IStrategy
    {
        ICandleService _candleService;
        ILogger _logger;

        public CountBackLineGuppyStrategy(ICandleService candleService, ILogger logger)
        {
            _candleService = candleService ?? throw new ArgumentNullException(nameof(candleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // TODO: does this run on every call or just once?

        public IStrategyResult GetStrategyResult(IPeriod period)
        {
            if (period == null) throw new ArgumentNullException(nameof(period));

            IEnumerable<ICandle> candles = _candleService.GetCandlesAsync(period).Result;

            // TODO: how get guppy/MA?

            return DoAnalyzeCandles(candles.ToArray());
        }

        CountBackLinkGuppyStrategyResult DoAnalyzeCandles(ICandle[] candles)
        {
            int? swingHighIndex = null;
            decimal lastHigh = candles.Last().High;

            // Get the last swing high
            int index = candles.Length - 2;
            ICandle currentCandle = null;
            while (swingHighIndex == null)
            {
                currentCandle = candles[index];

                if (index > -1 && currentCandle.High > lastHigh && candles[index - 1].High < currentCandle.High)
                {
                    swingHighIndex = index;
                }
                else
                {
                    index = index - 1;
                }
            }

            _logger.LogDebug("Found swing high of {swingHigh} with low {low}", candles[swingHighIndex.Value].High, candles[swingHighIndex.Value].Low);

            int precedingLowCount = 0;
            int currentIndex = swingHighIndex.Value;
            decimal? shortCBL = candles[swingHighIndex.Value].Low;
            while (precedingLowCount < 3)
            {
                if (candles[currentIndex].Low < shortCBL)
                {
                    shortCBL = candles[currentIndex].Low;
                    _logger.LogDebug("Found new low {shortCBL}", shortCBL);
                    precedingLowCount = precedingLowCount + 1;
                }

                currentIndex = currentIndex - 1;
            }


            return new CountBackLinkGuppyStrategyResult(0, 0, shortCBL.Value, 0);
        }


        decimal DoGetShortCBL(IEnumerable<ICandle> candles)
        {
            throw new NotImplementedException();
        }

        decimal DoGetLongCBL(IEnumerable<ICandle> candles)
        {
            throw new NotImplementedException();
        }

        decimal DoGetShortStop(IEnumerable<ICandle> candles)
        {
            throw new NotImplementedException();
        }

        decimal DoGetLongStop(IEnumerable<ICandle> candles)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ICandle> RefreshCandles(IPeriod period)
        {
            return _candleService.GetCandlesAsync(period).Result;
        }
    }
}
