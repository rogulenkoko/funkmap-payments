using Newtonsoft.Json;

namespace PayPal.Models.Subscription
{
    internal class PayPalPlanCreateRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("payment_definitions")]
        public PayPalPaymentDefinition[] PaymentDefinitions { get; set; }

        [JsonProperty("merchant_preferences")]
        public PayPalMerchantPreferences MerchantPreferences { get; set; }
    }

    internal class PayPalPaymentDefinition
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("frequency")]
        public string Frequency { get; set; }

        [JsonProperty("frequency_interval")]
        public string FrequencyInterval { get; set; }

        [JsonProperty("amount")]
        public PayPalAmount Amount { get; set; }

        [JsonProperty("cycles")]
        public string Cycles { get; set; }

        [JsonProperty("charge_models")]
        public PayPalChargeModel[] ChargeModels { get; set; }
    }

    internal class PayPalMerchantPreferences
    {
        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }

        [JsonProperty("cancel_url")]
        public string CancelUrl { get; set; }

        [JsonProperty("initial_fail_amount_action")]
        public string InitialFailAmountAction { get; set; }

        [JsonProperty("max_fail_attempts")]
        public string MaxFailAttempts { get; set; }

        [JsonProperty("auto_bill_amount")]
        public string AutoBillAmount { get; set; }
    }

    internal class PayPalChargeModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("amount")]
        public PayPalAmount Amount { get; set; }
    }
}
