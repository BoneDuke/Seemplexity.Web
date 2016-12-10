using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seemplexity.Avalon.BusinesLogic.Utils;
using Seemplexity.Web.Models;

namespace Seemplexity.Web.ModelBinders
{
    public class DateModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var date = Parsers.ParseDateTime(request["Date"]);
            return date;
        }
    }
}