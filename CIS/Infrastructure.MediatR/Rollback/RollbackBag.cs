namespace CIS.Infrastructure.MediatR.Rollback;

/// <summary>
/// Vychozi implementace rollback uloziste
/// </summary>
internal sealed class RollbackBag
    : IRollbackBag
{
    private Dictionary<string, object> _traceValues = new();

    public bool ContainsKey(string key) => _traceValues.ContainsKey(key);

    public object? this[string key] => _traceValues.TryGetValue(key, out object? value) ? value : null;

    public int Count => _traceValues.Count;

    public void Add(string key, object value)
    {
        _traceValues.Add(key, value);
    }
}
