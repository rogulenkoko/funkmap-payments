using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funkmap.Payments.Controllers
{
    [Route("api/product")]
    [Authorize]
    public class ProductController : Controller
    {
        //public async Task<IActionResult> GetAllProducts()
        //{

        //}
    }
}
