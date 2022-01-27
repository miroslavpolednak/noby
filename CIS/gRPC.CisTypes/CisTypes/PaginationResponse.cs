using CIS.Core.Types;

namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class PaginationResponse
    : IPaginableResponse
{
    public PaginationResponse(IPaginableRequest request, int recordsTotalSize)
    {
        RecordOffset = request.RecordOffset;
        PageSize = request.PageSize;
        RecordsTotalSize = recordsTotalSize;
        // sorting
        if (request.HasSorting && request.TypeOfSortingField == typeof(PaginationSortingField))
            Sorting.AddRange((IEnumerable<PaginationSortingField>)request.GetSorting()!);
        else if (request.HasSorting)
            Sorting.AddRange(request.GetSorting()!.Select(t => new PaginationSortingField() { Field = t.Field, Descending = t.Descending }));
    }
}
