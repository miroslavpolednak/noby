using CIS.Core.Types;
using System.Text.Json.Serialization;

namespace NOBY.ApiContracts;

public partial class SharedTypesPaginationRequest
    : IPaginableRequest
{
    [JsonIgnore]
    public bool HasSorting => Sorting is not null && Sorting.Count != 0;

    /// <summary>
    /// Pro interoperabilitu s gRPC typem PaginationRequest
    /// </summary>
    [JsonIgnore]
    public Type TypeOfSortingField => typeof(SharedTypesPaginationSortingField);

    /// <summary>
    /// Pro interoperabilitu s gRPC typem PaginationRequest
    /// </summary>
    public IEnumerable<IPaginableSortingField>? GetSorting() => Sorting;
}

public partial class SharedTypesPaginationResponse
    : IPaginableResponse
{
    public SharedTypesPaginationResponse() { }

    public SharedTypesPaginationResponse(IPaginableRequest request, int recordsTotalSize)
    {
        RecordOffset = request.RecordOffset;
        PageSize = request.PageSize;
        RecordsTotalSize = recordsTotalSize;
        if (request.HasSorting && request.TypeOfSortingField == typeof(SharedTypesPaginationSortingField))
            Sorting = request.GetSorting() as List<SharedTypesPaginationSortingField>;
        else if (request.HasSorting)
            Sorting = request.GetSorting()!.Select(t => new SharedTypesPaginationSortingField(t.Field, t.Descending)).ToList();
    }
}

public partial class SharedTypesPaginationSortingField
    : IPaginableSortingField
{
    public SharedTypesPaginationSortingField() { }

    public SharedTypesPaginationSortingField(string field, bool descending)
    {
        Field = field;
        Descending = descending;
    }
}