using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EventPlus.Application.Options;

public sealed class JwtOptions
{
    public AccessTokenOptions AccessToken { get; set; } = new();
    public RefreshTokenOptions RefreshToken { get; set; } = new();


    public sealed class AccessTokenOptions
    {
        public SecurityKey Secret { get; set; } = null!;
        public string? Issuer { get; set; }
        public string[]? Audiences { get; set; }
        public TimeSpan Lifetime { get; set; }
        public TimeSpan ClockSkew { get; set; }
    }

    public sealed class RefreshTokenOptions
    {
        public TimeSpan Lifetime { get; set; }
        public bool RequireSameUserAgent { get; set; }
        public bool RequireSameIPAddress { get; set; }
    }


    internal sealed class Configurator : IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration;
        public Configurator(IConfiguration configuration) => _configuration = configuration;

        public void Configure(JwtOptions options)
        {
            var config = _configuration.GetRequiredSection("Jwt");
            config.Bind(options);

            var stringToken = config.GetValue<string>("AccessToken:Secret")!;
            options.AccessToken.Secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(stringToken));
        }
    }
}