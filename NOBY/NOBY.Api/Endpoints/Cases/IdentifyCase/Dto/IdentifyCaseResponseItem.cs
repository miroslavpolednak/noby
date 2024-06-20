using SharedTypes.Types;

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
	public CaseStates? State { get; set; }

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

    /// <summary>
    /// Jméno a příjmení vlastníka case
    /// </summary>
    public string? CaseOwnerName { get; set; }

    /// <summary>
    /// Timestamp vytvoreni Case
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Datum a cas posledni zmeny Case
    /// </summary>
    public DateTime StateUpdatedOn { get; set; }

    public IdentifyCaseResponseItemCustomer Customer { get; set; } = null!;

    public List<IdentifyCaseResponseItemTask>? ActiveTasks { get; set; }

    public IdentifyCaseResponseItem() { }

    public IdentifyCaseResponseItem(long caseId)
    {
        CaseId = caseId;
    }

    public sealed class IdentifyCaseResponseItemCustomer
    {
        /// <summary>
        /// Jméno customera
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Příjmení customera
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Identita klienta
        /// </summary>
        public CustomerIdentity? Identity { get; set; }

        /// <summary>
        /// Datum narození FO
        /// </summary>
        public DateOnly? DateOfBirth { get; set; }
    }

    public sealed class IdentifyCaseResponseItemTask
    {
        public int CategoryId { get; set; }

        public int TaskCount { get; set; }
    }
}