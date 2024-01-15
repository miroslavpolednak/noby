using SharedTypes.Enums;
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
        var (entity, realEstateIds, attachments, caseInstance, _) = await _aggregate.GetAggregatedData(request.RealEstateValuationId, cancellationToken);

        // detail oceneni
        var houseAndFlat = await _aggregate.GetHouseAndFlat(request.RealEstateValuationId, cancellationToken);
        // instance uzivatele
        var currentUser = await _userService.GetCurrentUser(cancellationToken);
        // info o produktu
        var productProps = await _aggregate.GetProductProperties(caseInstance.State, caseInstance.CaseId, cancellationToken);
        // klient
        var customer = await _customerService.GetCustomerDetail(caseInstance.Customer.Identity, cancellationToken);

        var orderRequest = new ExternalServices.PreorderService.V1.Contracts.StandardOrderRequestDTO
        {
            LocalSurveyPerson = $"{request.Data?.FirstName} {request.Data?.LastName}",
            LocalSurveyEmail = request.Data?.Email,
            LocalSurveyPhone = $"{request.Data?.PhoneIDC}{request.Data?.PhoneNumber}",
            LocalSurveyFunction = request.Data?.RealEstateValuationLocalSurveyFunctionCode,
            LocalSurveyAttachments = new ExternalServices.PreorderService.V1.Contracts.LocalSurveyAttachmentsDTO()
        };
        orderRequest.FillBaseOrderData(caseInstance, customer, currentUser, realEstateIds, attachments);
        orderRequest.FillBaseStandardOrderData(currentUser, entity, houseAndFlat, in productProps);
        
        entity.ValuationTypeId = (int)RealEstateValuationTypes.Standard;
        var orderResponse = await _preorderService.CreateOrder(orderRequest, cancellationToken);

        // ulozeni vysledku
        await _aggregate.SaveResults(entity, orderResponse.OrderId, RealEstateValuationStates.Probiha, request.Data, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Services.OrderAggregate _aggregate;
    private readonly IPreorderServiceClient _preorderService;
    private readonly IUserServiceClient _userService;
    private readonly ICustomerServiceClient _customerService;

    public OrderStandardValuationHandler(
        ICustomerServiceClient customerService,
        Services.OrderAggregate aggregate,
        IPreorderServiceClient preorderService,
        IUserServiceClient userService)
    {
        _customerService = customerService;
        _aggregate = aggregate;
        _preorderService = preorderService;
        _userService = userService;
    }
}
