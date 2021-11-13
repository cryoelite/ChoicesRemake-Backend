using Microsoft.OpenApi.Models;
using StaticAssets;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Authentication.Settings
{
    public class SwaggerConfFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = WebAPI_Headers.backerToken,
                In = ParameterLocation.Header,
                Required = false,
            });
        }
    }
}