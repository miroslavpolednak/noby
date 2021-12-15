namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class ValidateSalesArrangementMediatrRequest
    : IRequest<Contracts.ValidateSalesArrangementResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }

    public ValidateSalesArrangementMediatrRequest(Contracts.SalesArrangementIdRequest request)
    { 
        SalesArrangementId = request.SalesArrangementId;
    }
}
