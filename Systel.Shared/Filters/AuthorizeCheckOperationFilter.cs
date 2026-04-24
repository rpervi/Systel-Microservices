using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
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
            // [AllowAnonymous] on the action always wins — skip everything
            var actionAllowsAnon = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AllowAnonymousAttribute>()
                .Any();

            if (actionAllowsAnon) return;

            // [AllowAnonymous] on the controller also skips (unless action overrides with [Authorize])
            var controllerAllowsAnon = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .OfType<AllowAnonymousAttribute>()
                .Any() ?? false;

            var actionHasAuthorize = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Any();

            var controllerHasAuthorize = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Any() ?? false;

            // Controller [AllowAnonymous] wins UNLESS the action itself has [Authorize]
            if (controllerAllowsAnon && !actionHasAuthorize) return;

            // Apply lock icon only when authorize is required
            if (controllerHasAuthorize || actionHasAuthorize)
            {
                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

                var securityRequirement = new OpenApiSecurityRequirement
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
                    ] = Array.Empty<string>()
                };

                operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
            }
        }
    }
}
