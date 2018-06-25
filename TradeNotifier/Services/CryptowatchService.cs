using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public class CryptowatchService : ICryptowatchService
    {
        private readonly ICryptowatchApi _cryptowatchApi;

        public CryptowatchService(ICryptowatchApi cryptowatchApi)
        {
            _cryptowatchApi = cryptowatchApi ?? throw new ArgumentNullException(nameof(cryptowatchApi));
        }

        public CryptowatchOHLCListDTO GetOHLCs()
        {
            string ohlcData = _cryptowatchApi.GetOHLCs();

            return JsonConvert.DeserializeObject<CryptowatchOHLCListDTO>(ohlcData);
        }

    }
}
