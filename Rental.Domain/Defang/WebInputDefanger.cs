using System.Text;
using System.Text.RegularExpressions;

namespace Rental.Domain.Defang;

internal partial class WebInputDefanger : IDefanger
{
    // Rough email token detection to avoid defanging valid addresses
    [GeneratedRegex(
        @"(?<=\b)[A-Za-z0-9._%+\-]+@[A-Za-z0-9.\-]+\.[A-Za-z]{2,63}(?=\b)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    )]
    private static partial Regex EmailToken();

    // Detect URLs, schemes, and obvious domains (not including @ to avoid emails)
    // Also match scheme tokens followed by ':' with optional '//' (e.g., "javascript:", "http://")
    [GeneratedRegex(
        @"(?:(?:https?|ftp|file|data|javascript|mailto|tel|sms):\/{0,2})|\bwww\.|\b([a-z0-9\-]+\.)+[a-z]{2,63}\b",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
    )]
    private static partial Regex UrlLike();

    // Strip HTML tags and script/style blocks
    [GeneratedRegex(
        @"<\/?(script|style)[^>]*?>[\s\S]*?<\/\1>|<[^>]+>",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
    )]
    private static partial Regex Tags();

    // Match dots in host-like parts to defang
    [GeneratedRegex(@"(?<=\b)([A-Za-z0-9\-]+)\.(?=[A-Za-z])")]
    private static partial Regex DotsInHosts();

    // Invisible/bi-di/control characters to remove
    [GeneratedRegex(
        @"[\u0000-\u001F\u007F\u200B-\u200D\u202A-\u202E\u2066-\u2069]",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    )]
    private static partial Regex Controls();

    // Collapse excessive whitespace (more than one space/newline)
    [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant)]
    private static partial Regex ExcessiveWhitespace();

    public string Defang(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Normalize Unicode to NFC to reduce spoofing variance
        var text = input.Normalize(NormalizationForm.FormC);

        // Decode common HTML entities quickly (basic subset)
        text = text.Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("&amp;", "&")
            .Replace("&quot;", "\"")
            .Replace("&#39;", "'");

        // Remove tags/script/style
        text = Tags().Replace(text, string.Empty);

        // Remove control and invisible chars
        text = Controls().Replace(text, string.Empty);

        // Collapse excessive whitespace while preserving single newlines
        text = ExcessiveWhitespace().Replace(text, m => m.Value.Contains('\n') ? "\n" : " ");

        // Defang URLs and link-like content, but keep email addresses intact
        text = DefangUrlsPreservingEmails(text);

        // Final trim
        text = text.Trim();
        return text;
    }

    private static string DefangUrlsPreservingEmails(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        // Temporarily protect email tokens
        var protectedMap = new Dictionary<string, string>();
        int i = 0;
        string Protector(string m)
        {
            var key = $"__EMAIL_TOKEN_{i++}__";
            protectedMap[key] = m;
            return key;
        }
        var protectedText = EmailToken().Replace(text, m => Protector(m.Value));

        // Defang URL-like patterns
        protectedText = UrlLike().Replace(protectedText, m => DefangMatch(m.Value));

        // Restore email tokens
        foreach (var kvp in protectedMap)
        {
            protectedText = protectedText.Replace(kvp.Key, kvp.Value);
        }
        return protectedText;
    }

    private static string DefangMatch(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;
        var v = value;
        // Neutralize "://" if present
        v = v.Replace("://", "[colon]//");
        // If it's a plain scheme token like "javascript:" or "mailto:", neutralize the colon
        if (v.EndsWith(':'))
        {
            v = v[..^1] + "[colon]";
        }
        // defang dots in host-like parts
        v = DotsInHosts().Replace(v, "$1[dot]");
        return v;
    }
}
