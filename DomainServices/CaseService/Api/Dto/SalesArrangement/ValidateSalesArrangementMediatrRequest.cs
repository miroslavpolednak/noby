namespace DomainServices.CaseService.Api.Dto.SalesArrangement;

internal sealed class ValidateSalesArrangementMediatrRequest
    : IRequest<Contracts.ValidateSalesArrangementResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }

    public ValidateSalesArrangementMediatrRequest(Contracts.SalesArrangementIdRequest request)
    { 
        SalesArrangementId = request.SalesArrangementId;
    }
}
