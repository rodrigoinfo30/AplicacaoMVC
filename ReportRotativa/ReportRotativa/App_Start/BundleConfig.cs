using System.Web;
using System.Web.Optimization;

namespace MoneyReport
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

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
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Layouts/Menu/css").Include(
          "~/Layouts/Menu/css/bootstrap.css",
          "~/Layouts/Menu/css/bootstrap.min.css",
          "~/Layouts/Menu/css/Buttonbusca.css",
          "~/Layouts/Menu/css/component.css",
          "~/Layouts/Menu/css/componentrel.css",
          "~/Layouts/Menu/css/demo.css",
          "~/Layouts/Menu/css/demo2.css",
          "~/Layouts/Menu/css/demorel.css",
          "~/Layouts/Menu/css/modal.css",
          "~/Layouts/Menu/css/normalize.css"));

            //#if DEBUG
            //            BundleTable.EnableOptimizations = false;
            //#else
            //            BundleTable.EnableOptimizations = true;
            //#endif

            //            bundles.Add(new ScriptBundle("~/bundles/jquerymobile").Include("~/Scripts/jquery.mobile-{version}.js"))

            //  bundles.Add(new StyleBundle("~/Menu/js").Include(
            //"~/Menu/css/bootstrap.css",
            //"~/Menu/css/bootstrap.min.css",
            //"~/Menu/css/Buttonbusca.css",
            //"~/Menu/css/component.css",
            //"~/Menu/css/componentrel.css",
            //"~/Menu/css/demo.css",
            //"~/Menu/css/demo2.css",
            //"~/Menu/css/demorel.css",
            //"~/Menu/css/modal.css",
            //"~/Menu/css/normalize.css"));
        }
    }
}
