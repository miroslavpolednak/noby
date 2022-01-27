namespace CIS.Core.Types;

public sealed class Paginable : IPaginableRequest
{
    public int RecordOffset { get; init; }
    public int PageSize { get; init; }
    public List<SortField>? Sorting { get; private set; } = null;

    public bool HasSorting => Sorting is not null && Sorting.Any();
    public Type TypeOfSortingField => typeof(SortField);
    public IEnumerable<IPaginableSortingField>? GetSorting() => Sorting;

    public override string ToString()
        => $"RecordOffset: {RecordOffset}; PageSize: {PageSize}; Sorting: {HasSorting}";

    public Paginable(int recordOffset, int pageSize)
    {
        RecordOffset = recordOffset;
        PageSize = pageSize;
    }

    public Paginable(int recordOffset, int pageSize, IEnumerable<IPaginableSortingField>? sorting) 
        : this(recordOffset, pageSize)
    {
        if (sorting is not null && sorting.Any())
            Sorting = sorting.Select(t => new SortField(t)).ToList();
    }

    public static Paginable FromRequest(IPaginableRequest? request, int defaultPageSize = 10, int defaultRecordOffset = 1)
    {
        if (request is null || request.PageSize <= 0)
            return new Paginable(defaultRecordOffset, defaultPageSize);
        else
            return new Paginable(request.RecordOffset, request.PageSize, request.GetSorting());
    }

    /// <summary>
    /// Premapuje Field names na hodnoty v databazi. Zaroven kontroluje, ze v mapperu jsou vsechna Field names z puvodni kolekce.
    /// </summary>
    public Paginable EnsureAndTranslateSortFields(IEnumerable<MapperField> mapper)
    {
        if (HasSorting)
        {
            Sorting!.ForEach(t =>
            {
                var field = mapper.FirstOrDefault(x => x.Name.Equals(t.Field, StringComparison.InvariantCultureIgnoreCase)) ?? throw new Exceptions.CisArgumentException(13, "Sort Field not allowed", "Field");
                t.Field = field.TranslateTo;
            });
        }
        return this;
    }

    public record MapperField(string Name, string TranslateTo);

    public class SortField : IPaginableSortingField
    {
        public string Field { get; internal set; }
        public bool Descending { get; init; }

        internal SortField(string field, bool descending)
        {
            Field = field;
            Descending = descending;
        }

        internal SortField(IPaginableSortingField field) : this(field.Field, field.Descending) { }
    }
}