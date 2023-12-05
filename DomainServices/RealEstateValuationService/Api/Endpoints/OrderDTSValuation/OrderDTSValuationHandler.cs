using SharedTypes.Enums;
using DomainServices.RealEstateValuationService.Api.Extensions;
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

        // detail oceneni
        var houseAndFlat = await _aggregate.GetHouseAndFlat(request.RealEstateValuationId, cancellationToken);
        // instance uzivatele
        var currentUser = await _userService.GetCurrentUser(cancellationToken);
        // info o produktu
        var productProps = await _aggregate.GetProductProperties(caseInstance.State, caseInstance.CaseId, cancellationToken);

        long orderId = entity.RealEstateTypeId switch
        {
            1 => await getHouseResult(),
            2 => await getFlatResult(),
            _ => throw ErrorCodeMapper.CreateValidationException(entity.RealEstateTypeId)
        };

        // ulozeni vysledku
        entity.ValuationTypeId = 2;
        await _aggregate.SaveResults(entity, orderId, RealEstateValuationStates.Probiha, null, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();

        async Task<long> getFlatResult()
        {
            var orderRequest = new ExternalServices.PreorderService.V1.Contracts.DtsFlatRequest
            {
                LocalSurveyAttachments = new ExternalServices.PreorderService.V1.Contracts.LocalSurveyAttachmentsDTO()
            };

            orderRequest.FillBaseOrderData(caseInstance, currentUser, realEstateIds, attachments);
            orderRequest.FillBaseStandardOrderData(currentUser, entity, houseAndFlat, in productProps);

            return (await _preorderService.CreateOrder(orderRequest, cancellationToken)).OrderId;
        }

        async Task<long> getHouseResult()
        {
            var orderRequest = new ExternalServices.PreorderService.V1.Contracts.DtsHouseRequest
            {
                LocalSurveyAttachments = new ExternalServices.PreorderService.V1.Contracts.LocalSurveyAttachmentsDTO()
            };

            orderRequest.FillBaseOrderData(caseInstance, currentUser, realEstateIds, attachments);
            orderRequest.FillBaseStandardOrderData(currentUser, entity, houseAndFlat, in productProps);

            return (await _preorderService.CreateOrder(orderRequest, cancellationToken)).OrderId;
        }
    }

    private readonly Services.OrderAggregate _aggregate;
    private readonly IPreorderServiceClient _preorderService;
    private readonly IUserServiceClient _userService;

    public OrderDTSValuationHandler(
        Services.OrderAggregate aggregate,
        IPreorderServiceClient preorderService,
        IUserServiceClient userService)
    {
        _aggregate = aggregate;
        _preorderService = preorderService;
        _userService = userService;
    }
}
