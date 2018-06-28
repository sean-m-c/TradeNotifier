using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeNotifier.Models;
using TradeNotifier.Services;
using TradeNotifier.Strategies;
using Xunit;

namespace TradeNotifier.Strategies
{
    public class CountBackLineGuppyStrategyResultTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IEnumerable<ICandle>> mockEnumerable;
        private Mock<IPeriod> mockPeriod;

        public CountBackLineGuppyStrategyResultTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockEnumerable = this.mockRepository.Create<IEnumerable<ICandle>>();
            this.mockPeriod = this.mockRepository.Create<IPeriod>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void TestMethod1()
        {
            IPeriod period = Period.FourHour;
            // Arrange
            var logger = Mock.Of<ILogger<CountBackLineGuppyStrategy>>();
            var candleService = Mock.Of<ICandleService>();

            List<ICandle> _samplesCandles = new List<ICandle>
            {
                new Candle(DateTime.Now.Add(new TimeSpan(0, 3, 0)), new Ohlc { Open = 1, High = 5, Low = 1, Close = 4 }, Period.FourHour), // Not yet closed, 
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan), new Ohlc { Open = 1, High = 5, Low = 1, Close = 5 }, Period.FourHour),  // closed over most recent long CBL
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 2), new Ohlc { Open = 3, High = 5, Low = 2, Close = 4 }, Period.FourHour), // most recent long CBL
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 3), new Ohlc { Open = 4, High = 4, Low = 4, Close = 4 }, Period.FourHour),
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 4), new Ohlc { Open = 3, High = 3, Low = 2, Close = 5 }, Period.FourHour), // Swing low
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 5), new Ohlc { Open = 4, High = 7, Low = 3, Close = 7 }, Period.FourHour),
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 6), new Ohlc { Open = 3, High = 8, Low = 2, Close = 8 }, Period.FourHour), // Swing low
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 7), new Ohlc { Open = 5, High = 11, Low = 5, Close = 10 }, Period.FourHour), // Swing high
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 8), new Ohlc { Open = 6, High = 9, Low = 4, Close = 6 }, Period.FourHour), // Swing low
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 9), new Ohlc { Open = 7, High = 8, Low = 5, Close = 4 }, Period.FourHour),
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 10), new Ohlc { Open = 5, High = 6, Low = 4, Close = 4 }, Period.FourHour),
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 11), new Ohlc { Open = 4, High = 8, Low = 4, Close = 7 }, Period.FourHour), // Swing high
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 12), new Ohlc { Open = 3, High = 4, Low = 2, Close = 4 }, Period.FourHour),
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 13), new Ohlc { Open = 2, High = 2, Low = 1, Close = 1 }, Period.FourHour)
            };

            Mock.Get(candleService).Setup(cs => cs.GetCandlesAsync(period)).Returns(Task.Factory.StartNew(() => _samplesCandles));

            var subject = new CountBackLineGuppyStrategy(candleService, logger);

            // Act
            IStrategyResult result = subject.GetStrategyResult(Period.FourHour);


            // Assert
            Assert.True(result.IsLong);
            Assert.False(result.IsLongStop);
            Assert.False(result.IsShort);
            Assert.False(result.IsShortStop);
        }
    }
}

