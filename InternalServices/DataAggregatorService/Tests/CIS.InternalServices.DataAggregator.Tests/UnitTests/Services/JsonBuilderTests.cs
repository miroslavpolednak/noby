using System.Text.Json;
using CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication.Json;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregator.Tests.UnitTests.Services;

public class JsonBuilderTests
{
    private readonly JsonSerializerOptions _serializerOptions = new() { WriteIndented = true };

    [Fact]
    public void JsonBuilder_SimpleProperty_ShouldMapToSimpleJsonProperty()
    {
        var testObject = new { Value = 0 };
        var valueSource = new DefaultJsonValueSource { FieldPath = nameof(testObject.Value) };
        var jsonBuilder = new JsonBuilder();

        jsonBuilder.Add(nameof(testObject.Value), valueSource);
        var json = jsonBuilder.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(testObject, _serializerOptions));

        //Old
        var oldJsonBuilder = new RiskLoanApplicationJsonObjectImpl();

        oldJsonBuilder.Add(nameof(testObject.Value).Split('.'), nameof(testObject.Value), false);
        var oldJson = JsonSerializer.Serialize(oldJsonBuilder.GetJsonObject(testObject), _serializerOptions);

        oldJson.Should().BeEquivalentTo(json);
    }

    public void JsonBuilder_ComplexProperty_ShouldMapToSimpleJsonProperty()
    {
        var testObject = new { Property = new { Value = 0 } };
        var valueSource = new DefaultJsonValueSource { FieldPath = $"{nameof(testObject.Property)}.{nameof(testObject.Property.Value)}" };
        var jsonBuilder = new JsonBuilder();

        jsonBuilder.Add(nameof(testObject.Property.Value), valueSource);
        var json = jsonBuilder.Serialize(testObject, _serializerOptions);
    }
}