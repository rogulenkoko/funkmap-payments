using Newtonsoft.Json;

namespace PayPal.Models
{
    internal class PayPalPaymentCreateRequest
    {
        [JsonProperty("intent")]
        public string Intent;

        [JsonProperty("payer")]
        public PayPalPayer Payer { get; set; }

        [JsonProperty("transactions")]
        public PayPalTransaction[] Transactions { get; set; }

        [JsonProperty("redirect_urls")]
        public PayPalRedirectUrls RedirectUrls { get; set; }
    }

    internal class PayPalRedirectUrls
    {
        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }

        [JsonProperty("cancel_url")]
        public string CancelUrl { get; set; }
    }
}
