using Common.Constants;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Gateway
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ActionResult GetResult(BaseResponse response) 
        {
            if (response.Status == HttpStatusCode.OK) return Ok(response.JSON);
            if (response.Status == HttpStatusCode.BadRequest) return BadRequest(response.JSON);
            if (response.Status == HttpStatusCode.InternalServerError) return StatusCode((int)HttpStatusCode.InternalServerError, response.JSON);
            return NotFound();  
        }

        protected string UserEmail()
        {
            return HttpContext.Items[ClaimConstant.Email].ToString();
        }
        protected int UserID() 
        {
            return Convert.ToInt32(HttpContext.Items[ClaimConstant.ID].ToString());
        }
    }
}
