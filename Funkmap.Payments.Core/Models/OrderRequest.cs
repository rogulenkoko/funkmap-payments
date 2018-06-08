using Funkmap.Payments.Core.Abstract;

namespace Funkmap.Payments.Core.Models
{
    public class OrderRequest
    {
        public string Login { get; set; }
        public PaymentRequest PaymentRequest { get; set; }

        public IPaymentParameter PaymentParameter { get; set; }
    }
}