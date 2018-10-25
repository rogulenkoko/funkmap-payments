namespace PayPal.Contracts
{
    public class PayPalPlan
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PeriodType PeriodType { get; set; }

        public int Frequency { get; set; }

        public string Currency { get; set; }

        public decimal Total { get; set; }
    }

    public enum PeriodType
    {
        Month = 1,
        Day = 2,
        Year = 3,
    }
}