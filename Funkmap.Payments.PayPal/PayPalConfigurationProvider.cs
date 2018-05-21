using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Funkmap.Payments.PayPal
{
    public class PayPalConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        public PayPalConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Dictionary<string, string> PayPalConfiguration => new Dictionary<string, string>()
        {
            {"mode", _configuration["PayPal:Mode"] },
            {"clientId", _configuration["PayPal:ClientId"] },
            {"clientSecret", _configuration["PayPal:ClientSecret"] },
        };
    }
}
