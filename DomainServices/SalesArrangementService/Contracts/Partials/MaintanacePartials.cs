using Google.Protobuf.WellKnownTypes;

namespace DomainServices.SalesArrangementService.Contracts;

public partial class GetCancelCaseJobIdsRequest
    : MediatR.IRequest<GetCancelCaseJobIdsResponse>
{ }

public partial class GetCancelServiceSalesArrangementsIdsRequest
    : MediatR.IRequest<GetCancelServiceSalesArrangementsIdsResponse>
{ }

public partial class GetOfferGuaranteeDateToCheckRequest
    : MediatR.IRequest<GetOfferGuaranteeDateToCheckResponse>
{ }

public partial class CancelNotFinishedExtraPaymentsRequest
    : MediatR.IRequest<Empty>
{ }