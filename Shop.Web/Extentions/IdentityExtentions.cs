using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;


namespace Shop.Web.Extentions
{
    public static class IdentityExtentions
    {
        public static string GetUserId(this ClaimsPrincipal claims)
        {
            if(claims != null)
            {
                var data = claims.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
                if (data != null) return data.Value;
            }

          return null;
        }

        public static string GetUserId(this IPrincipal principal)
        {
            var user = (ClaimsPrincipal)principal;

            return user.GetUserId();
        }
    }
}
