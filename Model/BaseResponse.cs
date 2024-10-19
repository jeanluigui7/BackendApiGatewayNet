using System;
using System.Net;
using System.Text.Json;

namespace Model
{
    public class BaseResponse
    {
        public HttpStatusCode Status { get; set; }
        public Object JSON { get; set; }
        public BaseResponse(HttpStatusCode status, Object json, JsonSerializerOptions option = null)
        {
            this.Status = status;
            this.JSON = json;
        }
    }
}
