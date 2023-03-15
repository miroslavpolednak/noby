namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto;

public sealed class CaseStateChangedRequest
{
    /// <summary>
    /// Login uživatele - ČPM.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    public long CaseId { get; set; }
    
    /// <summary>
    /// Číslo smlouvy
    /// </summary>
    public string? ContractNumber { get; set; }

    /// <summary>
    /// Celé jméno klienta
    /// </summary>
    public string? ClientFullName { get; set; }

    public string CaseStateName { get; set; } = string.Empty;
    public int ProductTypeId { get; set; }
    
    public string OwnerUserCpm { get; set; } = string.Empty;
    public string? OwnerUserIcp { get; set; }
    
    public CIS.Foms.Enums.Mandants Mandant { get; set; }
    
    public string? RiskBusinessCaseId { get; set; }

    public bool? IsEmployeeBonusRequested { get; set; }
}