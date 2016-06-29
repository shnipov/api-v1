using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.SwaggerGen.Generator;

namespace TodoApi
{
    public class TokenAuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            IEnumerable<string> authenticationSchemes = context.ApiDescription.ActionDescriptor.FilterDescriptors
                .Select(filterInfo => filterInfo.Filter)
                .OfType<AuthorizeFilter>()
                .SelectMany(x => x.Policy.AuthenticationSchemes)
                .Distinct();

            if (authenticationSchemes.Any(x => x == "Bearer"))
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }

                operation.Parameters.Add(new NonBodyParameter
                {
                    Description = "Authorization token",
                    In = "header",
                    Name = "Authorization",
                    Required = true,
                    Type = "string"
                });
            }
        }
    }
}