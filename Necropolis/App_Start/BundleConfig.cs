using System.Web;
using System.Web.Optimization;

namespace Necropolis
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/camera.js",
                      "~/Scripts/jquery-1.10.2.js",
                      "~/Scripts/jquery-migrate-1.1.1.js",
                      "~/Scripts/jquery.easing.1.3.js",
                      "~/Scripts/jquery.js",
                      "~/Scripts/superfish.js",
                      "~/Scripts/jquery.mobilemenu.js",
                      "~/Scripts/jquery.mobile.customized.min.js",
                      "~/Scripts/touchTouch.jquery.js",
                      "~/Scripts/site.js",
                      "~/Scripts/jquery.slicknav.js"));

                   bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/responsive.css",
                      "~/Content/camera.css",
                      "~/Content/style.css",
                      "~/Content/touchTouch.css",
                      "~/Content/slicknav.css",
                      "~/Content/site.css"));
        }
    }
}
