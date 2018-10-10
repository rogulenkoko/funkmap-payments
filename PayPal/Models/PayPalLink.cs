using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalLink
    {
        [JsonProperty("href")]
        public string Href { get; set; }
        [JsonProperty("rel")]
        public string Rel { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}