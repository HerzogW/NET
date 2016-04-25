/********************************************************
*                                                        *
*   Copyright (c) Microsoft. All rights reserved.        *
*                                                        *
*********************************************************/

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension
{
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.OData.Extensions;
    using System.Web.Http.Routing;

    /// <summary>
    /// Web API configuration and services
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///  Web API configuration and services
        /// </summary>
        /// <param name="config">Http configuration</param>
        public static void Register(HttpConfiguration config)
        {
            if (config == null)
            {
                return;
            }

            config.AddODataQueryFilter();

            config.Routes.MapHttpRoute(
            name: "GetHealthStatus",
            routeTemplate: "healthstatus",
            defaults: new { controller = "HealthStatus", action = "GetHealthStatus" },
            constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute(
            name: "GetApiItem",
            routeTemplate: "{controller}/apis/{apiName}",
            defaults: new { controller = "ApisConfiguration", action = "GetApiItem" },
            constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute(
            name: "GetApiQuickStart",
            routeTemplate: "{controller}/apis/{apiName}/quickstart",
            defaults: new { controller = "ApisConfiguration", action = "GetApiQuickStart" },
            constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute(
            name: "GetAllApiItems",
            routeTemplate: "{controller}/apis",
            defaults: new { controller = "ApisConfiguration", action = "GetAllApiItems" },
            constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute(
            name: "GetApiSpecs",
            routeTemplate: "{controller}/apis/{apiName}/specs",
            defaults: new { controller = "ApisConfiguration", action = "GetApiSpecs" },
            constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute("API RPC Style", "api/{controller}/{action}",
            new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });

            config.Routes.MapHttpRoute("API default REST style post", "api/{controller}/{action}",
                new { action = "Post" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
        }
    }
}