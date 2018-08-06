using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Orders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // GET api/orders
        [Authorize(Roles = "Matrix42.MyWorkspace.Customer")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] {"Order1", "Order2" };
        }
    }
}