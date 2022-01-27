namespace CIS.Infrastructure.WebApi.Types;

public sealed record class PaginationSortingField(string Field, bool Descending) 
    : Core.Types.IPaginableSortingField;
