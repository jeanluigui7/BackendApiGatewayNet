using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Security.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ActionResult GetResult(BaseResponse response)
        {
            if (response.Status == HttpStatusCode.OK) return Ok(response.JSON);
            if (response.Status == HttpStatusCode.BadRequest) return BadRequest(response.JSON);

            return NotFound();
        }
    }
}
