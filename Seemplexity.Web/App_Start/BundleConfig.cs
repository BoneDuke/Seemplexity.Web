using System.Web;
using System.Web.Optimization;

namespace Seemplexity.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                        "~/Scripts/bootstrap-datepicker/bootstrap-datepicker.js",
                        "~/Scripts/bootstrap-datepicker/locales/bootstrap-datepicker.ru.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.structure.css",
                      "~/Content/jquery-ui.theme.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.css",
                      "~/Content/flag-icon.css"));

            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
                        "~/Content/admin.css",
                        "~/Content/chosen/chosen.css"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
              "~/Scripts/Pages/Admin/BusScheme.js",
              "~/Scripts/chosen/chosen.jquery.js"));

            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                "~/Content/bootstrap-datepicker/bootstrap-datepicker3.css"));
        }
    }
}
