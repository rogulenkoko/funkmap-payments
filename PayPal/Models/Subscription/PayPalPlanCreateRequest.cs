using Newtonsoft.Json;

namespace PayPal.Models.Subscription
{
    internal class PayPalPlanCreateRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    internal class PayPalPaymentDefinition
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("frequency")]
        public string Frequency { get; set; }

        [JsonProperty("frequency_interval")]
        public string FrequencyInterval { get; set; }

        [JsonProperty("amount")]
        public PayPalAmount Amount { get; set; }

        [JsonProperty("cycles")]
        public string Cycles { get; set; }
    }
}
