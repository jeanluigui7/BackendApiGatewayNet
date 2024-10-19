using System.Text.Json.Serialization;

namespace Models.Security.Queries
{
    public class LogoutUserPersonQuery
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        public int UpdatedBy { get; set; }
        public int StatusID { get; set; }
    }
}