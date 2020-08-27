using BookShop.Api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace BookShop.Api.Extensions
{
    public static class SecureExtensions
    {
        public static void AddCookies(this HttpContext httpContext, string token, string apiKey)
        {
            httpContext.Response.Cookies.Append(
                apiKey,
                token,
                new CookieOptions { MaxAge = TimeSpan.FromHours(24)});
        }

        public static void UseSecureJwt(this IApplicationBuilder app) => app.UseMiddleware<SecureJwtMiddleware>();

        public static void UseCookiesPolicy(this IApplicationBuilder app)
        {
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
            });
        }

        public static void AddJwtAuthentication(this IServiceCollection services, string tokenKey, string Issuer)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(tokenKey)),
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidIssuer = Issuer,
                        LifetimeValidator = (notBefore, expires, token, parameters) =>
                        {
                            return DateTime.UtcNow < expires && DateTime.UtcNow >= notBefore;
                        }
                    };
                });
        }
    }
}
