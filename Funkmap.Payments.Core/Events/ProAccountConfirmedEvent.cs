namespace Funkmap.Payments.Core.Events
{
    public class ProAccountConfirmedEvent : IPaymentEvent
    {
        public string Login { get; set; }
    }
}
