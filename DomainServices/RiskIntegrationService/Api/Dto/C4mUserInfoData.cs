namespace DomainServices.RiskIntegrationService.Api.Dto;

internal sealed class C4mUserInfoData
{
    public long PersonId { get; set; }
    public string PersonSurname { get; set; } = String.Empty;
    public long? PersonOrgUnitId { get; set; }
    public string PersonOrgUnitName { get; set; } = String.Empty;
    public string PersonJobPostId { get; set; } = String.Empty;
    public int? DealerCompanyId { get; set; }
}
