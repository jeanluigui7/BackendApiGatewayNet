using Common.Base;
using Common.GoogleCaptcha;
using Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Models;
using Models.Security.Queries;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Proxies.SecurityProxy
{
    public class SecurityProxy : ISecurityProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IGoogleCaptchaService _googleCaptchaService;
        public SecurityProxy(IOptions<ApiUrls> apiUrls, IConfiguration configuration, HttpClient httpClient, IGoogleCaptchaService googleCaptchaService)
        {
            _apiUrls = apiUrls.Value;
            _configuration = configuration;
            _httpClient = httpClient;
            _googleCaptchaService = googleCaptchaService;
        }
        public async Task<BaseResponse> GetUserPersonByCredential(GetUserPersonByCredentialQuery request)
        {
            if (Convert.ToBoolean(_configuration["GoogleCaptcha:ValidCaptcha"]))
            {
                bool validate = _googleCaptchaService.ValidateCaptcha(request.TokenCaptcha);
                if (!validate)
                {
                    return new BaseResponse(HttpStatusCode.OK, new EntityResponse<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Payload = null,
                        Message = "Ocurrió un error al iniciar sesión"
                    });
                }
            }

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var req = await _httpClient.PostAsync($"{_apiUrls.SecurityUrl}api/v1/Security", content);
            var resp = await req.Content.ReadAsStringAsync();
            var resProxy = JsonSerializer.Deserialize<EntityResponse<GetUserPersonQueryDto>>(resp,
             new JsonSerializerOptions { 
                    PropertyNameCaseInsensitive = true
             });

            var response = new BaseResponse(HttpStatusCode.OK, new EntityResponse<object>
            {
                Code = resProxy.Code,
                Payload = resProxy.Payload,
                Message = resProxy.Message
            });
            return response;
        }
        public async Task<bool> GetTokenActive(LogoutUserPersonQuery req)
        {
            req.Token = req.Token.Split(" ")[1];
            var content = new StringContent(
               JsonSerializer.Serialize(req),
               Encoding.UTF8,
               "application/json"
           );

            var request = await _httpClient.PostAsync($"{_apiUrls.SecurityUrl}api/v1/Security/get-token-active", content);

            var res = await request.Content.ReadAsStringAsync();
            var resProxi = JsonSerializer.Deserialize<BaseResponse<GetUserTokenDto>>(res
              ,
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               });

            return resProxi.Payload != null;
        }
    }
}
