using Common.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.JwtToken
{
    public interface ITokenService
    {
        public Task<TokenData> GetToken(string idUser, string email, string isShopper, string isVendor);
        public Task<TokenData> GetTokenNew(List<Claim> claims, string privateRsaKey, string aud, string expirationMinutes);
        public JObject ValidateTokenFilter(string token, string privateRsaKey);
    }
}
