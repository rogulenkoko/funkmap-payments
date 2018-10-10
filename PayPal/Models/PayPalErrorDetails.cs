using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalErrorDetails
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("issue")]
        public string Issue { get; set; }
    }
}