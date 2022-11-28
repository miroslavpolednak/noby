namespace ExternalServices.SbWebApi.Dto;

public sealed record CaseStateChangedRequest(
    string Login,
    long CaseId,
    string? ContractNumber,
    string? ClientFullName,
    string CaseStateName,
    int ProductTypeId,
    string OwnerUserCpm,
    string? OwnerUserIcp,
    CIS.Foms.Enums.Mandants Mandant,
    string? RiskBusinessCaseId)
{ }