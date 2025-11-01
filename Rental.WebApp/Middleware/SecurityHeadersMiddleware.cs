using Microsoft.AspNetCore.Http;

namespace Rental.WebApp.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;

        // No-cache headers for HTML pages only (allow caching of static resources)
        if (
            context.Response.ContentType?.Contains("text/html") == true
            || context.Request.Path.Value?.EndsWith(".cshtml") == true
        )
        {
            response.Headers.Append(
                "Cache-Control",
                "no-cache, no-store, must-revalidate, private, max-age=0"
            );
            response.Headers.Append("Pragma", "no-cache");
            response.Headers.Append("Expires", "-1");
        }

        // Security headers - Updated CSP for better icon and font support and Turnstile
        response.Headers.Append(
            "Content-Security-Policy",
            "default-src 'self'; "
                + "base-uri 'self'; "
                + "script-src 'self' 'unsafe-inline' https://challenges.cloudflare.com; "
                + // Still needed for small config scripts
                "style-src 'self' 'unsafe-inline'; "
                + // Still needed for conditional display styles
                "img-src 'self' data:; "
                + // Added data: for inline SVGs and base64 images
                "font-src 'self' data:; "
                + // Added data: for embedded fonts in CSS
                "connect-src 'self' https://challenges.cloudflare.com; "
                + "frame-src 'self' https://challenges.cloudflare.com; "
                + "form-action 'self'; "
                + "frame-ancestors 'none'; "
                + "object-src 'none'; "
                + "media-src 'none'; "
                + "worker-src 'none';"
        );

        response.Headers.Append("X-Content-Type-Options", "nosniff");
        response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
        response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        response.Headers.Append("X-XSS-Protection", "1; mode=block");
        response.Headers.Append("Referrer-Policy", "no-referrer");
        response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");

        await _next(context);
    }
}
