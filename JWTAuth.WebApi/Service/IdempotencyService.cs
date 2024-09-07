using JWTAuth.WebApi.Interface;
using JWTAuth.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.WebApi.Service
{
    public class IdempotencyService:IIdempotencyService
    {
        private readonly DatabaseContext _dbContext;

        public IdempotencyService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IdempotencyResponse?> GetResponseForKeyAsync(string idempotencyKey)
        {
            return await _dbContext.IdempotencyKeys
                .Where(k => k.IdempotencyKey == idempotencyKey)
                .Select(k => new IdempotencyResponse
                {
                    StatusCode = k.StatusCode,
                    ResponseBody = k.ResponseBody
                })
                .FirstOrDefaultAsync();
        }

        public async Task StoreResponseForKeyAsync(string idempotencyKey, string requestBody, string responseBody, int statusCode)
        {
            var key = new IdempotencyKeysTable
            {
                IdempotencyKey = idempotencyKey,
                RequestBody = requestBody,
                ResponseBody = responseBody,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.IdempotencyKeys.Add(key);
            await _dbContext.SaveChangesAsync();
        }
    }
}
