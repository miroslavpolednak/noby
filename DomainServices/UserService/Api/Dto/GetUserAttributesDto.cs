namespace DomainServices.UserService.Api.Dto;

internal sealed record GetUserAttributesDto(
    int v33id,
    string? email,
    string? phone,
    string? VIPFlag,
    string? personOrgUnitName,
    string? dealerCompanyName,
    int? distributionChannelId,
    string? companyCin,
    DateTime? archiveDate
    )
{
}
