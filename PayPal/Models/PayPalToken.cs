using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalToken
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("expires_in")]
        public double ExpiresIn { get; set; }
    }
}
