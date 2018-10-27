using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Core.Parameters;
using Funkmap.Payments.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal.Abstract;
using PayPal.Contracts;

namespace Funkmap.Payments.Controllers
{
    [Route("api/payment/paypal")]
    public class PayPalPaymentsController : Controller
    {
        private readonly IPayPalService _payPalService;
        private readonly IPaymentsService _paymentsService;
        private readonly IPaymentsUnitOfWork _paymentsUnitOfWork;
        private readonly IProductValidationService _validationService;
        private readonly ISubscriptionEventService _subscriptionEventService;

        public PayPalPaymentsController(IPayPalService payPalService,
                                        IPaymentsUnitOfWork paymentsUnitOfWork,
                                        IPaymentsService paymentsService,
                                        IProductValidationService validationService,
                                        ISubscriptionEventService subscriptionEventService)
        {
            _payPalService = payPalService;
            _paymentsUnitOfWork = paymentsUnitOfWork;
            _paymentsService = paymentsService;
            _validationService = validationService;
            _subscriptionEventService = subscriptionEventService;
        }

        /// <summary>
        /// Create donation to Bandmap service
        /// </summary>
        /// <param name="request">Donation request model</param>
        [HttpPost]
        [Route("donation")]
        public async Task<IActionResult> CreateDonation([FromBody]DonationRequest request)
        {
            var currentHost = $"{Request.Scheme}://{Request.Host}";
            var payment = new PayPalPayment
            {
                Currency = Enum.GetName(typeof(Currency), request.Currency),
                Total = request.Total,
                ReturnUrl = $"{currentHost}/api/payment/paypal/donation/confirm",
                CancelUrl = $"{currentHost}/api/payment/paypal/donation/cancel"
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

            await _paymentsUnitOfWork.PaymentRepository.CreateAsync(donation);
            await _paymentsUnitOfWork.SaveAsync();

            return Ok(response);
        }


        /// <summary>
        /// Subscribe to Bandmap features
        /// </summary>
        /// <param name="request">Request model</param>
        [HttpPost]
        [Route("subscription")]
        //[Authorize]
        public async Task<IActionResult> CreateSubscription(SubscriptionRequest request)
        {
            var validationResult = await _validationService.ValidateProductAsync(request.ProductId, request.ProductId);

            if (!String.IsNullOrEmpty(validationResult?.ErrorMessage))
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            var product = await _paymentsUnitOfWork.ProductRepository.GetAsync(request.ProductId);

            var currentHost = $"{Request.Scheme}://{Request.Host}";
            var parameter = new CreatePlanParameter
            {
                ProductName = request.ProductId,
                ReturnUrl = $"{currentHost}/api/payment/paypal/subscription/confirm",
                CancelUrl = $"{currentHost}/api/payment/paypal/subscription/cancel"
            };
            var payPalPlanId = await _paymentsService.GetOrCreatePayPalPlanIdAsync(parameter);
            var agreement = new PayPalAgreement
            {
                Name = product.Title,
                Description = product.Description,
                PayPalPlanId = payPalPlanId
            };
            var agreementResponse = await _payPalService.CreateAgreementAsync(agreement);

            var subscription = new Subscription
            {
                Status = SubscriptionStatus.Pending,
                InfluencedLogin = request.Login,
                ProductName = product.Name,
                Currency = product.Currency,
                PricePerPeriod = product.Price,
                PayPalAgreementId = agreementResponse.Id,
            };
            await _paymentsUnitOfWork.SubscriptionRepository.CreateAsync(subscription);
            await _paymentsUnitOfWork.SaveAsync();
            return Ok(agreementResponse);
        }


        /// <summary>
        /// Paypal return payment URL (DO NOT CALL IT)
        /// </summary>
        [HttpGet]
        [Route("donation/confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPayment([FromQuery]string payerId, [FromQuery]string paymentId)
        {
            var execute = new PayPalExecutePayment { PayerId = payerId, PaymentId = paymentId };
            await _payPalService.ExecutePaymentAsync(execute);

            var payment = await _paymentsUnitOfWork.PaymentRepository.GetPayments()
                .Where(x => x.ExternalId == paymentId)
                .SingleOrDefaultAsync();
            payment.PaymentStatus = PaymentStatus.Executed;
            _paymentsUnitOfWork.PaymentRepository.Update(payment);
            await _paymentsUnitOfWork.SaveAsync();
            //todo show success page
            return Ok();
        }

        /// <summary>
        /// Paypal cancel payment URL (DO NOT CALL IT)
        /// </summary>
        [HttpGet]
        [Route("donation/cancel")]
        public async Task<IActionResult> CancelPayment([FromQuery]string token)
        {
            return Ok();
        }

        /// <summary>
        /// Paypal return subscription URL (DO NOT CALL IT)
        /// </summary>
        [HttpGet]
        [Route("subscription/confirm")]
        public async Task<IActionResult> ConfirmSubscription([FromQuery] string token)
        {
            using (var transaction = _paymentsUnitOfWork.BeginTransaction())
            {
                try
                {
                    var subscription = await _paymentsUnitOfWork.SubscriptionRepository
                        .GetSubscriptions()
                        .Where(x => x.PayPalAgreementId == token)
                        .SingleOrDefaultAsync();

                    subscription.Status = SubscriptionStatus.Active;
                    _paymentsUnitOfWork.SubscriptionRepository.Update(subscription);
                    await _paymentsUnitOfWork.SaveAsync();

                    await _subscriptionEventService.PublishSubscriptionEventAsync(subscription.ProductName, subscription.InfluencedLogin);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
            //todo show success page
            return Ok();
        }

        /// <summary>
        /// Paypal cancel subscription URL (DO NOT CALL IT)
        /// </summary>
        [HttpGet]
        [Route("subscription/cancel")]
        public async Task<IActionResult> CancelSubscription([FromQuery] string token)
        {
            var subscription = await _paymentsUnitOfWork.SubscriptionRepository
                .GetSubscriptions()
                .Where(x => x.PayPalAgreementId == token)
                .SingleOrDefaultAsync();

            subscription.Status = SubscriptionStatus.Canceled;
            _paymentsUnitOfWork.SubscriptionRepository.Update(subscription);
            await _paymentsUnitOfWork.SaveAsync();
            return Ok();
        }
    }
}
