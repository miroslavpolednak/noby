using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CaseService.Contracts;

public partial class GetConfirmedPriceExceptionsRequest
    : MediatR.IRequest<GetConfirmedPriceExceptionsResponse>
{ }

public partial class DeleteConfirmedPriceExceptionRequest
    : MediatR.IRequest<Empty>
{ }