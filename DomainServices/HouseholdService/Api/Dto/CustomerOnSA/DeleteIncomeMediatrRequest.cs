namespace DomainServices.HouseholdService.Api.Dto;

internal record DeleteIncomeMediatrRequest(int IncomeId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
