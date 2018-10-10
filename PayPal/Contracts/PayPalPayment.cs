namespace PayPal.Contracts
{
    public class PayPalPayment
    {
        public string Currency { get; set; }

        public decimal Total { get; set; }

        public string CancelUrl { get; set; }

        public string ReturnUrl { get; set; }
    }
}
