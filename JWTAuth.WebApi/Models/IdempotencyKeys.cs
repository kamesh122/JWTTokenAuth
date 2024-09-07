using Microsoft.AspNetCore.Http;

namespace JWTAuth.WebApi.Models
{
    public class IdempotencyKeysTable
    {

        public string IdempotencyKey { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }


    }
}
