using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalAmount
    {
        [JsonProperty("total")] public string Total { get; set; }

        [JsonProperty("currency")] public string Currency { get; set; }
    }
}