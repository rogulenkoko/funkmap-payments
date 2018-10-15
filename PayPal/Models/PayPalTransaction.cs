using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalTransaction
    {
        [JsonProperty("amount")] public PayPalAmount Amount { get; set; }

        [JsonProperty("related_resources")] public object[] RelatedResources { get; set; }
    }
}