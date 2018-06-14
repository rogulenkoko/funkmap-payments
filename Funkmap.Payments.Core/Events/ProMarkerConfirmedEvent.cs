namespace Funkmap.Payments.Core.Events
{
    public class ProMarkerConfirmedEvent : IPaymentEvent
    {
        public string ProfileLogin { get; set; }
    }
}
