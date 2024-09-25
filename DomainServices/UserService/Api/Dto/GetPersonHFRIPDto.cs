namespace DomainServices.UserService.Api.Dto;

internal sealed record GetPersonHFRIPDto(
    long PersonId,
    string? PersonSurname,
    string? PersonOrgUnitId,
    string? PersonOrgUnitName,
    string? PersonJobPostId,
    string? Company,
    long? BrokerId,
    int? DealerCompanyId,
    long? BrokerIdVZ)
{
}
