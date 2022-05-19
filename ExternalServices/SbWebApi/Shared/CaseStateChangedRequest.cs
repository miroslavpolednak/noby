namespace ExternalServices.SbWebApi.Shared;

public sealed record CaseStateChangedRequest(
    long CaseId,
    string? ContractNumber,
    string? FullName,
    string CaseStateName,
    int ProductTypeId,
    string OwnerUserCpm,
    string? OwnerUserIcp,
    CIS.Foms.Enums.Mandants Mandant,
    string? RiskBusinessCaseId)
{
}
