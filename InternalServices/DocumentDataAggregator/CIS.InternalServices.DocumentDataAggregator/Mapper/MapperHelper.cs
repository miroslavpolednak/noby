using CIS.InternalServices.DocumentDataAggregator.DataServices;
using FastMember;

namespace CIS.InternalServices.DocumentDataAggregator.Mapper;

internal static class MapperHelper
{
    public static void MapInputParameters(InputParameters parameters, DynamicInputParameter dynamicParameter, AggregatedData data)
    {
        var propertyValue = GetValue(data, dynamicParameter.SourceFieldPath);

        if (propertyValue == null)
            throw new InvalidOperationException();

        TypeAccessor.Create(typeof(InputParameters))[parameters, dynamicParameter.InputParameterName] = propertyValue;
    }

    public static object? GetValue(object obj, string fullPropertyName)
    {
        var memberNames = fullPropertyName.Split('.');

        if (memberNames.Length == 1)
        {
            return ObjectAccessor.Create(obj)[memberNames.First()];
        }

        var currentObject = obj;

        foreach (var propertyName in memberNames)
        {
            var accessor = ObjectAccessor.Create(currentObject);

            currentObject = accessor[propertyName];
        }

        return currentObject;
    }
}