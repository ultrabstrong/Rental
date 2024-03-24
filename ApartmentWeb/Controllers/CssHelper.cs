using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.WebControls;

namespace ApartmentWeb.Controllers
{
    public static class CssHelper
    {
        public static IHtmlString EmbedCss(this HtmlHelper htmlHelper, string path)
        {
            try
            {
                StyleBundle b = (StyleBundle)BundleTable.Bundles.GetBundleFor("~/Content/css");
                BundleContext bc = new BundleContext(new HttpContextWrapper(HttpContext.Current), BundleTable.Bundles, "~/Content/css");
                List<BundleFile> files = b.EnumerateFiles(bc).ToList();

                string stylestring = "";
                foreach (BundleFile file in files)
                {
                    string filepath = HttpContext.Current.Server.MapPath(file.IncludedVirtualPath);
                    string filetext = File.ReadAllText(filepath);
                    stylestring += $"<!-- Style for {file.IncludedVirtualPath} -->\n<style>\n{filetext}\n</style>\n";
                }
                return htmlHelper.Raw(stylestring);
            }
            catch
            {
                // return nothing if we can't read the file for any reason
                return null;
            }
        }
    }
}