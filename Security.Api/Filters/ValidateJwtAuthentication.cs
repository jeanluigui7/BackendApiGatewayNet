
using Aplication.Security.Queries;
using Common.Constants;
using Common.JwtToken;
using Common.Util;
using Infraestructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace Security.Api.Filters
{
    public class ValidateJwtAuthentication : ActionFilterAttribute
    {
        private readonly ILogger<ValidateBasicAuthentication> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        public ValidateJwtAuthentication(ILogger<ValidateBasicAuthentication> logger, IConfiguration configuration, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            StringValues authorization;
            string content = "HTTP 401 Unauthorized";
            try
            {
                bool hasValue = context.HttpContext.Request.Headers.TryGetValue("Authorization", out authorization);
                if (!hasValue)
                {
                    context.Result = new UnauthorizedObjectResult(content);
                }
                else
                {
                    JObject jwt = _tokenService.ValidateTokenFilter(authorization, _configuration["Jwt:RsaPrivateKey"]);
                    if (jwt == null)
                    {
                        context.Result = new UnauthorizedObjectResult(content);
                    }
                    else
                    {
                        var tokenActive = await _unitOfWork.UserTokenRepository.GetByTokenAsync(authorization);
                        if (tokenActive != null)
                        {
                            context.HttpContext.Items.Add(ClaimConstant.Email, jwt[ClaimConstant.Email]);
                            context.HttpContext.Items.Add(ClaimConstant.ID, jwt[ClaimConstant.ID]);
                            //context.HttpContext.Items.Add(ClaimConstant.IsShopper, jwt[ClaimConstant.IsShopper]);
                        }
                        else
                        {
                            context.Result = new UnauthorizedObjectResult(content);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Utils.GetObjectError(ex)} {_configuration["TagsLog:TagException"]}");
                context.Result = new UnauthorizedObjectResult(content);
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
