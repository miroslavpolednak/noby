namespace CIS.InternalServices.DocumentDataAggregator.EasForms.Json;

internal abstract class EasFormJsonObject
{
    protected int Depth { get; private init; }

    public abstract void Add(string[] propertyPath, string dataFieldPath);

    public abstract object? GetJsonObject(object data);

    protected EasFormJsonObject CreateValue(string dataFieldPath) =>
        new EasFormJsonValue
        {
            Depth = Depth + 1,
            DataFieldPath = dataFieldPath
        };

    protected EasFormJsonObject CreateObject<TObject>() where TObject : EasFormJsonObject, new() =>
        new TObject
        {
            Depth = Depth + 1
        };
}