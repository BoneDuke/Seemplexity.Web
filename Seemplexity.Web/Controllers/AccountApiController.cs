using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Seemplexity.Web.Controllers
{
    public class AccountApiController : ApiController
    {
        [HttpGet]
        public void SetUserLang(string lang)
        {
            var cookie = "ru";
            switch (lang.ToUpper())
            {
                case "RUS":
                    cookie = "ru";
                    break;
                case "ENG":
                    cookie = "en";
                    break;
                case "BUL":
                    cookie = "bg";
                    break;
                case "ROM":
                    cookie = "ro";
                    break;
            }
            HttpContext.Current.Response.SetCookie(new HttpCookie("lang", cookie));
        }
    }
}
