using System.Security.Claims;

namespace Flaadestation.ASP.Utils
{
    public static class AuthenticationUtils
    {
        public static Guid? GetCompanyIdFromClaims(ClaimsPrincipal user)
        {
            if (user.Identity == null || !user.Identity.IsAuthenticated)
                return null;

            var companyClaim = user.Claims.FirstOrDefault(c => c.Type == "CompanyId");

            if (companyClaim == null) return null;

            if (Guid.TryParse(companyClaim.Value, out Guid CompanyId))
                return CompanyId;

            return null;
        }
    }
}
