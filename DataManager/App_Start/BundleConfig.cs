using System.Web;
using System.Web.Optimization;

namespace DataManager
{
    public class BundleConfig
    {
        #region Methods
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.UseCdn = true;

            // Javascript
            bundles.Add(new ScriptBundle("~/Scripts/Analogue").Include("~/Assets/Js/analogue.js"));

            Bundle jqueryBundle = new ScriptBundle("~/Scripts/jQuery", "//code.jquery.com/jquery-1.12.4.min.js").Include("~/Assets/Js/jquery-{version}.min.js");
            jqueryBundle.CdnFallbackExpression = "window.jquery";
            bundles.Add(jqueryBundle);

            Bundle jqueryUiBundle = new ScriptBundle("~/Scripts/jQueryUI", "//code.jquery.com/ui/1.11.4/jquery-ui.min.js").Include("~/Assets/Js/jquery-ui.min.js");
            jqueryUiBundle.CdnFallbackExpression = "window.jquery && window.jquery.ui";
            bundles.Add(jqueryUiBundle);

            bundles.Add(new ScriptBundle("~/Scripts/Global").Include(
               "~/Assets/Js/jquery-ui.js",
               "~/Assets/Js/jquery.datAjax.js",
               "~/Assets/Js/jQuery.html5forms.js",
               "~/Assets/Js/jquery.maskedinput.js",
               "~/Assets/Js/jquery.timepicker.js",
               "~/Assets/Js/select2.min.js",
               "~/Assets/Js/Global.js"));

            // CSS
            bundles.Add(new StyleBundle("~/Styles/FontAwesome", "//use.fontawesome.com/releases/v5.0.9/js/all.js"));

            bundles.Add(new StyleBundle("~/Styles/Global").Include(
                "~/Assets/Css/Reset.css",
                "~/Assets/Css/Global.css",
                "~/Assets/Css/Responsive.css",
                "~/Assets/Css/select2.min.css",
                "~/Assets/Css/jquery.timepicker.css"));

            bundles.Add(new StyleBundle("~/Styles/jQueryUI", "//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"));

            BundleTable.EnableOptimizations = true;
        }
        #endregion
    }
}
