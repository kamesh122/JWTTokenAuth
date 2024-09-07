namespace JWTAuth.WebApi.Models
{
    public class IdempotencyResponse
    {
        public int StatusCode { get; set; }
        public string ResponseBody { get; set; }
    }
}