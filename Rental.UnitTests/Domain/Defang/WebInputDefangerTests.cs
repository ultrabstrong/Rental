using Rental.Domain.Defang;

namespace Rental.UnitTests.Domain.Defang;

public class WebInputDefangerTests
{
    private readonly WebInputDefanger _sut = new();

    [Theory]
    [InlineData("hello", "hello")]
    [InlineData(" hello ", "hello")]
    [InlineData("<b>hi</b>", "hi")]
    public void Defang_BasicCases(string input, string expected)
    {
        var actual = _sut.Defang(input);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Defang_Defangs_Urls_But_Preserves_Emails()
    {
        var input = "Contact me at user@example.com and visit http://evil.example.com now";
        var output = _sut.Defang(input);
        var expected =
            "Contact me at user@example.com and visit http[colon]//evil[dot]example[dot]com now";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Defang_Removes_Control_And_Invisible_Chars()
    {
        var input = "a\u200B\u202Ea";
        var output = _sut.Defang(input);
        Assert.Equal("aa", output);
    }

    [Fact]
    public void Defang_Strips_Tags_And_Defangs_Links_In_Real_Attack_String()
    {
        // Using IANA-reserved example.com to avoid real external links while keeping structure similar
        var input =
            "Maintenance request for * * * <a href=\"http://example.com/index.php?y3znyy\">$3,222 payment available</a> * * * hs=8c4cd9b856a4b213243cf30ab02c1903* ???* from iasbhc ef3o7y";
        var output = _sut.Defang(input);
        var expected =
            "Maintenance request for * * * $3,222 payment available * * * hs=8c4cd9b856a4b213243cf30ab02c1903* ???* from iasbhc ef3o7y";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Defang_Defangs_Bare_Domains_And_WWW_And_Schemes()
    {
        var input = "Visit www.example.com example.org and https://sub.example.co.uk/path";
        var output = _sut.Defang(input);
        var expected =
            "Visit www.example[dot]com example[dot]org and https[colon]//sub[dot]example[dot]co[dot]uk/path";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Defang_Neutralizes_Dangerous_Schemes()
    {
        var input = "javascript:alert(1) mailto:someone@example.com";
        var output = _sut.Defang(input);
        var expected = "javascript[colon]alert(1) mailto[colon]someone@example.com";
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Defang_Removes_Script_Blocks_Completely()
    {
        var input = "before <script>alert(1)</script> after";
        var output = _sut.Defang(input);
        Assert.Equal("before after", output);
    }
}
