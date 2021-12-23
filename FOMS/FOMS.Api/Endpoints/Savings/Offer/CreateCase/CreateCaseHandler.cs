using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class CreateCaseHandler
    : BaseCaseHandler, IRequestHandler<CreateCaseRequest, int>
{
    public async Task<int> Handle(CreateCaseRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create case for {offerInstanceId}", request.OfferInstanceId);

        // vytvorit case
        long caseId = await createCase(request.OfferInstanceId, request.Request.FirstName, request.Request.LastName, request.Request.DateOfBirth, request.Customer);
        _logger.LogDebug("Case #{caseId} created", caseId);

        // vytvorit zadost
        int salesArrangementId = await createSalesArrangement(caseId, request.OfferInstanceId);
        _logger.LogDebug("Sales arrangement #{salesArrangementId} created", salesArrangementId);

        // vytvorit produkt
        var productId = resolveProductResult(await _productService.CreateProductInstance(caseId, _aggregate.Configuration.BuildingSavings?.SavingsProductInstanceType ?? 0));
        _logger.LogDebug("Product #{productInstanceId} created", productId);
    
        return salesArrangementId;
    }

    private int resolveProductResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<int> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            _ => throw new NotImplementedException()
        };

    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private readonly ILogger<CreateCaseHandler> _logger;

    public CreateCaseHandler(
        ILogger<CreateCaseHandler> logger,
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService,
        BaseCaseHandlerAggregate aggregate)
        : base(aggregate)
    {
        _productService = productService;
        _logger = logger;
    }
}
