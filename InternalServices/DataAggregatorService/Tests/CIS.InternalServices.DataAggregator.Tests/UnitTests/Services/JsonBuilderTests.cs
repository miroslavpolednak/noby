using System.Text.Json;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;
using CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication.Json;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregator.Tests.UnitTests.Services;

public class JsonBuilderTests
{
    private readonly JsonSerializerOptions _serializerOptions = new() { WriteIndented = true };
    private readonly JsonBuilder _sut = new();

    [Fact]
    public void JsonBuilder_MapSimplePropertyToSimpleProperty_ShouldReturnJsonWithSimpleProperty()
    {
        const string JsonPath = "Value";
        var testObject = new { Value = 0 };
        var valueSource = new DefaultJsonValueSource { FieldPath = nameof(testObject.Value) };

        _sut.Add(JsonPath, valueSource);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(testObject, _serializerOptions));

        //Old
        var oldJsonBuilder = new RiskLoanApplicationJsonObjectImpl();

        oldJsonBuilder.Add(nameof(testObject.Value).Split('.'), nameof(testObject.Value), false);
        var oldJson = JsonSerializer.Serialize(oldJsonBuilder.GetJsonObject(testObject), _serializerOptions);

        oldJson.Should().BeEquivalentTo(json);
    }

    [Fact]
    public void JsonBuilder_MapComplexPropertyToSimpleProperty_ShouldReturnJsonWithSimpleProperty()
    {
        const string JsonPath = "Value";
        var testObject = new { Property = new { Value = 0 } };
        var valueSource = new DefaultJsonValueSource { FieldPath = $"{nameof(testObject.Property)}.{nameof(testObject.Property.Value)}" };

        _sut.Add(JsonPath, valueSource);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(new { Value = testObject.Property.Value }, _serializerOptions));

        //Old
        var oldJsonBuilder = new RiskLoanApplicationJsonObjectImpl();

        oldJsonBuilder.Add(nameof(testObject.Property.Value).Split('.'), $"{nameof(testObject.Property)}.{nameof(testObject.Property.Value)}", false);
        var oldJson = JsonSerializer.Serialize(oldJsonBuilder.GetJsonObject(testObject), _serializerOptions);

        oldJson.Should().BeEquivalentTo(json);
    }

    [Fact]
    public void JsonBuilder_MapSimplePropertyToComplexJsonComplexProperty_ShouldReturnJsonWithComplexProperty()
    {
        const string JsonPath = "Object.Property";
        var testObject = new { Value = 0 };
        var valueSource = new DefaultJsonValueSource { FieldPath = nameof(testObject.Value) };

        _sut.Add(JsonPath, valueSource);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(new { Object = new { Property = testObject.Value } }, _serializerOptions));

        //Old
        var oldJsonBuilder = new RiskLoanApplicationJsonObjectImpl();

        oldJsonBuilder.Add("Object.Property".Split('.'), nameof(testObject.Value), false);
        var oldJson = JsonSerializer.Serialize(oldJsonBuilder.GetJsonObject(testObject), _serializerOptions);

        oldJson.Should().BeEquivalentTo(json);
    }

    [Fact]
    public void JsonBuilder_MapPrimitiveTypesCollectionToJsonCollectionProperty_ShouldReturnJsonWithCollectionOfPrimitiveTypes()
    {
        const string JsonPath = "Collection[]";
        var testObject = new { Value = new[] { 1, 2, 3 } };
        var valueSource = new DefaultJsonValueSource { FieldPath = $"{nameof(testObject.Value)}{ConfigurationConstants.CollectionMarker}" };

        _sut.Add(JsonPath, valueSource);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(new { Collection = testObject.Value }, _serializerOptions));

        //Old
        var oldJsonBuilder = new RiskLoanApplicationJsonObjectImpl();

        oldJsonBuilder.Add(JsonPath.Split('.'), $"{nameof(testObject.Value)}{ConfigurationConstants.CollectionMarker}", false);
        var oldJson = JsonSerializer.Serialize(oldJsonBuilder.GetJsonObject(testObject), _serializerOptions);

        oldJson.Should().BeEquivalentTo(json);
    }

    [Fact]
    public void JsonBuilder_MapPropertyFromCollectionToJsonCollectionProperty_ShouldReturnJsonWithCollectionOfPrimitiveTypes()
    {
        const string JsonPath = "Collection[]";
        var testObject = new { Value = new[] { new { Property = 0 } } };
        var valueSource = new DefaultJsonValueSource { FieldPath = $"{nameof(testObject.Value)}{ConfigurationConstants.CollectionMarker}.Property" };

        _sut.Add(JsonPath, valueSource);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(new { Collection = testObject.Value.Select(x => x.Property) }, _serializerOptions));

        //Old
        var oldJsonBuilder = new RiskLoanApplicationJsonObjectImpl();

        oldJsonBuilder.Add(JsonPath.Split('.'), $"{nameof(testObject.Value)}{ConfigurationConstants.CollectionMarker}.Property", false);
        var oldJson = JsonSerializer.Serialize(oldJsonBuilder.GetJsonObject(testObject), _serializerOptions);

        oldJson.Should().BeEquivalentTo(json);
    }

    [Fact]
    public void JsonBuilder_MapCollectionToJsonCollection_ShouldReturnJsonWithCollection()
    {
        const string JsonPath = "Collection[].Property";
        var testObject = new { Collection = new[] { new { Property = 0 } } };
        var valueSource = new DefaultJsonValueSource { FieldPath = "Collection[].Property" };

        _sut.Add(JsonPath, valueSource);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(testObject, _serializerOptions));

        //Old
        var oldJsonBuilder = new RiskLoanApplicationJsonObjectImpl();

        oldJsonBuilder.Add(JsonPath.Split('.'), "Collection[].Property", false);
        var oldJson = JsonSerializer.Serialize(oldJsonBuilder.GetJsonObject(testObject), _serializerOptions);

        oldJson.Should().BeEquivalentTo(json);
    }

    [Fact]
    public void JsonBuilder_InvalidFieldPath_ShouldThrow()
    {
        const string JsonPath = "Value";
        var testObject = new { Value = 0 };
        var valueSource = new DefaultJsonValueSource { FieldPath = "InvalidPath" };

        _sut.Add(JsonPath, valueSource);
        var act = () => _sut.Serialize(testObject, _serializerOptions);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}