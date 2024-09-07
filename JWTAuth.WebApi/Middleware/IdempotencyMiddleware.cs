using JWTAuth.WebApi.Interface;
using JWTAuth.WebApi.Service;

namespace JWTAuth.WebApi.Middleware
{
    public class IdempotencyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IIdempotencyService _idempotencyService;

        public IdempotencyMiddleware(RequestDelegate next)
        {
            _next = next; 
        }

        public async Task InvokeAsync(HttpContext context, IIdempotencyService idempotencyService)
        {
            
            if (context.Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey))
            {
                // Check if the key already exists in the store
                var result = await idempotencyService.GetResponseForKeyAsync(idempotencyKey);
                if (result != null)
                {
                    context.Response.StatusCode = result.StatusCode;
                    await context.Response.WriteAsync(result.ResponseBody);
                    return;
                }
            }

            // Proceed to the next middleware or controller
            await _next(context);
        }
    }
}
