using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregator.Tests.UnitTests.Services;

public class JsonBuilderPrimitiveTypesCollectionSourceTests
{
    private readonly IJsonValueSource _defaultJsonValueSource = Substitute.For<IJsonValueSource>();

    public JsonBuilderPrimitiveTypesCollectionSourceTests()
    {
        _defaultJsonValueSource.ParseValue(Arg.Any<object>(), Arg.Any<object>()).Returns(args => args.Args().First());
    }

    [Fact]
    public void ParseValue_Null_ShouldReturnEmptyCollection()
    {
        var jsonValueSource = CreateSource();

        var result = jsonValueSource.ParseValue(null, default!);

        result.Should().Be(Enumerable.Empty<object>());
    }

    [Fact]
    public void ParseValue_NotCollection_ShouldThrowInvalidOperation()
    {
        var jsonValueSource = CreateSource();

        var act = () => jsonValueSource.ParseValue(string.Empty, default!);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ParseValue_CollectionOfObjects_ShouldReturnPrimitiveTypesCollection()
    {
        var jsonValueSource = CreateSource();
        jsonValueSource.FieldPath = "[].Value";
        var collection = Enumerable.Range(1, 3).Select(x => new { Value = x }).ToArray();

        var result = jsonValueSource.ParseValue(collection, default!);
        var collectionResult = ((IEnumerable<object>)result).ToList();

        collectionResult.Should().HaveCount(collection.Length);
        collectionResult.Should().AllBeOfType<int>();
        _defaultJsonValueSource.Received(collection.Length).ParseValue(Arg.Any<object>(), Arg.Any<object>());
    }

    private JsonValuePrimitiveTypesCollectionSource CreateSource() => new(_defaultJsonValueSource, 0);
}