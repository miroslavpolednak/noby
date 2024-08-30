using DomainServices.RealEstateValuationService.Api.Extensions;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using DomainServices.UserService.Clients.v1;
using DomainServices.CustomerService.Clients.v1;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.OrderDTSValuation;

internal sealed class OrderDTSValuationHandler(
    Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper _mapper,
    ICustomerServiceClient _customerService,
    Services.OrderAggregate _aggregate,
    IPreorderServiceClient _preorderService,
    IUserServiceClient _userService)
        : IRequestHandler<OrderDTSValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(OrderDTSValuationRequest request, CancellationToken cancellationToken)
    {
        var (entity, revDetailData, realEstateIds, attachments, caseInstance, _) = await _aggregate.GetAggregatedData(request.RealEstateValuationId, cancellationToken);

        // detail oceneni
        var houseAndFlat = _mapper.MapFromDataToSingle(revDetailData).HouseAndFlatDetails;
        // instance uzivatele
        var currentUser = await _userService.GetCurrentUser(cancellationToken);
        // info o produktu
        var productProps = await _aggregate.GetProductProperties(caseInstance.State, caseInstance.CaseId, cancellationToken);
        // klient
        var customer = await _customerService.GetCustomerDetail(caseInstance.Customer.Identity, cancellationToken);

        long orderId = entity.RealEstateTypeId switch
        {
            1 => await getHouseResult(),
            2 => await getFlatResult(),
            _ => throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(entity.RealEstateTypeId)
        };

        // ulozeni vysledku
        entity.Comment = request.Comment;
        entity.ValuationTypeId = 2;
        await _aggregate.SaveResultsAndUpdateEntity(entity, orderId, WorkflowTaskStates.ProbihaOceneni, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();

        async Task<long> getFlatResult()
        {
            var orderRequest = new ExternalServices.PreorderService.V1.Contracts.DtsFlatRequest
            {
                SpecialRequest = request.Comment,
                LocalSurveyAttachments = new ExternalServices.PreorderService.V1.Contracts.LocalSurveyAttachmentsDTO()
            };

            orderRequest.FillBaseOrderData(caseInstance, customer, currentUser, realEstateIds, attachments);
            orderRequest.FillBaseStandardOrderData(currentUser, entity, houseAndFlat, in productProps);

            return (await _preorderService.CreateOrder(orderRequest, cancellationToken)).OrderId;
        }

        async Task<long> getHouseResult()
        {
            var orderRequest = new ExternalServices.PreorderService.V1.Contracts.DtsHouseRequest
            {
                SpecialRequest = request.Comment,
                LocalSurveyAttachments = new ExternalServices.PreorderService.V1.Contracts.LocalSurveyAttachmentsDTO()
            };

            orderRequest.FillBaseOrderData(caseInstance, customer, currentUser, realEstateIds, attachments);
            orderRequest.FillBaseStandardOrderData(currentUser, entity, houseAndFlat, in productProps);

            return (await _preorderService.CreateOrder(orderRequest, cancellationToken)).OrderId;
        }
    }
}
