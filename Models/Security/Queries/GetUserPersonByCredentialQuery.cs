using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Security.Queries
{
    public class GetUserPersonByCredentialQuery
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("moduleId")]
        public int ModuleID { get; set; }
        [JsonPropertyName("auth")]
        public bool Auth { get; set; }
        [JsonPropertyName("tokenCaptcha")]
        public string TokenCaptcha { get; set; }
        [JsonPropertyName("loginPostRegister")]
        public bool LoginPostRegister { get; set; }


    }
}
