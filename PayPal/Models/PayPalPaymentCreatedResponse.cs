using System;
using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalPaymentCreatedResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("payer")]
        public PayPalPayer Payer { get; set; }

        [JsonProperty("transactions")]
        public PayPalTransaction[] Transactions { get; set; }

        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("links")]
        public PayPalLink[] Links { get; set; }


        //Error

        [JsonProperty("name")]
        public string ErrorName { get; set; }

        [JsonProperty("message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("information_link")]
        public string InformationLink { get; set; }

        [JsonProperty("details")]
        public PayPalErrorDetails[] ErrorDetailes { get; set; }
    }
}
