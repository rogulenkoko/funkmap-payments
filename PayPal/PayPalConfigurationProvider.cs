using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace PayPal
{
    public class PayPalConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        public PayPalConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ClientId => _configuration["PayPal:ClientId"];
        public string ClientSecret => _configuration["PayPal:ClientSecret"];

        public string PayPalUrl
        {
            get
            {
                switch (_configuration["PayPal:Mode"])
                {
                    case "sandbox": return "https://api.sandbox.paypal.com";
                    case "live": return "https://api.paypal.com";
                    default: throw new ArgumentException("Invalid PayPal mode. Available sandbox and real mode.");
                }
            }
        }

        public Dictionary<string, string> PayPalConfiguration => new Dictionary<string, string>()
        {
            {"mode", _configuration["PayPal:Mode"] }
        };
    }
}
