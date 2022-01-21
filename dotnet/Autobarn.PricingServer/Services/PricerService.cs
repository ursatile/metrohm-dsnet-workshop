using System.Threading.Tasks;
using Autobarn.PricingEngine;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingServer.Services {
    public class PricerService : Pricer.PricerBase {
        private readonly ILogger<PricerService> _logger;
        public PricerService(ILogger<PricerService> logger) {
            _logger = logger;
        }

        public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
            var reply = request switch {
                { Color: "Blue" } => new PriceReply { Price = 5000, CurrencyCode = "USD" },
                { Year: var year } when year < 1980 => new PriceReply { Price = 20, CurrencyCode = "GBP" },
                { ManufacturerName: "DMC" } => new PriceReply { Price = 50000, CurrencyCode = "USD" },
                _ => new PriceReply { Price = 12345, CurrencyCode = "EUR" }
            };
            return Task.FromResult(reply);
        }
    }
}
