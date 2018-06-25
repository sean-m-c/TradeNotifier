using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public class CandleService : ICandleService
    {
        private readonly ILogger _logger;

        public CandleService(ILogger<CandleService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public decimal CalculateShortCBL(CryptowatchOHLCDTO[] candles)
        {
            int? swingHighIndex = null;
            decimal lastHigh = candles.Last().HighPrice;

            // Get the last swing high
            int index = candles.Length - 2;
            CryptowatchOHLCDTO currentCandle = null;
            while (swingHighIndex == null)
            {
                currentCandle = candles[index];

                if (index > -1 && currentCandle.HighPrice > lastHigh && candles[index - 1].HighPrice < currentCandle.HighPrice)
                {
                    swingHighIndex = index;
                }
                else
                {
                    index = index - 1;
                }
            }

            _logger.LogDebug("Found swing high of {swingHigh} with low {low}", candles[swingHighIndex.Value].HighPrice, candles[swingHighIndex.Value].LowPrice);

            int precedingLowCount = 0;
            int currentIndex = swingHighIndex.Value;
            decimal? shortCBL = candles[swingHighIndex.Value].LowPrice;
            while (precedingLowCount < 3)
            {
                if (candles[currentIndex].LowPrice < shortCBL)
                {
                    shortCBL = candles[currentIndex].LowPrice;
                    _logger.LogDebug("Found new low {shortCBL}", shortCBL);
                    precedingLowCount = precedingLowCount + 1;
                }

                currentIndex = currentIndex - 1;
            }

            return shortCBL.Value;
        }

    }
}
