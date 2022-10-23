using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;
using CIS.InternalServices.DocumentDataAggregator.DataServices.Dto;
using FastMember;

namespace CIS.InternalServices.DocumentDataAggregator.Mapper;

internal static class MapperExtensions
{
    public static void Map(this InputParameters parameters, DynamicInputParameter dynamicParameter, AggregatedData aggregatedData)
    {
        var propertyValue = aggregatedData.GetValue(dynamicParameter.SourceField.Path);

        if (propertyValue == null)
            throw new InvalidOperationException();

        TypeAccessor.Create(typeof(InputParameters))[parameters, dynamicParameter.InputParameterName] = propertyValue;
    }

    public static object? GetValue(this AggregatedData aggregatedData, string fullPropertyName)
    {
        var memberNames = fullPropertyName.Split('.');

        if (memberNames.Length == 1)
        {
            return ObjectAccessor.Create(aggregatedData)[memberNames.First()];
        }

        object currentObject = aggregatedData;

        foreach (var propertyName in memberNames)
        {
            var accessor = ObjectAccessor.Create(currentObject);

            currentObject = accessor[propertyName];
        }

        return currentObject;
    }
}