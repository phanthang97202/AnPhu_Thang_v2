using System.Web;
using System.Web.Optimization;

namespace idn.AnPhu.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region["Common"]
            #region["stylesheet"]
            bundles.Add(new StyleBundle("~/bundles/css-datetimepicker").Include(
                "~/Areas/Auth/Content/assets/css/bootstrap-datepicker3.min.css",
                "~/Areas/Auth/Content/assets/css/bootstrap-timepicker.min.css",
                "~/Areas/Auth/Content/assets/css/daterangepicker.min.css",
                "~/Areas/Auth/Content/assets/css/bootstrap-datetimepicker.min.css"
            ));
            #endregion
            #region["script"]
            bundles.Add(new ScriptBundle("~/bundles/jquery-datetimepicker").Include(
                "~/Areas/Auth/Content/assets/js/bootstrap-datepicker.min.js",
                "~/Areas/Auth/Content/assets/js/bootstrap-timepicker.min.js",
                "~/Areas/Auth/Content/assets/js/daterangepicker.min.js",
                "~/Areas/Auth/Content/assets/js/bootstrap-datetimepicker.min.js"
            ));
            #endregion
            #endregion

            #region["Administrator"]
            #region["stylesheet"]
            bundles.Add(new StyleBundle("~/Content/admin-bootstrap-ace").Include(
                "~/Areas/Auth/Content/assets/css/bootstrap.min.css",
                //"~/Areas/Auth/Content/assets/css/font-awesome.min.css",
                "~/Areas/Auth/Content/assets/font-awesome/4.5.0/css/font-awesome.min.css",
                "~/Areas/Auth/Content/assets/css/chosen.min.css",
                "~/Areas/Auth/Content/assets/css/fonts.googleapis.com.css",
                "~/Areas/Auth/Content/assets/css/ace.min.css",
                "~/Areas/Auth/Content/assets/css/ace-part2.min.css",
                "~/Areas/Auth/Content/assets/css/ace-skins.min.css",
                "~/Areas/Auth/Content/assets/css/ace-rtl.min.css"

            ));
            bundles.Add(new StyleBundle("~/Content/admin-jquery-ui-css").Include(
                "~/Areas/Auth/Content/jquery-ui/jquery-ui.min.css",
                "~/Areas/Auth/Content/jquery-ui/jquery-ui.custom.min.css"

            ));

            bundles.Add(new StyleBundle("~/Content/admin-css").Include(
                "~/Areas/Auth/Content/Administrator.css"

            ));
            #endregion
            #region["script"]
            bundles.Add(new ScriptBundle("~/bundles/admin-jquery-library").Include(
                "~/Areas/Auth/Content/assets/js/jquery-2.1.4.min.js",
                "~/Areas/Auth/Scripts/support-ie/ie-emulation-modes-warning.js",
                "~/Areas/Auth/Scripts/support-ie/ie10-viewport-bug-workaround.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/admin-jquery-library-lte-ie8").Include(
                    "~/Areas/Auth/Scripts/support-ie/html5shiv.min.js",
                    "~/Areas/Auth/Scripts/support-ie/respond.min.js",
                    "~/Areas/Auth/Scripts/js/excanvas.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/admin-jquery-show-dialog").Include(
                "~/Areas/Auth/Scripts/jquery-ui/jquery-ui.min.js",
                "~/Areas/Auth/Content/assets/js/jquery-ui.custom.min.js"//,
                                                                        //"~/Areas/Auth/Content/assets/js/jquery.ui.touch-punch.min.js"

            ));
            bundles.Add(new ScriptBundle("~/bundles/admin-jquery-bootstrap").Include(
                "~/Areas/Auth/Content/assets/js/bootstrap.min.js",
                "~/Areas/Auth/Content/assets/js/chosen.jquery.min.js",
                "~/Areas/Auth/Content/assets/js/moment.min.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/admin-jquery-ace").Include(
                "~/Areas/Auth/Content/assets/js/ace-extra.min.js",
                "~/Areas/Auth/Content/assets/js/ace-elements.min.js",
                "~/Areas/Auth/Content/assets/js/ace.min.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/admin-jquery-validate").Include(
                "~/Areas/Auth/Scripts/jquery-validate/jquery.validate.min.js",
                "~/Areas/Auth/Scripts/jquery-validate/jquery.form-2.68.js",
                "~/Areas/Auth/Scripts/jquery-validate/ajaxform.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/admin-jquery-library").Include(
                "~/Areas/Auth/Scripts/js/jquery-library-1.0.7.js"
            ));
            #endregion
            #endregion

            #region["Frontend"]

            #endregion

            BundleTable.EnableOptimizations = false;
            //BundleTable.EnableOptimizations = true;
        }
    }
}
