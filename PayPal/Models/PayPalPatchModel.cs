using Newtonsoft.Json;

namespace PayPal.Models
{
    public class PayPalPatchModel
    {
        [JsonProperty("op")]
        public string Operation { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("value")]
        public PayPalPatchValue PayPalPatchValue { get; set; }
    }

    public class PayPalPatchValue
    {
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
