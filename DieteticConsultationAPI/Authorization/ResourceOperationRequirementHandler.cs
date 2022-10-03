using DieteticConsultationAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DieteticConsultationAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Patient>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Patient patient)
        {
            if (HasAccessPatient(requirement))
                context.Succeed(requirement);

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? String.Empty;
           
            if (IsPatient(patient.CreatedById, userId))
            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        private static bool HasAccessPatient(ResourceOperationRequirement requirement) =>
            requirement.ResourceOperation == ResourceOperation.Read
            || requirement.ResourceOperation == ResourceOperation.Create;

        private static bool IsPatient(int? createdById, string? userId) => int.TryParse(userId, out var result) && createdById is not null && createdById == result;
    }
}

