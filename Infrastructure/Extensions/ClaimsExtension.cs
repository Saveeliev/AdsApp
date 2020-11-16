using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AdsApp.Infrastructure.Extensions
{
    public static class ClaimsExtension
    {
        public static Guid GetUserId(this IEnumerable<Claim> claim)
        {
            var userId = claim.Single(i => i.Type == ClaimsIdentity.DefaultNameClaimType).Value;

            var id = Guid.Parse(userId);

            return id;
        }
    }
}