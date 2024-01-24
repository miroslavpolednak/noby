using DomainServices.RealEstateValuationService.Api.Extensions;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using DomainServices.UserService.Clients;
using DomainServices.CustomerService.Clients;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderStandardValuation;

internal sealed class OrderStandardValuationHandler
    : IRequestHandler<OrderStandardValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(OrderStandardValuationRequest request, CancellationToken cancellationToken)
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

        var orderRequest = new ExternalServices.PreorderService.V1.Contracts.StandardOrderRequestDTO
        {
            LocalSurveyPerson = $"{request.LocalSurveyDetails?.FirstName} {request.LocalSurveyDetails?.LastName}",
            LocalSurveyEmail = request.LocalSurveyDetails?.Email,
            LocalSurveyPhone = $"{request.LocalSurveyDetails?.PhoneIDC}{request.LocalSurveyDetails?.PhoneNumber}",
            LocalSurveyFunction = request.LocalSurveyDetails?.RealEstateValuationLocalSurveyFunctionCode,
            LocalSurveyAttachments = new ExternalServices.PreorderService.V1.Contracts.LocalSurveyAttachmentsDTO()
        };
        orderRequest.FillBaseOrderData(caseInstance, customer, currentUser, realEstateIds, attachments);
        orderRequest.FillBaseStandardOrderData(currentUser, entity, houseAndFlat, in productProps);
        
        entity.ValuationTypeId = (int)RealEstateValuationTypes.Standard;
        var orderResponse = await _preorderService.CreateOrder(orderRequest, cancellationToken);

        // ulozeni vysledku
        await _aggregate.SaveResultsAndUpdateEntity(entity, orderResponse.OrderId, RealEstateValuationStates.Probiha, cancellationToken);

        // ulozeni order detail
        await _aggregate.UpdateLocalSurveyDetailsOnly(request.RealEstateValuationId, request.LocalSurveyDetails, revDetailData, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper _mapper;
    private readonly Services.OrderAggregate _aggregate;
    private readonly IPreorderServiceClient _preorderService;
    private readonly IUserServiceClient _userService;
    private readonly ICustomerServiceClient _customerService;

    public OrderStandardValuationHandler(
        Database.DocumentDataEntities.Mappers.RealEstateValuationDataMapper mapper,
        ICustomerServiceClient customerService,
        Services.OrderAggregate aggregate,
        IPreorderServiceClient preorderService,
        IUserServiceClient userService)
    {
        _mapper = mapper;
        _customerService = customerService;
        _aggregate = aggregate;
        _preorderService = preorderService;
        _userService = userService;
    }
}
