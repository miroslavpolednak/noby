using CIS.Core.Types;

namespace CIS.Infrastructure.WebApi.Types;

public sealed class PaginationResponse
    : IPaginableResponse
{
    public int RecordOffset { get; init; }
    public int PageSize { get; init; }
    public int RecordsTotalSize { get; init; }
    public List<PaginationSortingField>? Sorting { get; init; }

    public PaginationResponse() { }

    public PaginationResponse(IPaginableRequest request, int recordsTotalSize)
    {
        RecordOffset = request.RecordOffset;
        PageSize = request.PageSize;
        RecordsTotalSize = recordsTotalSize;
        if (request.HasSorting && request.TypeOfSortingField == typeof(PaginationSortingField))
            Sorting = request.GetSorting() as List<PaginationSortingField>;
        else if (request.HasSorting)
            Sorting = request.GetSorting()!.Select(t => new PaginationSortingField(t.Field, t.Descending)).ToList();
    }
}
