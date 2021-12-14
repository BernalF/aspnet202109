using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Northwind.Store.UI.Web.Intranet.Auth
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        //private readonly UserManager<IdentityUser> _um;
        //public MinimumAgeHandler(UserManager<IdentityUser> um)
        //{
        //    _um = um;
        //}

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       MinimumAgeRequirement requirement)
        {
#if DEBUG
            foreach (var item in context.User.Claims)
            {
                System.Diagnostics.Debug.WriteLine($"{item.Type}, {item.Value}");
            }
#endif
            //var iu = _um.GetUserAsync(context.User).Result;
            //var customClaims = _um.GetClaimsAsync(iu).Result;
            // || !customClaims.Any(c => c.Type == ClaimTypes.DateOfBirth)

            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;
            }

            var dateOfBirth = Convert.ToDateTime(
                context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);

            int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
            {
                calculatedAge--;
            }

            if (calculatedAge >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}