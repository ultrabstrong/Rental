using Rental.WebApp.Enums;
using Rental.WebApp.Mappers;
using DomainWageType = Rental.Domain.Enums.WageType;
using DomainYesNo = Rental.Domain.Enums.YesNo;

namespace Rental.UnitTests.WebApp.Mappers;

public class EnumMappingHelpersTests
{
    [Fact]
    public void MapEnum_YesNo_RoundTrip()
    {
        YesNo? src = YesNo.Yes;
        var mapped = src.MapEnum<YesNo, DomainYesNo>();
        Assert.NotNull(mapped);
        Assert.Equal((int)src.Value, (int)mapped!.Value);
    }

    [Fact]
    public void MapEnum_WageType_RoundTrip()
    {
        WageType? src = WageType.Hourly;
        var mapped = src.MapEnum<WageType, DomainWageType>();
        Assert.NotNull(mapped);
        Assert.Equal((int)src.Value, (int)mapped!.Value);
    }

    [Fact]
    public void MapEnum_Null_ReturnsNull()
    {
        YesNo? src = null;
        var mapped = src.MapEnum<YesNo, DomainYesNo>();
        Assert.Null(mapped);
    }
}
