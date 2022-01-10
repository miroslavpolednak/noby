namespace CIS.Core.Types;

public class PaginableRequest
{
    public int RecordOffset { get; init; }
    public int PageSize { get; init; }
    public List<PaginableSortingField>? Sort { get; init; }

    public PaginableRequest() { }

    public PaginableRequest(int recordOffset, int pageSize, string sortField, bool sortDescending)
    {
        RecordOffset = recordOffset;
        PageSize = pageSize;
        Sort = new List<PaginableSortingField>(1)
        {
            new PaginableSortingField(sortField, sortDescending)
        };
    }

    public PaginableRequest WithSortFields(List<(string ClientField, string ServerField)> fields)
    {
        if (Sort is null || !Sort.Any()) return this;

        fields.ForEach(t =>
        {
            var f = Sort.FirstOrDefault(x => x.Field.Equals(t.ClientField, StringComparison.InvariantCultureIgnoreCase));
            if (f is not null) f.Field = t.ServerField;
        });

        return this;
    }

    /// <summary>
    /// Default instance of Pagination
    /// </summary>
    public static PaginableRequest Create()
        => new PaginableRequest
        {
            PageSize = 10,
            RecordOffset = 1
        };

    public static PaginableRequest Create(string field, bool descending)
        => new PaginableRequest
        {
            PageSize = 10,
            RecordOffset = 1,
            Sort = new()
            {
                new(field, descending)
            }
        };
}
