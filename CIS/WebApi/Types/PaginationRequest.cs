using System.Text.Json.Serialization;

namespace CIS.Infrastructure.WebApi.Types;

public class PaginationRequest
    : Core.Types.IPaginableRequest
{
    /// <summary>
    /// Offset (index, start=0) zaznamu, od ktereho se ma zacit s nacitanim
    /// </summary>
    public int RecordOffset { get; init; }
    
    /// <summary>
    /// Pocet zaznamu na jedne strance
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// [optional] Nastaveni razeni
    /// </summary>
    public List<PaginationSortingField>? Sorting { get; init; }

    [JsonIgnore]
    public bool HasSorting => Sorting is not null && Sorting.Any();
    
    /// <summary>
    /// Pro interoperabilitu s gRPC typem PaginationRequest
    /// </summary>
    [JsonIgnore]
    public Type TypeOfSortingField => typeof(PaginationSortingField);
    
    /// <summary>
    /// Pro interoperabilitu s gRPC typem PaginationRequest
    /// </summary>
    public IEnumerable<Core.Types.IPaginableSortingField>? GetSorting() => Sorting;
}
