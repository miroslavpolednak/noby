namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication.Json;

internal abstract class RiskLoanApplicationJsonObject
{
    protected int Depth { get; private init; }

    public abstract void Add(string[] propertyPath, string dataFieldPath, bool useDefaultInsteadOfNull);

    public abstract object? GetJsonObject(object data);

    protected RiskLoanApplicationJsonObject CreateValue(string dataFieldPath, bool useDefaultInsteadOfNull) =>
        new RiskLoanApplicationJsonValue
        {
            Depth = Depth + 1,
            DataFieldPath = dataFieldPath,
            UseDefaultInsteadOfNull = useDefaultInsteadOfNull
        };

    protected RiskLoanApplicationJsonObject CreateObject<TObject>() where TObject : RiskLoanApplicationJsonObject, new() =>
        new TObject
        {
            Depth = Depth + 1
        };
}