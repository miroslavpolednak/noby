using System.Text.Json.Serialization;

namespace CIS.Core.Types;

public sealed class PaginableSortingField
{
    public string Field { get; set; }
    public bool Descending { get; init; } = true;

    [JsonConstructor]
    public PaginableSortingField(string field, bool descending)
    {
        Field = field;
        Descending = descending;
    }
}
