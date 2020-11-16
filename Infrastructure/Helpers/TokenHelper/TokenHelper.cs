using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Helpers.TokenHelper
{
    public class TokenHelper : ITokenHelper
    {
        private readonly AuthOptions _options;

        public TokenHelper(IOptions<AuthOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateToken(Guid userId)
        {
            var identity = GetIdentity(userId);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: _options.ISSUER,
                    audience: _options.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_options.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
        public ClaimsIdentity GetIdentity(Guid userId)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString())
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
