using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JobService.Filter
{
    public class ApplyAuthorizeSecurityRequirement : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the endpoint has [Authorize] and does not have [AllowAnonymous]
            var hasAuthorize = context.ApiDescription.CustomAttributes()
                .Any(attr => attr.GetType() == typeof(AuthorizeAttribute));

            var hasAllowAnonymous = context.ApiDescription.CustomAttributes()
                .Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));

            if (hasAuthorize && !hasAllowAnonymous)
            {
                // Add security requirements for endpoints with [Authorize]
                operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
            }
            else
            {
                // Remove security requirements for endpoints with [AllowAnonymous]
                operation.Security?.Clear();
            }
        }
    }
}