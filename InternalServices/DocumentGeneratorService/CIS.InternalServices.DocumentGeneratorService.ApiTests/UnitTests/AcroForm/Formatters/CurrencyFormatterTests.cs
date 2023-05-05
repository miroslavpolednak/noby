using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat.Formatters;
using FluentAssertions;
using Xunit;

namespace CIS.InternalServices.DocumentGeneratorService.Tests.UnitTests.AcroForm.Formatters;

public class CurrencyFormatterTests
{
    [Fact]
    public void Format_ObjIsNotDecimal_ShouldThrowArgument()
    {
        var sut = CurrencyFormatter.Instance;

        Action act = () => sut.Format("Not a number", CultureInfoFixture.CultureInfo);

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(1000, "1\u00A0000 Kč")] // \u00A0 = Non-breaking space
    [InlineData(0, "0 Kč")]
    [InlineData(0.1, "0,1 Kč")]
    [InlineData(0.11, "0,11 Kč")]
    [InlineData(0.111, "0,11 Kč")]
    public void Format_ValidNumber_ShouldFormatCurrency(decimal value, string expectedText)
    {
        var sut = CurrencyFormatter.Instance;

        var formattedText = sut.Format(value, CultureInfoFixture.CultureInfo);

        formattedText.Should().BeEquivalentTo(expectedText);
    }
}