using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class CreateCaseHandler
    : BaseCaseHandler, IRequestHandler<CreateCaseRequest, SaveCaseResponse>
{
    public async Task<SaveCaseResponse> Handle(CreateCaseRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create case for {offerInstanceId}", request.OfferInstanceId);

        // ziskat data o klientovi
        //TODO asi neexistuje metoda, ktera by vracela jen zakladni potrebne udaje?
        var customer = resolveCustomerResult(await _customerService.GetDetail(new DomainServices.CustomerService.Contracts.GetDetailRequest {  Identity = request.Customer?.Id ?? 0 }));
        _logger.LogDebug("Customer {customer} found", request.Customer);

        // vytvorit case
        long caseId = await createCase(request.OfferInstanceId, customer.FirstName, customer.LastName, customer.DateOfBirth, request.Customer);
        _logger.LogDebug("Case #{caseId} created", caseId);

        // vytvorit zadost
        int salesArrangementId = await createSalesArrangement(caseId, request.OfferInstanceId);
        _logger.LogDebug("Sales arrangement #{salesArrangementId} created", salesArrangementId);

        // vytvorit produkt
        var productId = resolveProductResult(await _productService.CreateProductInstance(caseId, _aggregate.Configuration.BuildingSavings?.SavingsProductInstanceType ?? 0));
        _logger.LogDebug("Product #{productInstanceId} created", productId);
    
        return new SaveCaseResponse
        {
            SalesArrangementId = salesArrangementId,
            CaseId = caseId
        };
    }

    private DomainServices.CustomerService.Contracts.GetDetailResponse resolveCustomerResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.CustomerService.Contracts.GetDetailResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };

    private int resolveProductResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<int> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            _ => throw new NotImplementedException()
        };

    private DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;
    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private readonly ILogger<CreateCaseHandler> _logger;

    public CreateCaseHandler(
        ILogger<CreateCaseHandler> logger,
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService,
        BaseCaseHandlerAggregate aggregate)
        : base(aggregate)
    {
        _customerService = customerService;
        _productService = productService;
        _logger = logger;
    }
}
