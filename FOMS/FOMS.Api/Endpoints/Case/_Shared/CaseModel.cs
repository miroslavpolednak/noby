namespace FOMS.Api.Endpoints.Case.Dto;

internal class CaseModel
{
	public long CaseId { get; set; }
	public string? CustomerName { get; set; }
	public DateTime? CustomerDateOfBirth { get; set; }
	public int State { get; set; }
	public string? StateName { get; set; }
	public bool ActionRequired { get; set; }
	public string? ContractNumber { get; set; }
	public int? TargetAmount { get; set; }
	public string? ProductName { get; set; }
	public DateTime CreatedTime { get; set; }
	public string? CreatedBy { get; set; }
}
