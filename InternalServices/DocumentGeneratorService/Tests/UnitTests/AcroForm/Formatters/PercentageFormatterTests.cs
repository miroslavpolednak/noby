using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat.Formatters;
using FluentAssertions;
using Xunit;

namespace CIS.InternalServices.DocumentGeneratorService.Tests.UnitTests.AcroForm.Formatters;

public class PercentageFormatterTests
{
    [Fact]
    public void Format_ObjIsDecimal_ShouldThrowArgument()
    {
        var sut = PercentageFormatter.Instance;

        Action act = () => sut.Format("Not a number", CultureInfoFixture.CultureInfo);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Format_ValidNumber_ShouldFormat()
    {
        const decimal Value = 1;

        var sut = PercentageFormatter.Instance;

        var result = sut.Format(Value, CultureInfoFixture.CultureInfo);

        result.Should().BeEquivalentTo((Value / 100).ToString("P2", CultureInfoFixture.CultureInfo));
    }
}