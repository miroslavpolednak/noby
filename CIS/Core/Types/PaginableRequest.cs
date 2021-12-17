namespace CIS.Core.Types;

public class PaginableRequest
{
    public int RecordOffset { get; init; }
    public int PageSize { get; init; }
    public List<SortingField>? Sort { get; init; }

    public PaginableRequest() { }

    public PaginableRequest(int recordOffset, int pageSize, string sortField, bool sortDescending)
    {
        RecordOffset = recordOffset;
        PageSize = pageSize;
        Sort = new List<SortingField>(1)
        {
            new SortingField(sortField, sortDescending)
        };
    }

    public sealed class SortingField
    {
        public string Field { get; init; }
        public bool Descending { get; init; } = true;

        public SortingField(string field, bool descending)
        {
            Field = field;
            Descending = descending;
        }
    }
}
