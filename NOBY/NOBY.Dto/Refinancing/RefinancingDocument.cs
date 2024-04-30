using NOBY.Dto.Workflow;

namespace NOBY.Dto.Refinancing;

public sealed class RefinancingDocument
{
    /// <summary>
    /// Noby task ID. Jde o ID sady úkolů generované Starbuildem.
    /// zafiltrovat seznam tasku na taskTypeId = 6 a neni cancelled
    /// </summary>
    /// <example>22777</example>
    public long? TaskId { get; set; }

    /// <summary>
    /// True pokud je k dispozici tlacitko Pokracovat
    /// </summary>
    public bool IsContinueEnabled { get; set; }

    public string DocumentName { get; set; } = null!;

    /// <summary>
    /// Název Noby stavu úkolu
    /// </summary>
    /// <example>K VYŘÍZENÍ</example>
    public string StateName { get; set; } = null!;

    /// <summary>
    /// Filter Noby stavu úkolu
    /// </summary>
    /// <example>1</example>
    public StateFilters StateFilter { get; set; }

    /// <summary>
    /// Indikátor barvy Noby stavu
    /// </summary>
    /// <example>1</example>
    public StateIndicators StateIndicator { get; set; }

    public int? SignatureTypeDetailId { get; set; }

    /// <summary>
    /// Označuje zda má být aktivní button Generovat dokument
    /// </summary>
    public bool IsGenerateDocumentEnabled { get; set; }
}
