using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using TradeNotifier.Models;
using TradeNotifier.Services.BitMEX;

namespace TradeNotifier.Services
{
    public class TradesService : ITradesService
    {
        private readonly IBitMEXApi _bitmexApi;

        public TradesService(IBitMEXApi bitmexApi)
        {
            _bitmexApi = bitmexApi ?? throw new ArgumentNullException(nameof(bitmexApi));
        }


        IEnumerable<BitMEXOrderBookItemDTO> ITradesService.GetOrders()
        {
            return JsonConvert.DeserializeObject<List<BitMEXOrderBookItemDTO>>(_bitmexApi.GetOrders());
        }
    }

}