using System.ComponentModel.DataAnnotations;
using NOBY.Api.Endpoints.Cases.IdentifyCase.Dto;

namespace NOBY.Api.Endpoints.Cases.IdentifyCase;

public sealed class IdentifyCaseRequest : IRequest<IdentifyCaseResponse>
{
    /// <summary>
    /// Kritérium vyhledávání: 0 - Čárový kód dokumentu (formId); 1 - Číslo úvěrového účtu; 2 - ID obchodního případu; 3 - Číslo smlouvy
    /// </summary>
    /// <example>0</example>
    [Required]
    public Criterion Criterion { get; set; }

    /// <summary>
    /// Tato hodnota je relevantní pro kritéria 0 (čárový kód)
    /// </summary>
    public string? FormId { get; set; }
    
    /// <summary>
    /// Tento objekt je relevantní pouze pro kritérium 1 (číslo úvěrového účtu).
    /// </summary>
    public PaymentAccount? Account { get; set; }
    
    /// <summary>
    /// Tato hodnota je relevantní pro kritéria 2 (ID obchodního případu)
    /// </summary>
    public long? CaseId { get; set; }
    
    /// <summary>
    /// Tato hodnota je relevantní pro kritéria 3 (číslo smlouvy)
    /// </summary>
    public string? ContractNumber { get; set; }
}

public enum Criterion
{
    FormId = 0,
    PaymentAccount = 1,
    CaseId = 2,
    ContractNumber = 3
}