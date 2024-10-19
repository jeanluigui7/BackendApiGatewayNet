using Aplication.Security.Queries.GetUserPersonByCredential;
using Aplication.Security.Queries.GetUserTokenActive;
using Common.Util;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Security.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SecurityController : BaseController
    {
        private readonly IMediator _mediator;
        private ILogger<SecurityController> _logger;
        private IConfiguration _configuration { get; }
        public SecurityController(IMediator mediator, ILogger<SecurityController> logger, IConfiguration configuration)
        {
            _mediator = mediator;
            _logger = logger;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] GetUserPersonByCredentialQuery request)
        {

            try
            {
                var res = await _mediator.Send(request);
                return GetResult(res);
            }
            catch (Exception e)
            {
                _logger.LogError($"{Utils.GetObjectError(e)} {_configuration["TagsLog:TagException"]}");
                return StatusCode((int)HttpStatusCode.InternalServerError, e);
            }
        }
        [AllowAnonymous]
        [HttpPost("get-token-active")]
        public async Task<IActionResult> GetTokenActive([FromBody] GetUserTokenActiveQuery request)
        {

            try
            {
                var res = await _mediator.Send(request);
                return GetResult(res);
            }
            catch (Exception e)
            {
                _logger.LogError($"{Utils.GetObjectError(e)} {_configuration["TagsLog:TagException"]}");
                return StatusCode((int)HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
