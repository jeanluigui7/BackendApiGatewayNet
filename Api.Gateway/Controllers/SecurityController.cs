using Common.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models.Security.Queries;
using Proxies.SecurityProxy;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Api.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : BaseController
    {
        private IConfiguration _configuration { get; }
        private readonly ISecurityProxy _securityProxy;
        private ILogger<SecurityController> _logger;
        public SecurityController(ISecurityProxy securityProxy, ILogger<SecurityController> logger, IConfiguration configuration)
        {
            _securityProxy = securityProxy;
            _logger = logger;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] GetUserPersonByCredentialQuery query)
        {
            try
            {
                var res = await _securityProxy.GetUserPersonByCredential(query);
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
