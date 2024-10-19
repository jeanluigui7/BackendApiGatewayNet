using Common.Models;
using Jose;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PemReader = Org.BouncyCastle.OpenSsl.PemReader;

namespace Common.JwtToken
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<TokenData> GetToken(string idUser, string email, string isShopper, string isVendor)
        {
            try
            {
                var expiracion = DateTime.Now.AddMinutes(60);


                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, email),
                new Claim("idUser", idUser),
                new Claim("email", email),
                new Claim("isShopper", isShopper),
                new Claim("isVendor", isVendor),
                new Claim("expires", expiracion.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: expiracion,
                    signingCredentials: creds
                    );

                return await Task.FromResult(new TokenData
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = expiracion
                });

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TokenData> GetTokenNew(List<Claim> claims, string privateRsaKey, string aud, string expirationMinutes)
        {
            var expiracion = DateTime.Now.AddMinutes(Convert.ToInt32(expirationMinutes));
            RSAParameters rsaParams;
            string privateKey = "-----BEGIN RSA PRIVATE KEY-----\n" + privateRsaKey + "\n-----END RSA PRIVATE KEY-----";
            using (var tr = new StringReader(privateKey))
            {
                var pemReader = new PemReader(tr);
                AsymmetricCipherKeyPair keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair == null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                Dictionary<string, object> payload = claims.ToDictionary(k => k.Type, v => (object)v.Value);

                DateTime startEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                DateTime localDate = DateTime.Now;
                int expirationMinutesInt = Convert.ToInt32(expirationMinutes);
                int unixDateTimeNow = Convert.ToInt32((localDate.ToUniversalTime() - startEpoch).TotalSeconds);
                int expireInUnixTime = expirationMinutesInt * 60; // 5 min unix time
                int expiration = unixDateTimeNow + expireInUnixTime;

                payload.Add("iat", unixDateTimeNow);
                payload.Add("nbf", unixDateTimeNow);

                if (expirationMinutesInt > 0)
                {
                    payload.Add("exp", expiration);
                }

                payload.Add("aud", aud);

                return await Task.FromResult(new TokenData
                {
                    token = JWT.Encode(payload, rsa, JwsAlgorithm.RS256),
                    expiration = expiracion
                });
            }
        }

        public JObject ValidateTokenFilter(string token, string privateRsaKey)
        {
            string jwt;
            try
            {
                string[] prases = token.Split(' ');
                jwt = prases[1];
            }
            catch (Exception e)
            {
                return null;
            }

            RSAParameters rsaParams;
            string publicRsaKey = privateRsaKey;
            string newKey = "-----BEGIN PUBLIC KEY-----" + Environment.NewLine + publicRsaKey + Environment.NewLine + "-----END PUBLIC KEY-----";

            using (var tr = new StringReader(newKey))
            {
                var pemReader = new PemReader(tr);
                var publicKeyParams = pemReader.ReadObject() as RsaKeyParameters;
                if (publicKeyParams == null)
                {
                    throw new Exception("Could not read RSA public key");
                }
                rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                // This will throw if the signature is invalid
                try
                {
                    JObject content = (JsonConvert.DeserializeObject<JToken>(Jose.JWT.Decode(jwt, rsa, Jose.JwsAlgorithm.RS256))).ToObject<JObject>();

                    int expireSAR = Convert.ToInt32(content["exp"].ToString());
                    Int32 unixTimestamp = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds) + 10;

                    if (expireSAR > unixTimestamp)
                    {
                        return content;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}
