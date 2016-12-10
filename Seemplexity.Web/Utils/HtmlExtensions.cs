using System;
using System.Web.Mvc;

namespace Seemplexity.Web.Utils
{
    public static class HtmlExtensions
    {
        //TODO: Доделать 
        public static MvcHtmlString Image(this HtmlHelper html, byte[] image)
        {
            var img = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
            return new MvcHtmlString("<img src='" + img + "' class='bus-description' />");
        }
    }
}