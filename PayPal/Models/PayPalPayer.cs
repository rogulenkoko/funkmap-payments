using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalPayer
    {
        [JsonProperty("payment_method")] public string PaymentMethod { get; set; }
    }
}