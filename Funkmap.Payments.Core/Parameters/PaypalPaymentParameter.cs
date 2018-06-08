using Funkmap.Payments.Core.Abstract;

namespace Funkmap.Payments.Core.Parameters
{
    public class PaypalPaymentParameter : IPaymentParameter
    {
        public string PayerId { get; set; }
    }
}
