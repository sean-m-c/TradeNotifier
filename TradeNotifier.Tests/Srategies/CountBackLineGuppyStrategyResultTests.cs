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
        public void TestLongCBLBasic()
        {
            IPeriod period = Period.FourHour;
            // Arrange
            var logger = Mock.Of<ILogger<CountBackLineGuppyStrategy>>();
            var candleService = Mock.Of<ICandleService>();


            // long CBL at 5, 
            List<ICandle> _samplesCandles = new List<ICandle>
            {
                new Candle(DateTime.Now.Add(new TimeSpan(0, 3, 0)),                   new Ohlc { Open = 4, High = 6, Low = 2, Close = 6 }, Period.FourHour), 
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan),     new Ohlc { Open = 3, High = 7, Low = 2, Close = 6 }, Period.FourHour), // long CBL
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 2), new Ohlc { Open = 2, High = 6, Low = 2, Close = 3 }, Period.FourHour), 
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 3), new Ohlc { Open = 4, High = 5, Low = 2, Close = 3 }, Period.FourHour),
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 4), new Ohlc { Open = 2, High = 4, Low = 1, Close = 2 }, Period.FourHour), // Swing low
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 5), new Ohlc { Open = 3, High = 4, Low = 2, Close = 3 }, Period.FourHour), 
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 6), new Ohlc { Open = 4, High = 5, Low = 2, Close = 3 }, Period.FourHour), // long CBL
                new Candle(DateTime.Now.Subtract(Period.FourHour.PeriodTimeSpan * 7), new Ohlc { Open = 5, High = 6, Low = 2, Close = 3 }, Period.FourHour) 
            };

            Mock.Get(candleService).Setup(cs => cs.GetCandlesAsync(period)).Returns(Task.Factory.StartNew(() => _samplesCandles));

            var subject = new CountBackLineGuppyStrategy(candleService, logger);

            // Act
            IStrategyResult result = subject.GetStrategyResult(Period.FourHour);


            // Assert
            Assert.True(result.IsLong);
            //Assert.False(result.IsLongStop);
            //Assert.False(result.IsShort);
            //Assert.False(result.IsShortStop);
        }
    }
}

