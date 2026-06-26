using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ElearningPlatform.Api.Filters
{
    public class SortPathsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var priorityControllers = new List<string>
            {
                "Authentication",
                "ApplicationUser",
                "Authorization"
            };

            var orderedPaths = new OpenApiPaths();

            var sortedPaths = swaggerDoc.Paths
                .OrderBy(p => GetControllerOrder(p.Key, priorityControllers))
                .ThenBy(p => p.Key);

            foreach (var path in sortedPaths)
            {
                var orderedOperations = path.Value.Operations
                    .OrderBy(op => GetMethodOrder(op.Key))
                    .ToDictionary(op => op.Key, op => op.Value);

                var newPathItem = new OpenApiPathItem();

                foreach (var operation in orderedOperations)
                {
                    newPathItem.Operations.Add(operation.Key, operation.Value);
                }

                orderedPaths.Add(path.Key, newPathItem);
            }

            swaggerDoc.Paths = orderedPaths;
        }

        private int GetControllerOrder(string path, List<string> priorityControllers)
        {
            for (int i = 0; i < priorityControllers.Count; i++)
            {
                if (path.ToLower().Contains(priorityControllers[i].ToLower()))
                    return i;
            }

            return priorityControllers.Count;
        }

        private int GetMethodOrder(OperationType method)
        {
            return method switch
            {
                OperationType.Get => 0,
                OperationType.Post => 1,
                OperationType.Put => 2,
                OperationType.Patch => 3,
                OperationType.Delete => 4,
                _ => 5
            };
        }
    }
}
