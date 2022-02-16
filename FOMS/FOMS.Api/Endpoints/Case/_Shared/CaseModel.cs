namespace FOMS.Api.Endpoints.Case.Dto;

internal class CaseModel
{
	public long CaseId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public CIS.Foms.Enums.CaseStates State { get; set; }
	public string? StateName { get; set; }
	public bool ActionRequired { get; set; }
	public string? ContractNumber { get; set; }
	public decimal TargetAmount { get; set; }
	public string? ProductName { get; set; }
	public DateTime CreatedTime { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime StateUpdated { get; set; }
}
