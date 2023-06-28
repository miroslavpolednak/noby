namespace NOBY.Api.Endpoints.Cases.Dto;

public class CaseModel
{
	public CaseOwnerModel? CaseOwner { get; set; }

	/// <summary>
	/// Unikátní Id obchodního případu
	/// </summary>
	public long CaseId { get; set; }

	/// <summary>
	/// Id klienta
	/// </summary>
	public CIS.Foms.Types.CustomerIdentity? CustomerIdentity { get; set; }

	/// <summary>
	/// FO/FOP/Cizinec/Nezletilec: Jméno
	/// </summary>
	public string? FirstName { get; set; }
	
	/// <summary>
	/// FO/FOP/Cizinec/Nezletilec: Příjmení
	/// </summary>
	public string? LastName { get; set; }
	
	/// <summary>
	/// FO/FOP/Cizinec/Nezletilec: Datum narození klienta
	/// </summary>
	public DateTime? DateOfBirth { get; set; }
	
	/// <summary>
	/// Stav Case - ciselnik CaseStates
	/// </summary>
	public CIS.Foms.Enums.CaseStates State { get; set; }
	
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
	public decimal TargetAmount { get; set; }
	
	/// <summary>
	/// Nazev produktu slovne - ciselnik CaseStates
	/// </summary>
	public string? ProductName { get; set; }
	
	/// <summary>
	/// Datum vytvoreni Case
	/// </summary>
	public DateTime CreatedTime { get; set; }
	
	/// <summary>
	/// Jmeno a prijmeni uzivatele, ktery vytvoril Case
	/// </summary>
	public string? CreatedBy { get; set; }
	
	/// <summary>
	/// Datum a cas posledni zmeny Case.
	/// </summary>
	public DateTime StateUpdated { get; set; }

	public NOBY.Dto.ContactsDto? OfferContacts { get; set; }

    public List<Dto.TaskModel>? ActiveTasks { get; set; }
}
