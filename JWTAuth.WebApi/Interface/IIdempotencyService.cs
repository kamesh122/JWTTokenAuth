using JWTAuth.WebApi.Models;

namespace JWTAuth.WebApi.Interface
{
    public interface IIdempotencyService
    {
        Task<IdempotencyResponse?> GetResponseForKeyAsync(string idempotencyKey);
        Task StoreResponseForKeyAsync(string idempotencyKey, string requestBody, string responseBody, int statusCode);
    }
}
