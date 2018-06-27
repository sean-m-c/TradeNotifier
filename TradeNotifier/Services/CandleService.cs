using LazyCache;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public class CandleService : ICandleService
    {
        private readonly IAppCache _cache;
        private readonly ICryptowatchApi _cryptowatchApi;
        private readonly ILogger _logger;

        public CandleService(IAppCache cache, ICryptowatchApi cryptowatchApi, ILogger<CandleService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cryptowatchApi = cryptowatchApi ?? throw new ArgumentNullException(nameof(cryptowatchApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<ICandle>> GetCandlesAsync(IPeriod period)
        {
            Task<List<ICandle>> candlesFactory() => PopulateCandlesCache(period);

            string cacheKey = $"candles_{period.HumanizedName}";
            var candles = await _cache.GetOrAddAsync(cacheKey, candlesFactory, new DateTimeOffset(period.NextClose()));

            return candles;
        }


        private async Task<List<ICandle>> PopulateCandlesCache(IPeriod period)
        {
            if (period == null) throw new ArgumentNullException(nameof(period));

            string json = await _cryptowatchApi.GetPeriodOHLCsAsync(period);
            JObject data = JObject.Parse(json);

            // TODO: handle allowance running out

            // Returned as [0] => result => (period in minutes) => array of OHLC data
            IList<JToken> results = data["result"][period.RoundedSeconds.ToString()].Children().ToList();

            // TODO: switch to automapper
            List<ICandle> candles = new List<ICandle>();
            foreach (JToken ohlcItem in results)
            {
                try
                {
                    string[] values = ohlcItem.ToObject<string[]>();
                    // [ CloseTime, OpenPrice, HighPrice, LowPrice, ClosePrice, Volume ]

                    DateTime closeTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(values[0])).LocalDateTime;
                    decimal openPrice = decimal.Parse(values[1]);
                    decimal highPrice = decimal.Parse(values[2]);
                    decimal lowPrice = decimal.Parse(values[3]);
                    decimal closePrice = decimal.Parse(values[4]);

                    candles.Add(new Candle(closeTime, highPrice, lowPrice, openPrice, closePrice, period));
                }
                catch (FormatException ex)
                {
                    _logger.LogError(ex, $"There was a problem converting the ohlcItem with values [{JsonConvert.SerializeObject(ohlcItem,Formatting.Indented)}].");
                }
            }

            return candles;
        }

        public decimal CalculateShortCBL(ICandle[] candles)
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

            return shortCBL.Value;
        }
    }
}
