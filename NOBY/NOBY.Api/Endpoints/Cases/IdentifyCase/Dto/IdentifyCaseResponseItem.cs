namespace NOBY.Api.Endpoints.Cases.IdentifyCase.Dto;

public sealed record IdentifyCaseResponseItem
{
    /// <summary>
    /// ID obchodního případu
    /// </summary>
    public long CaseId { get; set; }

    /// <summary>
    /// Typ vztahu
    /// </summary>
    public int? ContractRelationshipTypeId { get; set; }

    /// <summary>
	/// Stav Case - ciselnik CaseStates
	/// </summary>
	public SharedTypes.Enums.CaseStates? State { get; set; }

    /// <summary>
    /// Slovne nazev stavu Case.
    /// </summary>
    public string? StateName { get; set; }

    /// <summary>
    /// ČÍslo smlouvy. Odpovídá smlouvě uzavřené pro zřízení stavebního spoření, nebo hypotéky. Úvěry ze SS mají jiné číslo smlouvy (jiný suffix), ale nezobrazuje se na case.
    /// </summary>
    public string? ContractNumber { get; set; }

    /// <summary>
    /// Cílová částka zobrazující se na dashboardu.
    /// </summary>
    public decimal? TargetAmount { get; set; }

    /// <summary>
    /// Nazev produktu slovne - ciselnik CaseStates
    /// </summary>
    public string? ProductName { get; set; }

    public IdentifyCaseResponseItem() { }

    public IdentifyCaseResponseItem(long caseId)
    {
        CaseId = caseId;
    }
}