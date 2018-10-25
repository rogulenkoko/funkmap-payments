using System;
using Newtonsoft.Json;

namespace PayPal.Models.Subscription
{
    internal class PayPalAgreementRequest
    {
        public PayPalAgreementRequest(string planId)
        {
            Plan = new PayPalAgreementPlan
            {
                Id = planId
            };

            Payer = new PayPalPayer
            {
                PaymentMethod = "paypal"
            };

            StartDate = DateTime.UtcNow.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ssZ");
        }


        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("plan")]
        public PayPalAgreementPlan Plan { get; set; }

        [JsonProperty("payer")]
        public PayPalPayer Payer { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }
    }

    internal class PayPalAgreementPlan
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
