using Newtonsoft.Json;

namespace PayPal.Models
{
    public class PayPalPaymentExecuteRequest
    {
        [JsonProperty("payer_id")]
        public string PayerId { get; set; }
    }
}
