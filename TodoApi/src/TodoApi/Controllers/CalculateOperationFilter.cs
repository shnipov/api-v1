using System.Collections.Generic;
using System.Linq;
using Swashbuckle.SwaggerGen.Generator;

namespace TodoApi.Controllers
{
    public class CalculateOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var toAddressParameter = operation.Parameters.SingleOrDefault(x => x.Name == "To.Address") as NonBodyParameter;
            if (toAddressParameter != null)
            {
                int index = operation.Parameters.IndexOf(toAddressParameter);
                operation.Parameters.Remove(toAddressParameter);

                IEnumerable<NonBodyParameter> fromAddressParameters = operation.Parameters.Where(x => x.Name.StartsWith("From.Address.")).Cast<NonBodyParameter>();
                List<NonBodyParameter> toAddressParameters = fromAddressParameters.Select(x => new NonBodyParameter
                {
                    Name = x.Name.Replace("From.", "To."),
                    Required = x.Required,
                    Description = x.Description,
                    Type = x.Type,
                    In = x.In
                }).ToList();
                toAddressParameters.ForEach(x => operation.Parameters.Insert(index++, x));
            }
        }
    }
}