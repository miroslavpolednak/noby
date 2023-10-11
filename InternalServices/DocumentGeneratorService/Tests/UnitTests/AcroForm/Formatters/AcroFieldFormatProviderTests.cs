using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;
using FluentAssertions;
using Xunit;

namespace CIS.InternalServices.DocumentGeneratorService.Tests.UnitTests.AcroForm.Formatters;

public class AcroFieldFormatProviderTests
{
    [Fact]
    public void GetFormat_CustomFormatter_ShouldReturnItself()
    {
        IFormatProvider formatProvider = new AcroFieldFormatProvider();

        var result = formatProvider.GetFormat(typeof(ICustomFormatter));

        result.Should().BeSameAs(result);
    }

    [Fact]
    public void CustomFormat_KnownCustomFormat_ShouldFormatUsingCustomFormatters()
    {
        const int Value = 1;
        const string ExpectedResult = "1 měsíc";

        ICustomFormatter sut = new AcroFieldFormatProvider();

        var result = sut.Format(CustomFormatterKeys.MonthsToYears, Value, CultureInfoFixture.CultureInfo);

        result.Should().BeEquivalentTo(ExpectedResult);
    }

    [Fact]
    public void CustomFormat_IFormattableObject_ShouldFormatUsingFormattable()
    {
        var value = DateTime.Now;

        ICustomFormatter sut = new AcroFieldFormatProvider();

        var result = sut.Format("d", value, CultureInfoFixture.CultureInfo);

        result.Should().BeEquivalentTo(value.ToString("d", CultureInfoFixture.CultureInfo));
    }

    [Fact]
    public void CustomFormat_ObjectWithoutFormattable_ShouldReturnDefaultToString()
    {
        var value = new object();

        ICustomFormatter sut = new AcroFieldFormatProvider();

        var result = sut.Format("{0}", value, CultureInfoFixture.CultureInfo);

        result.Should().BeEquivalentTo(value.ToString());
    }
}