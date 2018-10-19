using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalError
    {
        [JsonProperty("name")] public string ErrorName { get; set; }

        [JsonProperty("message")] public string ErrorMessage { get; set; }

        [JsonProperty("information_link")] public string InformationLink { get; set; }

        [JsonProperty("details")] public PayPalErrorDetails[] ErrorDetailes { get; set; }
    }
}
