using System.Web.Mvc;
using Seemplexity.Avalon.BusinesLogic.Utils;
using Seemplexity.Web.Models;

namespace Seemplexity.Web.ModelBinders
{
    public class BusFilterViewModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = (BusFilterViewModel)base.BindModel(controllerContext, bindingContext);
            var request = controllerContext.HttpContext.Request;
            result.Date = Parsers.ParseDateTime(request["Date"]);
            return result;
        }
    }
}