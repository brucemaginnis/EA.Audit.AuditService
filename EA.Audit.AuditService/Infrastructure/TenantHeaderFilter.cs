using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace EA.Audit.AuditService.Infrastructure
{
    public class TenantHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = Constants.Tenant.TenantId,
                In = ParameterLocation.Header,
                Description = Constants.Tenant.TenantIdSwaggerDescription,
                Required = true
            });
        }
    }
}
