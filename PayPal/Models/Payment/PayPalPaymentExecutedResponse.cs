using System;
using Newtonsoft.Json;

namespace PayPal.Models.Payment
{
    internal class PayPalPaymentExecutedResponse : PayPalError
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("intent")] public string Intent { get; set; }

        [JsonProperty("state")] public string State { get; set; }

        [JsonProperty("cart")] public string Cart { get; set; }

        [JsonProperty("payer")] public PayPalPayer Payer { get; set; }

        [JsonProperty("transactions")] public PayPalTransaction[] Transactions { get; set; }

        [JsonProperty("create_time ")] public DateTime CreateTime { get; set; }

        [JsonProperty("links")] public PayPalLink[] Links { get; set; }
    }
}