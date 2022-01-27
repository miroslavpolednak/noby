namespace CIS.Core.Types;

public interface IPaginableRequest
{
    int PageSize { get; }
    
    int RecordOffset { get; }
    
    bool HasSorting { get; }

    Type TypeOfSortingField { get; }

    IEnumerable<IPaginableSortingField>? GetSorting();
}
