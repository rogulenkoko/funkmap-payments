using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funkmap.Payments.Controllers
{
    [Route("api/fake")]
    public class FakeController : Controller
    {
        [HttpPost]
        [Authorize]
        [Route("")]
        public IActionResult Do()
        {
            return Ok("success");
        }
    }
}
