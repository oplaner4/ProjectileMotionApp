using System.Web;
using System.Web.Optimization;

namespace ProjectileMotionWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Scripts").Include(
                        "~/Scripts/External/jquery-3.4.1.js",
                        "~/Scripts/External/popper.js",
                        "~/Scripts/External/bootstrap.js",
                        "~/fontawesome/js/all.js",
                         "~/Scripts/ChartjsDefiningVariables.js",
                        "~/Scripts/External/Chart.js",
                        "~/Scripts/Script.js"
                        )
            );

            bundles.Add(new StyleBundle("~/bundles/Styles").Include(
                    "~/Css/External/bootstrap.css",
                    "~/Css/External/pretty-checkbox.min.css",
                    "~/Css/External/Chart.css",
                    "~/Css/Style.css"
                )
            );

            BundleTable.EnableOptimizations = true;
        }
    }
}
