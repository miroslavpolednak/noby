namespace SharedTypes.GrpcTypes;

public partial class PaginationRequest 
    : CIS.Core.Types.IPaginableRequest
{
    public bool HasSorting => Sorting is not null && Sorting.Any();
    public Type TypeOfSortingField => typeof(PaginationSortingField);
    public IEnumerable<CIS.Core.Types.IPaginableSortingField>? GetSorting() => Sorting;

    public PaginationRequest(CIS.Core.Types.IPaginableRequest request)
    {
        this.PageSize = request.PageSize;
        this.RecordOffset = request.RecordOffset;
        if (request.HasSorting)
            this.Sorting.AddRange(request.GetSorting()!.Select(t => new PaginationSortingField() { Field = t.Field, Descending = t.Descending }));
    }
}
