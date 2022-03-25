namespace DomainServices.SalesArrangementService.Api.Dto;

internal record UpdateSalesArrangementParametersMediatrRequest(Contracts.UpdateSalesArrangementParametersRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
}
