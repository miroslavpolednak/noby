using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class SendToCmpMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }

    public SendToCmpMediatrRequest(SendToCmpRequest request)
    {
        SalesArrangementId = request.SalesArrangementId;
    }
}