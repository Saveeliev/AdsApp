using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace AdsApp
{
    public class JwtDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _algorithm;
        private readonly TokenValidationParameters _validationParameters;

        public JwtDataFormat([NotNull] string algorithm, [NotNull] TokenValidationParameters validationParameters)
        {
            _algorithm = algorithm ?? throw new ArgumentNullException(nameof(algorithm));
            _validationParameters =
                validationParameters ?? throw new ArgumentNullException(nameof(validationParameters));
        }

        public AuthenticationTicket Unprotect(string protectedText)
            => Unprotect(protectedText, null);

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal;

            try
            {
                principal = handler.ValidateToken(protectedText, _validationParameters, out var validToken);

                if (!(validToken is JwtSecurityToken validJwt))
                {
                    throw new ArgumentException("Invalid JWT");
                }

                if (!validJwt.Header.Alg.Equals(_algorithm, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Algorithm must be '{_algorithm}'");
                }
            }
            catch (SecurityTokenValidationException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            return new AuthenticationTicket(principal, new AuthenticationProperties(), "Cookie");
        }

        public string Protect(AuthenticationTicket data)
        {
            throw new NotSupportedException();
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            throw new NotSupportedException();
        }
    }
}