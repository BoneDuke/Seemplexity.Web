using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Seemplexity.Resources;

namespace Seemplexity.Web.Filters
{
    public class LocalizedAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var defaultLocalization = Settings.DefaultLanguage;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("DefaultLocalization"))
                defaultLocalization = ConfigurationManager.AppSettings["DefaultLocalization"];

            // Получаем куки из контекста, которые могут содержать установленную культуру
            var cultureCookie = filterContext.HttpContext.Request.Cookies["lang"];
            var cultureName = cultureCookie != null ? cultureCookie.Value : defaultLocalization;

            // Список культур
            if (!Settings.Languages.Select(l => l.Code).Contains(cultureName))
            {
                cultureName = defaultLocalization;
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}