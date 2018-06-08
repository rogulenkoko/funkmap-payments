using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Core.Parameters;

namespace Funkmap.Payments.Tests.PaymentService
{
    public class PayPalPaymentServiceTest
    {
        private IPaymentService<PaypalPaymentParameter> _paymentService;

        public PayPalPaymentServiceTest()
        {
            
        }

        public void ExecutePaymentTest()
        {
            var order = new Core.Models.Order()
            {
                Product = new Core.Models.Product()
                {
                    Name = "donation",
                    Description = "donation",
                    Price = 0
                },
                Creator = new Core.Models.User()
                {
                    Name = "rogulenkoko",
                    Email = "rogulenkoko@gmail.com",
                },
                OrderPrice = 0.5m
            };
            _paymentService.ExecutePayment(order, new PaypalPaymentParameter());
        }
    }
}
