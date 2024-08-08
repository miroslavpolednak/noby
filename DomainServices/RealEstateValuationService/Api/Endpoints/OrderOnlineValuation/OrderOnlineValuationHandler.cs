using DomainServices.CustomerService.Clients;
using DomainServices.RealEstateValuationService.Api.Extensions;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using DomainServices.UserService.Clients;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderOnlineValuation;

internal sealed class OrderOnlineValuationHandler(
    Services.OrderAggregate _aggregate,
    IPreorderServiceClient _preorderService,
    IUserServiceClient _userService,
    ICustomerServiceClient _customerService)
        : IRequestHandler<OrderOnlineValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(OrderOnlineValuationRequest request, CancellationToken cancellationToken)
    {
        var (entity, revDetailData, realEstateIds, attachments, caseInstance, _) = await _aggregate.GetAggregatedData(request.RealEstateValuationId, cancellationToken);

        // klient
        var customer = await _customerService.GetCustomerDetail(caseInstance.Customer.Identity, cancellationToken);
        // instance uzivatele
        var currentUser = await _userService.GetCurrentUser(cancellationToken);

        var orderRequest = new ExternalServices.PreorderService.V1.Contracts.OnlineMPRequestDTO
        {
            ValuationRequestId = entity.PreorderId.GetValueOrDefault(),
            LocalSurveyPerson = $"{request.LocalSurveyDetails?.FirstName} {request.LocalSurveyDetails?.LastName}",
            LocalSurveyEmail = request.LocalSurveyDetails?.Email,
            LocalSurveyPhone = $"{request.LocalSurveyDetails?.PhoneIDC}{request.LocalSurveyDetails?.PhoneNumber}",
            LocalSurveyFunction = request.LocalSurveyDetails?.RealEstateValuationLocalSurveyFunctionCode
        };
        orderRequest.FillBaseOrderData(caseInstance, customer, currentUser, realEstateIds, attachments);
        
        var orderResponse = await _preorderService.CreateOrder(orderRequest, cancellationToken);

        // ulozeni vysledku
        await _aggregate.SaveResultsAndUpdateEntity(entity, orderResponse.OrderId, RealEstateValuationStates.Dokonceno, cancellationToken);

        // ulozeni detailu objednavky
        await _aggregate.UpdateLocalSurveyDetailsOnly(request.RealEstateValuationId, entity.IsRevaluationRequired ? request.LocalSurveyDetails : null, revDetailData, cancellationToken);
    
        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
