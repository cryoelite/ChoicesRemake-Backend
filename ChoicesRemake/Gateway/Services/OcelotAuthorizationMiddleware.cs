using Microsoft.AspNetCore.Http;

using Ocelot.Authorization;
using Ocelot.Middleware;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class OcelotAuthorizationMiddleware
    {
        public static async Task Authorize(HttpContext httpContext, Func<Task> next)
        {
            if (ValidateRole(httpContext))
                await next.Invoke();
            else
            {
                httpContext.Response.StatusCode = 403;
                httpContext.Items.SetError(new UnauthorizedError($"Fail to authorize"));
            }
        }

        private static bool ValidateIfStringIsRole(string role)
        {
            return role.Equals(ClaimTypes.Role) || role.Equals("Role") ||
                   role.Equals("role");
        }

        private static bool ValidateRole(HttpContext ctx)
        {
            var downStreamRoute = ctx.Items.DownstreamRoute();
            if (downStreamRoute.AuthenticationOptions.AuthenticationProviderKey == null) return true;
            //This will get the claims of the users JWT
            var userClaims = ctx.User.Claims.ToArray<Claim>();

            //This will get the required authorization claims of the route
            var authScopes = downStreamRoute.AuthenticationOptions.AllowedScopes;

            //Getting the required claims for the route
            foreach (var scope in authScopes)
            {
                var scopeSplit = scope.Split(' ');
                if (ValidateIfStringIsRole(scopeSplit[0]))
                {
                    foreach (Claim userClaim in userClaims)
                    {
                        if (ValidateIfStringIsRole(userClaim.Type) && scopeSplit[1].Equals(userClaim.Value))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}