using System.Web.Mvc;

namespace ApartmentWeb.Filters
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            // No-cache headers
            response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate, private, max-age=0");
            response.AppendHeader("Pragma", "no-cache");
            response.AppendHeader("Expires", "-1");

            // Security headers - Improved CSP after script organization
            response.Headers.Add("Content-Security-Policy", 
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline'; " +  // Still needed for small config scripts
                "style-src 'self' 'unsafe-inline'; " +   // Still needed for conditional display styles
                "img-src 'self'; " +
                "font-src 'self'; " +
                "connect-src 'self'; " +
                "form-action 'self'; " +
                "frame-ancestors 'none'; " +
                "object-src 'none'; " +
                "media-src 'none'; " +
                "worker-src 'none';");
            
            response.Headers.Add("X-Content-Type-Options", "nosniff");
            response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            response.Headers.Add("X-XSS-Protection", "1; mode=block");
            response.Headers.Add("Referrer-Policy", "no-referrer");
            response.Headers.Add("Permissions-Policy", "geolocation=(), microphone=(), camera=()");

            base.OnResultExecuting(filterContext);
        }
    }
}