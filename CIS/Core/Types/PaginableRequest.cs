using System.Text.Json.Serialization;

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
        public string Field { get; set; }
        public bool Descending { get; init; } = true;

        [JsonConstructor]
        public SortingField(string field, bool descending)
        {
            Field = field;
            Descending = descending;
        }
    }

    public void ChangeSortingFields(List<(string Original, string ChangeTo)> fields)
    {
        if (Sort is null || !Sort.Any()) return;
        
        fields.ForEach(t =>
        {
            var f = Sort.FirstOrDefault(x => x.Field.Equals(t.Original, StringComparison.InvariantCultureIgnoreCase));
            if (f is not null) f.Field = t.ChangeTo;
        });
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
