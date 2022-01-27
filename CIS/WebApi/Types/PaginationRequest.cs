namespace CIS.Infrastructure.WebApi.Types;

public class PaginationRequest
    : Core.Types.IPaginableRequest
{
    public int RecordOffset { get; init; }
    public int PageSize { get; init; }
    public List<PaginationSortingField>? Sorting { get; init; }

    public bool HasSorting => Sorting is not null && Sorting.Any();
    public Type TypeOfSortingField => typeof(PaginationSortingField);
    public IEnumerable<Core.Types.IPaginableSortingField>? GetSorting() => Sorting;
}
