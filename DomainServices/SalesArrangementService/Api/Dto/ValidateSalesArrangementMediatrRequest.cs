using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class ValidateSalesArrangementMediatrRequest
    : IRequest<ValidateSalesArrangementResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }

    public ValidateSalesArrangementMediatrRequest(SalesArrangementIdRequest request)
    {
        SalesArrangementId = request.SalesArrangementId;
    }
}