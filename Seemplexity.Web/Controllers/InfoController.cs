using System.IO;
using System.Threading;
using System.Web.Mvc;
using Seemplexity.Web.Filters;

namespace Seemplexity.Web.Controllers
{
    [Localized]
    public class InfoController : Controller
    {
        public ActionResult Contacts()
        {
            return View("Contacts_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public ActionResult Directions()
        {
            return View("Directions_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public ActionResult News()
        {
            return View("News_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public ActionResult ToPartners()
        {
            return View("ToPartners_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public ActionResult HowToUse()
        {
            return View("HowToUse_" + Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }
    }
}