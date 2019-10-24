using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Dashboard.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        [HttpGet]
        [Route("api/dashboard/sales")]
        public IActionResult Sales()
        {
            return Ok();
        }

        [HttpGet]
        [Route("api/dashboard/usersWithLessOffer")]
        public IActionResult UsersWithLessOffer()
        {
            return Ok();
        }        
    }
}