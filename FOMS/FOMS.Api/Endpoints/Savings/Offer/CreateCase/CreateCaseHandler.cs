using CIS.Core.Results;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class CreateCaseHandler
    : IRequestHandler<CreateCaseRequest, SaveCaseResponse>
{
    public async Task<SaveCaseResponse> Handle(CreateCaseRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create building savings case for {offerInstanceId}", request.OfferInstanceId);

        // ziskat data o klientovi
        //TODO asi neexistuje metoda, ktera by vracela jen zakladni potrebne udaje?
        var customerRequest = new DomainServices.CustomerService.Contracts.GetDetailRequest { Identity = request.Customer?.Id ?? 0 };
        var customer = ServiceCallResult.Resolve<DomainServices.CustomerService.Contracts.GetDetailResponse>(await _customerService.GetDetail(customerRequest));
        _logger.LogDebug("Customer {customer} found", request.Customer);

        // detail simulace
        var offerInstance = ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.GetBuildingSavingsDataResponse>(await _offerService.GetBuildingSavingsData(request.OfferInstanceId, cancellationToken));

        // vytvorit case
        long caseId = await _mediator.Send(new SharedHandlers.Requests.SharedCreateCaseRequest
        {
            OfferInstanceId = request.OfferInstanceId,
            DateOfBirth = customer.DateOfBirth,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Customer = request.Customer,
            ProductInstanceType = _configuration.BuildingSavings.SavingsProductInstanceType,
            TargetAmount = offerInstance.InputData.TargetAmount
        }, cancellationToken);

        // vytvorit zadost
        int salesArrangementId = await _mediator.Send(new SharedHandlers.Requests.SharedCreateSalesArrangementRequest
        {
            CaseId = caseId,
            OfferInstanceId = request.OfferInstanceId,
            ProductInstanceType = _configuration.BuildingSavings.SavingsSalesArrangementType
        }, cancellationToken);

        // vytvorit produkt
        long productId = await _mediator.Send(new SharedHandlers.Requests.SharedCreateProductInstanceRequest()
        {
            CaseId = caseId,
            ProductInstanceType = _configuration.BuildingSavings?.SavingsProductInstanceType ?? 0
        }, cancellationToken);
    
        return new SaveCaseResponse
        {
            SalesArrangementId = salesArrangementId,
            CaseId = caseId
        };
    }

    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly IMediator _mediator;
    private readonly Infrastructure.Configuration.AppConfiguration _configuration;

    public CreateCaseHandler(
        ILogger<CreateCaseHandler> logger,
        IMediator mediator,
        Infrastructure.Configuration.AppConfiguration configuration,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService)
    {
        _offerService = offerService;
        _configuration = configuration;
        _customerService = customerService;
        _logger = logger;
        _mediator = mediator;
    }
}
