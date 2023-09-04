using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Http.Api;

public static class ClerkAdapter
{
    public static IServiceCollection AddClerk(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    string pem = configuration.GetSection("Clerk:PEMPublicKey").Value!;
                    string[] splitPem = Regex.Matches(pem, ".{1,64}").Select(m => m.Value).ToArray();
                    string publicKey = "-----BEGIN PUBLIC KEY-----\n" + string.Join("\n", splitPem) + "\n-----END PUBLIC KEY-----";
                    RSA rsa = RSA.Create();
                    rsa.ImportFromPem(publicKey);
                    SecurityKey issuerSigningKey = new RsaSecurityKey(rsa);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "https://wealthy-vulture-44.clerk.accounts.dev",
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = issuerSigningKey
                    };
                }
            );
        
        return services;
    }
}