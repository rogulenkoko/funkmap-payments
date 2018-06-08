using System.Threading.Tasks;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Core.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funkmap.Payments.Controllers
{
    [Route("api/payment")]
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IOrderService _orderService;

        public PaymentsController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("paypal")]
        public async Task<IActionResult> PayProAccount([FromBody]PaypalRequest request)
        {
            var login = User.Identity.Name;

            var orderRequest = new OrderRequest
            {
                PaymentRequest = request,
                Login = login,
                PaymentParameter = new PaypalPaymentParameter
                {
                    //todo paypal parameters
                }
            };
            
            CommandResponse orderExecutionResult = await _orderService.ExecuteOrderAsync(orderRequest);
            return Ok(orderExecutionResult);
        }
    }

    
}
