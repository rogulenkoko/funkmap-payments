using System;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPal.Abstract;
using PayPal.Contracts;

namespace Funkmap.Payments.Controllers
{
    [Route("api/payment/paypal")]
    [Authorize]
    public class PayPalPaymentsController : Controller
    {
        private readonly IPayPalService _payPalService;
        private readonly IDonationRepository _donationRepository;

        public PayPalPaymentsController(IPayPalService payPalService, IDonationRepository donationRepository)
        {
            _payPalService = payPalService;
            _donationRepository = donationRepository;
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

            var donation = new Donation
            {
                Currency = payment.Currency,
                Total = payment.Total,
                DateUtc = DateTime.UtcNow
            };

            await _donationRepository.CreateAsync(donation);
            await _donationRepository.SaveAsync();

            return Ok(response);
        }

        [HttpGet]
        [Route("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPayment([FromQuery]string payerId, [FromQuery]string paymentId)
        {
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
