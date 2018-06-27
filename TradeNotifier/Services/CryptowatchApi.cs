using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public class CryptowatchApi : ICryptowatchApi
    {
        const string _baseUrl = "https://api2.service.cryptowat.ch/markets/bitmex/btcusd-perpetual-futures/ohlc";


        public async Task<string> GetPeriodOHLCsAsync(IPeriod period)
        {
            // https://cryptowat.ch/docs/api#ohlc
            if (period == null) throw new ArgumentNullException(nameof(period));

            string url = $"{_baseUrl}?period={period.RoundedSeconds}";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";

            try
            {
                using (WebResponse webResponse = await webRequest.GetResponseAsync())
                using (Stream str = webResponse.GetResponseStream())
                using (StreamReader sr = new StreamReader(str))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException wex)
            {
                using (HttpWebResponse response = (HttpWebResponse)wex.Response)
                {
                    if (response == null)
                        throw;

                    using (Stream str = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(str))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
