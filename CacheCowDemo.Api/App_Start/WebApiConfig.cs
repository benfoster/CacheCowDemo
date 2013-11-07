using CacheCow.Common;
using CacheCow.Server;
using CacheCow.Server.EntityTagStore.SqlServer;
using CacheCowDemo.Common;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace CacheCowDemo.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSystemDiagnosticsTracing();
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.MessageHandlers.Add(
                new CachingHandler(new SqlServerEntityTagStore())
                {
                    CacheKeyGenerator = GenerateCacheKey,
                    LinkedRoutePatternProvider = GetLinkedRoutes
                }
            );

            config.Routes.MapHttpRoute(
                name: "ContactNumbers",
                routeTemplate: "contacts/{contactId}/numbers/{id}",
                defaults: new { controller = "ContactNumbers", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static CacheKey GenerateCacheKey(string resourceUri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            var routePattern = RemoveQuerystring(resourceUri); // ignore the query portion
            return new CacheKey(resourceUri, headers.SelectMany(h => h.Value), routePattern);
        }

        private static string[] InvalidateCacheMethods = new[] { "POST", "PUT", "PATCH", "DELETE" };

        private static IEnumerable<string> GetLinkedRoutes(string resourceUri, HttpMethod method)
        {
            if (!InvalidateCacheMethods.Any(m => m == method.Method))
            {
                return new string[0];
            }

            var path = RemoveQuerystring(resourceUri);
            return GetRelatedResources(path);
        }

        private static IEnumerable<string> GetRelatedResources(string resourcePath, int? maxInvalidationDepth = null)
        {
            var isSubResource = maxInvalidationDepth.HasValue && resourcePath.Count(c => c == '/') >= maxInvalidationDepth;
            var limit = isSubResource ? resourcePath.GetNthIndexOf('/', maxInvalidationDepth.Value) : 0;
            int segmentIndex = 0;

            // return each route in the resource heirarchy, as far as the limit
            while ((segmentIndex = resourcePath.LastIndexOf("/")) > limit)
            {
                resourcePath = resourcePath.Substring(0, segmentIndex);
                yield return resourcePath;
            }

            yield break;
        }

        private static string RemoveQuerystring(string uri)
        {
            return (uri.Split('?')[0]).TrimEnd('/');
        }
    }
}
