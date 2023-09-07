using CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms;

namespace CIS.InternalServices.DataAggregator.Tests.UnitTests.Generators.EasForms;

public class EasFormJsonValueSourceTests
{
    private readonly EasFormSourceField _sourceField = new() { FieldPath = string.Empty, DataService = DataService.General };

    [Fact]
    public void ParseValue_Number_ShouldReturnString()
    {
        const int Value = 0;
        var jsonValueSource = new EasFormJsonValueSource(_sourceField);

        var result = jsonValueSource.ParseValue(Value, default!);

        result.Should().BeOfType<string>();
        result.Should().BeEquivalentTo(Value.ToString());
    }
}