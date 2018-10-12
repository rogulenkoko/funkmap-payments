using System;
using System.Threading.Tasks;
using PayPal.Abstract;
using PayPal.Contracts;

namespace PayPal
{
    public partial class PayPalService : IPayPalService, IDisposable
    {
        public async Task<PayPalPaymentResult> CreateSubscription(PayPalPayment payment)
        {
            throw new NotImplementedException();
        }

        private async Task CreatePlanAsync()
        {

        }
    }
}
