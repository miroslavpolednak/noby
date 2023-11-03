using System.Collections;
using FastMember;

namespace CIS.InternalServices.DataAggregatorService.Api.Helpers;

internal static class MapperHelper
{
    public static void MapInputParameters(InputParameters parameters, DynamicInputParameter dynamicParameter, AggregatedData data)
    {
        var propertyValue = GetValue(data, dynamicParameter.SourceFieldPath)
                            ?? throw new InvalidOperationException($"Requested dynamic parameter '{dynamicParameter.SourceFieldPath}' has returned null.");

        var typeAccessor = TypeAccessor.Create(typeof(InputParameters));

        if (propertyValue is IEnumerable enumerableSource)
        {
            if (typeAccessor[parameters, dynamicParameter.InputParameter] is not IList list)
                throw new InvalidOperationException($"{dynamicParameter.InputParameter} is not a collection");

            foreach (var obj in enumerableSource) 
                list.Add(obj);

            return;
        }

        typeAccessor[parameters, dynamicParameter.InputParameter] = propertyValue;
    }

    public static object? GetValue(object obj, string fullPropertyName)
    {
        var memberNames = fullPropertyName.Split('.');

        try
        {
            if (memberNames.Length == 1)
            {
                return string.IsNullOrWhiteSpace(memberNames.First()) ? obj : ObjectAccessor.Create(obj)[memberNames.First()];
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
        catch (Exception ex)
        {
            throw new InvalidOperationException($"GetValue for property '{fullPropertyName}' of the {obj.GetType().FullName} threw exception", ex);
        }
    }

    public static Type GetType(object obj, string fullPropertyName)
    {
        var memberNames = fullPropertyName.Split('.');
        var currentType = obj.GetType();

        try
        {
            if (memberNames.Length == 1)
            {
                if (string.IsNullOrWhiteSpace(memberNames.First()))
                    return currentType;

                return TypeAccessor.Create(currentType).GetMembers().First(m => m.Name == memberNames.First()).Type;
            }

            foreach (var propertyName in memberNames)
            {
                var members = TypeAccessor.Create(currentType).GetMembers();

                currentType = members.First(m => m.Name == propertyName).Type;
            }

            return currentType;

        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"GetType for property '{fullPropertyName}' of the {currentType.FullName} threw exception", ex);
        }
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