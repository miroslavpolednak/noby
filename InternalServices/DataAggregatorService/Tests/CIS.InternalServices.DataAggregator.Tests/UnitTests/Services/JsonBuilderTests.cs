using System.Text.Json;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;
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
        const string FieldPath = "Value";
        var testObject = new { Value = 0 };

        _sut.Add(JsonPath, FieldPath);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(testObject, _serializerOptions));
    }

    [Fact]
    public void JsonBuilder_MapComplexPropertyToSimpleProperty_ShouldReturnJsonWithSimpleProperty()
    {
        const string JsonPath = "Value";
        const string FieldPath = "Property.Value";
        var testObject = new { Property = new { Value = 0 } };

        _sut.Add(JsonPath, FieldPath);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(new { testObject.Property.Value }, _serializerOptions));
    }

    [Fact]
    public void JsonBuilder_MapSimplePropertyToComplexJsonComplexProperty_ShouldReturnJsonWithComplexProperty()
    {
        const string JsonPath = "Object.Property";
        const string FieldPath = "Value";
        var testObject = new { Value = 0 };

        _sut.Add(JsonPath, FieldPath);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(new { Object = new { Property = testObject.Value } }, _serializerOptions));
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
    }

    [Fact]
    public void JsonBuilder_MapPropertyFromCollectionToJsonCollectionProperty_ShouldReturnJsonWithCollectionOfPrimitiveTypes()
    {
        const string JsonPath = "Collection[]";
        const string FieldPath = $"Value{ConfigurationConstants.CollectionMarker}.Property";
        var testObject = new { Value = new[] { new { Property = 0 } } };

        _sut.Add(JsonPath, FieldPath);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(new { Collection = testObject.Value.Select(x => x.Property) }, _serializerOptions));
    }

    [Fact]
    public void JsonBuilder_MapCollectionToJsonCollection_ShouldReturnJsonWithCollection()
    {
        const string JsonPath = "Collection[].Property";
        const string FieldPath = "Collection[].Property";
        var testObject = new { Collection = new[] { new { Property = 0 } } };

        _sut.Add(JsonPath, FieldPath);
        var json = _sut.Serialize(testObject, _serializerOptions);

        json.Should().BeEquivalentTo(JsonSerializer.Serialize(testObject, _serializerOptions));
    }

    [Fact]
    public void JsonBuilder_InvalidFieldPath_ShouldThrow()
    {
        const string JsonPath = "Value";
        const string FieldPath = "InvalidPath";
        var testObject = new { Value = 0 };

        _sut.Add(JsonPath, FieldPath);
        var act = () => _sut.Serialize(testObject, _serializerOptions);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}