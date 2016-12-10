using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Seemplexity.Web.Filters;

namespace Seemplexity.Web.Controllers
{
    [Localized]
    public class ResortsController : Controller
    {
        public ActionResult Albena()
        {
            return View("Albena_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public ActionResult GoldenSands()
        {
            return View("GoldenSands_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public ActionResult GoldenDay()
        {
            return View("GoldenDay_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public ActionResult Elenite()
        {
            return View("Elenite_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public ActionResult GoldenCoast()
        {
            return View("GoldenCoast_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public ActionResult Duni()
        {
            return View("Duni_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }
    }
}