using DieteticConsultationAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DieteticConsultationAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Patient>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Patient patient)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (patient.CreatedById == int.Parse(userId!))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}