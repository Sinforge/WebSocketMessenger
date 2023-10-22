using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebSocketMessenger.Core.Configuration;

namespace WebSocketMessenger.API.Extentions
{
    public static class CommonExtentions
    {
        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    var jwtConfig = configuration.GetSection("Audience");
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig["Secret"])),
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig["Iss"],
                        ValidateAudience = true,
                        ValidAudience = jwtConfig["Aud"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = true,

                    };
                });
            services.AddAuthorization();
            services.Configure<Audience>(configuration.GetSection("Audience"));

        }
    }
}
