using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Funkmap.Payments.Tools
{
    public class OAuthDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Security = new List<IDictionary<string, IEnumerable<string>>>()
            {
                new Dictionary<string, IEnumerable<string>>()
                {
                    { "Bearer", new string[]{ } },
                    //{ "oauth2", new string[]{ } },
                }
            };
        }
    }
}
