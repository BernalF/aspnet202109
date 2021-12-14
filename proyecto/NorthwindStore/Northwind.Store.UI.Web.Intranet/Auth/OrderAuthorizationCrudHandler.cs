using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Northwind.Store.UI.Web.Intranet.Auth
{
    public class OrderAuthorizationCrudHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Model.Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Model.Order resource)
        {
            if (requirement.Name == Operations.Create.Name)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
