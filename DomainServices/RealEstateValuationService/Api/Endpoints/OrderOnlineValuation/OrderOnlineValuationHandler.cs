using CIS.Foms.Enums;
using DomainServices.CustomerService.Clients;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using DomainServices.UserService.Clients;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderOnlineValuation;

internal sealed class OrderOnlineValuationHandler
    : IRequestHandler<OrderOnlineValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(OrderOnlineValuationRequest request, CancellationToken cancellationToken)
    {
        var (entity, realEstateIds, attachments, caseInstance, _) = await _aggregate.GetAggregatedData(request.RealEstateValuationId, cancellationToken);
        // klient
        var customer = await _customerService.GetCustomerDetail(caseInstance.Customer.Identity, cancellationToken);
        // instance uzivatele
        var currentUser = await _userService.GetCurrentUser(cancellationToken);

        var orderRequest = new ExternalServices.PreorderService.V1.Contracts.OnlineMPRequestDTO
        {
            ValuationRequestId = entity.PreorderId.GetValueOrDefault(),
            DealNumber = caseInstance.Data.ContractNumber,
            ClientName = $"{customer.NaturalPerson?.FirstName} {customer.NaturalPerson?.LastName}",
            ClientEmail = customer.Contacts?.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Email)?.Email?.EmailAddress,
            CremRealEstateIds = realEstateIds,
            AttachmentIds = attachments,
            EFormId = 0,
            CompanyCode = "02",
            ProductCode = "02",
            Cpm = Convert.ToInt64(currentUser.UserInfo.Cpm, CultureInfo.InvariantCulture),// nez to v ACV opravi
            Icp = Convert.ToInt64(currentUser.UserInfo.Icp, CultureInfo.InvariantCulture)
        };

        var orderResponse = await _preorderService.OrderOnline(orderRequest, cancellationToken);

        // ulozeni vysledku
        await _aggregate.SaveResults(entity, orderResponse.OrderId, request.Data, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Services.OrderAggregate _aggregate;
    private readonly IPreorderServiceClient _preorderService;
    private readonly IUserServiceClient _userService;
    private readonly ICustomerServiceClient _customerService;

    public OrderOnlineValuationHandler(
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
