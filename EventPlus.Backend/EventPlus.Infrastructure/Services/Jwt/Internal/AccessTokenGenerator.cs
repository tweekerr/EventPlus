using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EventPlus.Application.Options;
using EventPlus.Application.Services.Jwt.Models;
using EventPlus.Core.Constants;
using EventPlus.Domain.Context;
using EventPlus.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeerCore.DependencyInjection;

namespace EventPlus.Infrastructure.Services.Jwt.Internal;

[Service]
public sealed class AccessTokenGenerator(IOptions<JwtOptions> optionsAccessor, ISqlServerDatabase database)
{
    private readonly JwtOptions.AccessTokenOptions _options = optionsAccessor.Value.AccessToken;

    public async Task<JwtToken> GenerateAsync(AppUser user, CancellationToken ct = default)
    {
        var expires = DateTime.UtcNow.Add(_options.Lifetime);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(await GetUserClaimsAsync(user, ct)),
            SigningCredentials = new SigningCredentials(_options.Secret, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _options.Issuer,
            Audience = _options.Audiences is { Length: > 0 } ? _options.Audiences[0] : null,
            IssuedAt = DateTime.UtcNow,
            Expires = expires,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken jwt = tokenHandler.CreateToken(tokenDescriptor);

        return new JwtToken(tokenHandler.WriteToken(jwt), expires);
    }

    private async Task<IEnumerable<Claim>> GetUserClaimsAsync(AppUser user, CancellationToken ct)
    {
        var claims = new List<Claim>
        {
            new(Claims.Id, user.Id.ToString()),
            new(Claims.Username, user.UserName),
            new(Claims.Avatar, user.Avatar ?? ""),
            new(Claims.FirstName, user.FirstName),
        };

        IEnumerable<string> roles = await GetUserRolesAsync(user.Id, ct);

        claims.AddRange(roles.Select(role => new Claim(Claims.Role, role)));

        claims.AddRange(await GetUserClaimsAsync(user.Id, ct));
        return claims;
    }

    private async Task<IEnumerable<Claim>> GetUserClaimsAsync(long userId, CancellationToken ct)
    {
        // TODO: mb smth not works
        List<Claim> claims = await (from u in database.Set<AppUserClaim>()
                where u.UserId == userId
                select new Claim(u.ClaimType, u.ClaimValue ?? "null"))
            .ToListAsync(ct);

        claims.AddRange(await database.Set<AppUserRole>()
            .Where(e => e.UserId == userId)
            .Join(database.Set<AppRoleClaim>(), ur => ur.RoleId, rc => rc.RoleId, (_, rc) => rc)
            .Select(e => new Claim(e.ClaimType!, e.ClaimValue ?? "null"))
            .ToListAsync(ct));

        return claims;
    }

    private Task<List<string>> GetUserRolesAsync(long userId, CancellationToken ct)
    {
        return (from u in database.Set<AppUserRole>()
            where u.UserId == userId
            join r in database.Set<AppRole>() on u.RoleId equals r.Id
            select r.Name).ToListAsync(ct);
    }
}