using System;
using System.Security.Claims;

namespace Infrastructure.Helpers.TokenHelper
{
    public interface ITokenHelper
    {
        public string GenerateToken(Guid userId);
        public ClaimsIdentity GetIdentity(Guid userId);
    }
}
