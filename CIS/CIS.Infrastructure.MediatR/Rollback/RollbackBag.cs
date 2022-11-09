namespace CIS.Infrastructure.MediatR.Rollback;

/// <summary>
/// Vychozi implementace rollback uloziste
/// </summary>
internal sealed class RollbackBag
    : IRollbackBag
{
    private Dictionary<string, object> _traceValues = new();

    public object? this[string key] => _traceValues.ContainsKey(key) ? _traceValues[key] : null;

    public int Count => _traceValues.Count;

    public void Add(string key, object value)
    {
        _traceValues.Add(key, value);
    }
}
