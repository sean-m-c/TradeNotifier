using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{ 
    public class CryptowatchOHLCListDTO
    {
        //public AllowanceDTO Allowance { get; set; }
        public Dictionary<int, List<string[]>> Result { get; set; }

        public IEnumerable<CryptowatchOHLCDTO> CalculateCandles()
        {
            List<string[]> ohlcData = Result.First().Value;

            foreach (string[] ohlc in ohlcData)
            {
                // The values are in this order: [CloseTime, OpenPrice, HighPrice, LowPrice, ClosePrice, Volume]
                yield return new CryptowatchOHLCDTO
                {
                    ClosePrice = decimal.Parse(ohlc[4]),
                    CloseTime = UnixTimeStampToDateTime(double.Parse(ohlc[0])),
                    HighPrice = decimal.Parse(ohlc[2]),
                    LowPrice = decimal.Parse(ohlc[3]),
                    OpenPrice = decimal.Parse(ohlc[1])
                };
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public class AllowanceDTO
        {
            public int Cost { get; set; }
            public int Remaining { get; set; }
        }
    }

    public class CryptowatchOHLCDTO
    {
        public DateTime CloseTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
    }
}
