using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using FastMember;

namespace CIS.InternalServices.DataAggregatorService.Api.Helpers;

internal static class MapperHelper
{
    public static void MapInputParameters(InputParameters parameters, DynamicInputParameter dynamicParameter, AggregatedData data)
    {
        var propertyValue = GetValue(data, dynamicParameter.SourceFieldPath);

        if (propertyValue == null)
            throw new InvalidOperationException($"Requested dynamic parameter '{dynamicParameter.SourceFieldPath}' has returned null.");

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

            if (currentObject is null)
                break;
        }

        return currentObject;
    }

    public static Type GetType(object obj, string fullPropertyName)
    {
        var memberNames = fullPropertyName.Split('.');

        var currentType = obj.GetType();

        if (memberNames.Length == 1)
        {
            return TypeAccessor.Create(currentType).GetMembers().First(m => m.Name == memberNames.First()).Type;
        }

        foreach (var propertyName in memberNames)
        {
            var members = TypeAccessor.Create(currentType).GetMembers();

            currentType = members.First(m => m.Name == propertyName).Type;
        }

        return currentType;
    }

    public static object ConvertObjectImplicitly(object obj)
    {
        var implicitConverter = obj.GetType()
                                   .GetMethods()
                                   .FirstOrDefault(m => m.Name == "op_Implicit" && m.GetParameters().Single().ParameterType == obj.GetType());

        if (implicitConverter is null)
            return obj;

        return implicitConverter.Invoke(null, new[] { obj }) ?? throw new InvalidOperationException();
    }
}