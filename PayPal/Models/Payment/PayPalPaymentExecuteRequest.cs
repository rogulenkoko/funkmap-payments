using Newtonsoft.Json;

namespace PayPal.Models.Payment
{
    public class PayPalPaymentExecuteRequest
    {
        [JsonProperty("payer_id")] public string PayerId { get; set; }
    }
}