using System;
using System.Threading.Tasks;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Abstract;
using Stripe;

namespace Funkmap.Payments.Stripe
{
    public class StripePaymentService : IPaymentService
    {
        private StripeChargeService _stripeChargeService;

        public StripePaymentService(StripeChargeService stripeChargeService)
        {
            _stripeChargeService = stripeChargeService;
        }

        public async Task<bool> ExecutePaymentAsync(Order order, string sourceToken)
        {
            if (order == null)
            {
                throw new ArgumentException("Order can not be null.");
            }

            if (order.Product == null)
            {
                throw new ArgumentException("Product can not be null.");
            }

            if (String.IsNullOrEmpty(sourceToken))
            {
                throw new ArgumentException("Source token can not be null or empty.");
            }

            var chargeOptions = new StripeChargeCreateOptions()
            {
                Amount = ToStripeAmount(order.OrderPrice),
                Currency = order.Currency,
                Description = order.Product.MetaDescription,
                SourceTokenOrExistingSourceId = sourceToken
            };

            var paymentResult = await _stripeChargeService.CreateAsync(chargeOptions);

            return true;
        }

        private int ToStripeAmount(decimal value)
        {
            var result = Convert.ToInt32(Math.Round(value, 2).ToString().Replace(",",""));
            return result;
        }
    }
}
