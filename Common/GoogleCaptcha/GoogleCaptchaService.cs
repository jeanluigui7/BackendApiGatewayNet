using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.GoogleCaptcha
{
    public class GoogleCaptchaService : IGoogleCaptchaService
    {
        private readonly string _key;
        private readonly string _urlGoogle;
        public GoogleCaptchaService(IConfiguration config)
        {
            _key = config["GoogleCaptcha:SecretKeyCaptcha"];
            _urlGoogle = config["GoogleCaptcha:UrlGoogleCaptcha"];
        }
        public bool ValidateCaptcha(string token)
        {
            string encodedResponse = token;
            if (string.IsNullOrEmpty(encodedResponse)) return false;

            var secret = _key;
            var url = _urlGoogle;
            if (string.IsNullOrEmpty(secret)) return false;

            var client = new System.Net.WebClient();

            var googleReply = client.DownloadString($"{url}?secret={secret}&response={encodedResponse}");

            return JsonConvert.DeserializeObject<GoogleCaptchaResponse>(googleReply).Success;
        }
    }
}
