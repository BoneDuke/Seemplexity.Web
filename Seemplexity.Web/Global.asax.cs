using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Seemplexity.Common.Impl;

namespace Seemplexity.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConvertersConfig.RegisterConverters();

            if (ConfigurationManager.AppSettings.AllKeys.Contains("InMemoryCacheTimeout"))
                InMemoryCache.CacheTimeout = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["InMemoryCacheTimeout"]));
        }
    }
}
