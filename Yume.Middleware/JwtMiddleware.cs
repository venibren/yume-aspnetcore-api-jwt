using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Yume.Authorization;
using Yume.Models.Session;
using Yume.Services;
using Yume.Services.Interfaces;

namespace Yume.Middleware
{
    public static class JwtMiddleware
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfigurationSection settings, string? secret)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<ISessionService, SessionService>();

            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();
            services.AddScoped<SessionService>();

            services.AddSingleton<IAuthorizationHandler, JwtAuthorizationHandler>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? string.Empty))
                };
            });

            services.AddAuthorization();

            // Database - MongoDb
            services.Configure<SessionDbSettingsModel>(settings);
            services.AddSingleton<JwtService>();
        }
    }
}
