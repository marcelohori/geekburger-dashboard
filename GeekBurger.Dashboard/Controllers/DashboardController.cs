using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Dashboard.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        [HttpGet("{storeid}")]
        public IActionResult
        GetSalesByStoreId(Guid storeId)
        {
            return Ok();
        }
    }
}