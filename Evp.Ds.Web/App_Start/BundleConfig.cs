using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Evp.Ds.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //style bundler
            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/www/lib/bootstrap/dist/css/bootstrap.css")
                .IncludeDirectory("~/www/styles", "*.css", true));

            //javascript bundler.
            //IMPORTANT: Order of files is important.
            //library packages. 
            bundles.Add(new ScriptBundle("~/bundles/lib")
                .Include(
                    "~/www/lib/jquery/dist/jquery.js",
                    "~/www/lib/bootstrap/dist/js/bootstrap.js",
                    "~/www/lib/angular/angular.js",
                    "~/www/lib/angular-ui-router/release/angular-ui-router.js",
                    "~/www/lib/angular-bootstrap/ui-bootstrap-tpls.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/www/app/app.js")
                .IncludeDirectory("~/www/app", "*.js", true));
        }
    }
}