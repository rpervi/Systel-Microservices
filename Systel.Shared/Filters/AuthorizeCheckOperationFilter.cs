using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systel.Shared.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 1. Check if the Action (Method) has [AllowAnonymous] - this always wins
            var hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

            if (hasAllowAnonymous) return;

            // 2. Check if the Controller has [Authorize]
            var controllerHasAuthorize = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ?? false;

            // 3. Check if the Action itself has [Authorize]
            var actionHasAuthorize = context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            // If either the Controller or the Action is protected, add the security requirement
            if (controllerHasAuthorize || actionHasAuthorize)
            {
                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

                var oidcRequirement = new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }
                    ] = new string[] { }
                };

                operation.Security = new List<OpenApiSecurityRequirement> { oidcRequirement };
            }
        }
    }
}
