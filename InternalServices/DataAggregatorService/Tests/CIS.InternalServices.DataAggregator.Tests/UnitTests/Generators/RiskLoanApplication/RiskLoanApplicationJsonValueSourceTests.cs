using CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.RiskLoanApplication;
using CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication;

namespace CIS.InternalServices.DataAggregator.Tests.UnitTests.Generators.RiskLoanApplication;

public class RiskLoanApplicationJsonValueSourceTests
{
    [Fact]
    public void ParseValue_Object_ShouldReturnSameObject()
    {
        var value = new object();
        var jsonValueSource = CreateJsonValueSource();

        var result = jsonValueSource.ParseValue(value, default!);

        result.Should().BeSameAs(value);
    }

    [Fact]
    public void ParseValue_NullWithoutUseDefaultInsteadOfNull_ShouldReturnNull()
    {
        int? value = null;
        var jsonValueSource = CreateJsonValueSource();

        var result = jsonValueSource.ParseValue(value, default!);

        result.Should().BeNull();
    }

    [Fact]
    public void ParseValue_NullWithUseDefaultInsteadOfNull_ShouldReturnDefault()
    {
        var obj = new { Value = (int?)null };
        var jsonValueSource = CreateJsonValueSource(true);

        var result = jsonValueSource.ParseValue(null, obj);

        result.Should().Be(default(int));
    }

    private static RiskLoanApplicationJsonValueSource CreateJsonValueSource(bool useDefaultInsteadOfNull = false)
    {
        var sourceField = new RiskLoanApplicationSourceField { FieldPath = "Value", DataService = DataService.General, UseDefaultInsteadOfNull = useDefaultInsteadOfNull };

        return new RiskLoanApplicationJsonValueSource(sourceField);
    }
}