using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat.Formatters;
using FluentAssertions;
using Xunit;

namespace CIS.InternalServices.DocumentGeneratorService.Tests.UnitTests.AcroForm.Formatters;

public class MonthsToYearsFormatterTests
{
    [Fact]
    public void Format_ObjIsNotInteger_ShouldThrowArgument()
    {
        var sut = MonthsToYearsFormatter.Instance;

        Action act = () => sut.Format("Not a number", CultureInfoFixture.CultureInfo);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Format_Zero_ShouldThrowArgumentOutOfRange()
    {
        var sut = MonthsToYearsFormatter.Instance;

        Action act = () => sut.Format(0, CultureInfoFixture.CultureInfo);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Format_NegativeInteger_ShouldThrowArgumentOutOfRange()
    {
        var sut = MonthsToYearsFormatter.Instance;

        Action act = () => sut.Format(int.MinValue, CultureInfoFixture.CultureInfo);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1, "1 měsíc")]
    [InlineData(12, "1 rok")]
    [InlineData(41, "3 roky a 5 měsíců")]
    public void Format_PositiveNumber_ShouldFormat(int months, string expectedText)
    {
        var sut = MonthsToYearsFormatter.Instance;

        var result = sut.Format(months, CultureInfoFixture.CultureInfo);

        result.Should().BeEquivalentTo(expectedText);
    }
}