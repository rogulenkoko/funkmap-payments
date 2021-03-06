﻿using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalAmount
    {
        [JsonProperty("total")] public decimal Total { get; set; }

        [JsonProperty("currency")] public string Currency { get; set; }
    }

    internal class PayPalPlanAmount
    {
        [JsonProperty("value")] public decimal Total { get; set; }

        [JsonProperty("currency")] public string Currency { get; set; }
    }
}