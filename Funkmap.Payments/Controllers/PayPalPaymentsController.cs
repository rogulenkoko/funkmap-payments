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

        public PayPalPaymentsController(IPayPalService payPalService, IPaymentRepository paymentRepository)
        {
            _payPalService = payPalService;
            _paymentRepository = paymentRepository;
        }

        [HttpPost]
        [Route("donation")]
        [AllowAnonymous]
        public async Task<IActionResult> PayProAccount([FromBody]DonationRequest request)
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

        [HttpGet]
        [Route("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPayment([FromQuery]string payerId, [FromQuery]string paymentId)
        {
            var payment = await _paymentRepository.GetPayments().Where(x => x.ExternalId == paymentId).SingleOrDefaultAsync();
            payment.PaymentStatus = PaymentStatus.Executed;
            _paymentRepository.Update(payment);
            await _paymentRepository.SaveAsync();
            //todo show success page
            return Ok();
        }

        [HttpGet]
        [Route("cancel")]
        [AllowAnonymous]
        public async Task<IActionResult> CancelPayment([FromQuery]string token)
        {
            return Ok();
        }
    }
}
