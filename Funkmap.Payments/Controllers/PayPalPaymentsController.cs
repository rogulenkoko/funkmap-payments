using System;
using System.Threading.Tasks;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Core.Parameters;
using Funkmap.Payments.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
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

        public PayPalPaymentsController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
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
