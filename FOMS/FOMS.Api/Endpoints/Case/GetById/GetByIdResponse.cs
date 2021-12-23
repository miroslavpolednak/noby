namespace FOMS.Api.Endpoints.Case.Dto;

internal class GetByIdResponse
{
    public long CaseId { get; set; }
    public int State { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ContractNumber { get; set; }
}
