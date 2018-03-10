using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WAES.BEN.TEST.SERVER
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
              name: "LoadApi",
              routeTemplate: "{controller}/{action}/{id}/{side}",
              defaults: new { id = RouteParameter.Optional ,side=RouteParameter.Optional}
          );

            config.Routes.MapHttpRoute(
             name: "DiffApi",
             routeTemplate: "{controller}/{action}/{id}",
             defaults: new { id = RouteParameter.Optional }
         );
        }
    }
}
