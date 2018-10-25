using Newtonsoft.Json;

namespace PayPal.Models.Subscription
{
    internal class PayPalAgreementCreateResponse : PayPalError
    {
        [JsonProperty("plan")]
        public PayPalPlanCreateRequest Plan { get; set; }

        [JsonProperty("links")]
        public PayPalLink[] Links { get; set; }
    }
}
