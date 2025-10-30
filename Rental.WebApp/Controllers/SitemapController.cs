using System.Globalization;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Rental.WebApp.Controllers;

public class SitemapController : Controller
{
    [HttpGet, Route("sitemap.xml")]
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)] // cache 24h
    public IActionResult Index()
    {
        // Use your canonical base URL. If you prefer to build from the request,
        // ensure HTTPS redirection + forwarded headers are correctly configured.
        const string baseUrl = "https://apexpropertiesmt.com";

        var urls = new (string path, string changefreq, double priority)[]
        {
            ("/", "daily", 1.0),
            ("/TenantInfo", "monthly", 0.6),
            ("/ContactUs", "yearly", 0.3),
            ("/Privacy", "yearly", 0.1),
            ("/Terms", "yearly", 0.1),
            ("/DownloadApplication", "monthly", 0.5),
            ("/Apply", "monthly", 0.7),
            ("/MaintenanceRequest", "monthly", 0.5),
        };

        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement(
                ns + "urlset",
                urls.Select(u => new XElement(
                    ns + "url",
                    new XElement(ns + "loc", baseUrl + u.path),
                    new XElement(ns + "lastmod", today),
                    new XElement(ns + "changefreq", u.changefreq),
                    new XElement(
                        ns + "priority",
                        u.priority.ToString("0.0", CultureInfo.InvariantCulture)
                    )
                ))
            )
        );

        return Content(
            doc.ToString(SaveOptions.DisableFormatting),
            "application/xml; charset=utf-8"
        );
    }
}
