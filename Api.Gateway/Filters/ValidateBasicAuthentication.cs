using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Api.Gateway.Filters
{
    public class ValidateBasicAuthentication : ActionFilterAttribute
    {
        private readonly ILogger<ValidateBasicAuthentication> _logger;
        private readonly IConfiguration _configuration;
        public ValidateBasicAuthentication(ILogger<ValidateBasicAuthentication> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string content = "HTTP 401 Unauthorized";
            try
            {
                if (String.IsNullOrEmpty(context.HttpContext.Request.Headers["AuthorizationBasic"].ToString()))
                {
                    context.Result = new UnauthorizedObjectResult(content);
                }
                else
                {
                    var authHeader = AuthenticationHeaderValue.Parse(context.HttpContext.Request.Headers["AuthorizationBasic"]);
                    var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                    var username = credentials[0];
                    var password = credentials[1];
                    if (username != _configuration["BasicAuth:ClientId"] && password != _configuration["BasicAuth:ClientSecret"])
                    {
                        context.Result = new UnauthorizedObjectResult(content);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Utils.GetObjectError(ex)} {_configuration["TagsLog:TagException"]}");
                context.Result = new UnauthorizedObjectResult(content);
            }

            base.OnActionExecuting(context);
        }
    }
}
