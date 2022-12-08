namespace CIS.Core.Types;

/// <summary>
/// Implementace <see cref="IPaginableRequest"/>
/// </summary>
public sealed class Paginable 
    : IPaginableRequest
{
    public int RecordOffset { get; init; }
    public int PageSize { get; init; }
    public List<SortField>? Sorting { get; private set; }

    public bool HasSorting => Sorting is not null && Sorting.Any();
    public Type TypeOfSortingField => typeof(SortField);
    public IEnumerable<IPaginableSortingField>? GetSorting() => Sorting;

    /// <summary>
    /// Safe guard - nastavení max. množství vrácených záznamů
    /// </summary>
    private const int _maxPageSize = 1000;

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

    /// <summary>
    /// Vytvori request s podporou strankovani z jiného IPaginableRequest.
    /// </summary>
    /// <remarks>
    /// Používá se pro konverzi s FE requestu na gRPC request, což jsou dvě různé implementace.
    /// </remarks>
    public static Paginable FromRequest(IPaginableRequest? request, int defaultPageSize = 10, int defaultRecordOffset = 0)
    {
        if (request is null)
            return new Paginable(defaultRecordOffset, defaultPageSize);
        else
            return new Paginable(request.RecordOffset, request.PageSize == 0 ? _maxPageSize : request.PageSize, request.GetSorting());
    }

    /// <summary>
    /// Premapuje Field names na hodnoty v databazi. Zaroven kontroluje, ze v mapperu jsou vsechna Field names z puvodni kolekce.
    /// </summary>
    /// <exception cref="Exceptions.CisArgumentException">Sort Field not allowed</exception>
    public Paginable EnsureAndTranslateSortFields(IEnumerable<MapperField> mapper)
    {
        if (HasSorting)
        {
            Sorting!.ForEach(t =>
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                var field = mapper
                    .FirstOrDefault(x => x.Name.Equals(t.Field, StringComparison.OrdinalIgnoreCase)) 
                            ?? throw new Exceptions.CisArgumentException(13, "Sort Field not allowed", "Field");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
                t.Field = field.TranslateTo;
            });
        }
        return this;
    }

    /// <summary>
    /// Nastaví requestu výchozí řazení - pouze pokud již není řazení nastaveno.
    /// </summary>
    public Paginable SetDefaultSort(string field, bool descending)
    {
        if (!HasSorting)
            Sorting = new List<SortField>()
            {
                new SortField(field, descending)
            };
        return this;
    }

    public record MapperField(string Name, string TranslateTo);

    public class SortField 
        : IPaginableSortingField
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