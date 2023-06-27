using CIS.InternalServices.NotificationService.Api.Helpers;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.ParsePhone;

public class ParsePhoneTests
{
    [Theory]
    [InlineData("+42077712345x")]
    [InlineData("+4207771234x6")]
    [InlineData("++420777123456")]
    [InlineData("777123456")]
    [InlineData("0420777123456")]
    [InlineData("+4207771234567890")]
    public void ParseInvalidValues(string value)
    {
        var phone = value.ParsePhone();
        Assert.Null(phone);
    }
    
    [Fact]
    public void ParseShouldRemoveSpaces()
    {
        var value = " + 4 2 0 7 7 7 1 2 3 4 5 6     ";
        var phone = value.ParsePhone();
        Assert.NotNull(phone);
        Assert.Equal("+420", phone.CountryCode);
        Assert.Equal("777123456", phone.NationalNumber);
    }

    [Fact]
    public void ParseShouldConvertZerosIntoPlus()
    {
        var value = "00420777123456";
        var phone = value.ParsePhone();
        Assert.NotNull(phone);
        Assert.Equal("+420", phone.CountryCode);
        Assert.Equal("777123456", phone.NationalNumber);
    }
    
    [Fact]
    public void ParseShouldRemoveSpaceAndConvertZerosIntoPlus()
    {
        var value = " 0 0 4 2 0 7 7 7 1 2 3 4 5 6     ";
        var phone = value.ParsePhone();
        Assert.NotNull(phone);
        Assert.Equal("+420", phone.CountryCode);
        Assert.Equal("777123456", phone.NationalNumber);
    }
}