using DomainServices.CustomerService.Clients;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using DomainServices.UserService.Clients;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderDTSValuation;

internal sealed class OrderDTSValuationHandler
    : IRequestHandler<OrderDTSValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(OrderDTSValuationRequest request, CancellationToken cancellationToken)
    {
        var (entity, realEstateIds, attachments, caseInstance, _) = await _aggregate.GetAggregatedData(request.RealEstateValuationId, cancellationToken);


        // ulozeni vysledku
        //await _aggregate.SaveResults(entity, orderResponse.OrderId, request.Data, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Services.OrderAggregate _aggregate;
    private readonly IPreorderServiceClient _preorderService;
    private readonly IUserServiceClient _userService;
    private readonly ICustomerServiceClient _customerService;

    public OrderDTSValuationHandler(
        Services.OrderAggregate aggregate,
        IPreorderServiceClient preorderService,
        IUserServiceClient userService,
        ICustomerServiceClient customerService)
    {
        _aggregate = aggregate;
        _preorderService = preorderService;
        _userService = userService;
        _customerService = customerService;
    }
}
