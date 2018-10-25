using Newtonsoft.Json;

namespace PayPal.Models.Subscription
{
    internal class PayPalPlanCreateResponse : PayPalError
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
