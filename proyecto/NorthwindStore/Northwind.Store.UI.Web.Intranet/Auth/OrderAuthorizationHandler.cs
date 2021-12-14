using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace Northwind.Store.UI.Web.Intranet.Auth
{
    public class OrderAuthorizationHandler : AuthorizationHandler<OrderRequirement, Model.Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderRequirement requirement, Model.Order resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // Determinar el monto de la orden
            if (resource.OrderDetails.Count > 0)
            {
                var total = resource.OrderDetails.Sum(d => d.UnitPrice * d.Quantity);

                if (total >= 1000)
                {
                    // Manager son los que pueden aplicar acciones sobre órdenes.
                    if (context.User.IsInRole("Manager"))
                    {
                        context.Succeed(requirement);
                    }
                }
                else
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
