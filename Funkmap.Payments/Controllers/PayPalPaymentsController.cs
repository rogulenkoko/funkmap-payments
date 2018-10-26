using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal.Abstract;
using PayPal.Contracts;

namespace Funkmap.Payments.Controllers
{
    [Route("api/payment/paypal")]
    [Authorize]
    public class PayPalPaymentsController : Controller
    {
        private readonly IPayPalService _payPalService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentsService _paymentsService;

        public PayPalPaymentsController(IPayPalService payPalService, 
                                        IPaymentRepository paymentRepository,
                                        IProductRepository productRepository,
                                        IPaymentsService paymentsService)
        {
            _payPalService = payPalService;
            _paymentRepository = paymentRepository;
            _productRepository = productRepository;
            _paymentsService = paymentsService;
        }

        /// <summary>
        /// Create donation to Bandmap service
        /// </summary>
        /// <param name="request">Donation request model</param>
        [HttpPost]
        [Route("donation")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateDonation([FromBody]DonationRequest request)
        {
            var currentHost = $"{Request.Scheme}://{Request.Host}";
            var payment = new PayPalPayment
            {
                Currency = Enum.GetName(typeof(Currency), request.Currency),
                Total = request.Total,
                ReturnUrl = $"{currentHost}/api/payment/paypal/confirm",
                CancelUrl = $"{currentHost}/api/payment/paypal/cancel"
            };
            
            var response = await _payPalService.CreatePaymentAsync(payment);

            var donation = new Payment
            {
                Currency = payment.Currency,
                Total = payment.Total,
                DateUtc = DateTime.UtcNow,
                ExternalId = response.Id,
                PaymentStatus = PaymentStatus.Created
            };

            await _paymentRepository.CreateAsync(donation);
            await _paymentRepository.SaveAsync();

            return Ok(response);
        }


        /// <summary>
        /// Subscribe to Bandmap features
        /// </summary>
        /// <param name="productId">Product id</param>
        [HttpPost]
        [Route("subscription")]
        public async Task<IActionResult> CreateSubscription(string productId)
        {
            var product = await _productRepository.GetAsync(productId);
            var payPlanId = await _paymentsService.GetOrCreatePayPalPlanIdAsync(productId);
            var agreement = new PayPalAgreement
            {
                Name = product.Name,
                Description = product.Description,
                PayPalPlanId = payPlanId
            };
            var agreementResponse = await _payPalService.CreateAgreementAsync(agreement);
            return Ok(agreementResponse);
        }


        /// <summary>
        /// Paypal return URL (DO NOT CALL IT)
        /// </summary>
        [HttpGet]
        [Route("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPayment([FromQuery]string payerId, [FromQuery]string paymentId)
        {
            var execute = new PayPalExecutePayment { PayerId = payerId, PaymentId = paymentId};
            await _payPalService.ExecutePaymentAsync(execute);

            var payment = await _paymentRepository.GetPayments().Where(x => x.ExternalId == paymentId).SingleOrDefaultAsync();
            payment.PaymentStatus = PaymentStatus.Executed;
            _paymentRepository.Update(payment);
            await _paymentRepository.SaveAsync();
            //todo show success page
            return Ok();
        }

        /// <summary>
        /// Paypal cancel URL (DO NOT CALL IT)
        /// </summary>
        [HttpGet]
        [Route("cancel")]
        [AllowAnonymous]
        public async Task<IActionResult> CancelPayment([FromQuery]string token)
        {
            return Ok();
        }
    }
}
