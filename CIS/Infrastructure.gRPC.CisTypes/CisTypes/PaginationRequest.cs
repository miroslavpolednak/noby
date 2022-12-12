namespace CIS.Infrastructure.gRPC.CisTypes;

public partial class PaginationRequest 
    : Core.Types.IPaginableRequest
{
    public bool HasSorting => Sorting is not null && Sorting.Any();
    public Type TypeOfSortingField => typeof(PaginationSortingField);
    public IEnumerable<Core.Types.IPaginableSortingField>? GetSorting() => Sorting;

    public PaginationRequest(Core.Types.IPaginableRequest request)
    {
        this.PageSize = request.PageSize;
        this.RecordOffset = request.RecordOffset;
        if (request.HasSorting)
            this.Sorting.AddRange(request.GetSorting()!.Select(t => new PaginationSortingField() { Field = t.Field, Descending = t.Descending }));
    }
}
