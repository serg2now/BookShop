using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BookShop.Api.Helpers
{
    public class SecureJwtMiddleware
    {
        private RequestDelegate _next;
        private IConfiguration _configuration;

        public SecureJwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var appKey = _configuration.GetSection("Security:appKey").Value;
            var token = context.Request.Cookies[appKey];
            var refreshToken = context.Request.Cookies[$"{appKey}Refresh"];

            if (!string.IsNullOrEmpty(token))
                context.Request.Headers.Add("Authorization", "Bearer " + token);

            if (!string.IsNullOrEmpty(refreshToken))
                context.Request.Headers.Add("RefreshToken", refreshToken);

            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Xss-Protection", "1");
            context.Response.Headers.Add("X-Frame-Options", "DENY");

            await _next(context);
        }
    }
}
