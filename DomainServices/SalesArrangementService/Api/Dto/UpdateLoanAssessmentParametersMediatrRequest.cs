namespace DomainServices.SalesArrangementService.Api.Dto;

internal record UpdateLoanAssessmentParametersMediatrRequest(Contracts.UpdateLoanAssessmentParametersRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
}
