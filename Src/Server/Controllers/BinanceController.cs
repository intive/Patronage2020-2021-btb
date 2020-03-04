using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Microsoft.Extensions.Options;

namespace BTB.Server.Controllers
{
    public class BinanceController : BaseController
    {
        private BinanceClient _client;

        protected BinanceClient Client => _client ??= new BinanceClient();

        public BinanceController(IOptions<BinanceSettings> settings)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(
                    settings.Value.ApiKey,
                    settings.Value.SecretKey
                ),
                LogVerbosity = LogVerbosity.Error,
                LogWriters = new List<TextWriter> { Console.Out }
            });
        }
    }
}
