using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer;

internal class CreateCaseHandler
    : IRequestHandler<CreateCaseRequest, int>
{
    public async Task<int> Handle(CreateCaseRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create case for {offerInstanceId}", request.OfferInstanceId);

        // vytvorit case
        long caseId = await createCase(request);
        _logger.LogDebug("Case #{caseId} created", caseId);

        // vytvorit zadost
        int salesArrangementId = resolveSalesArrangementResult(await _salesArrangementService.CreateSalesArrangement(caseId, _configuration.Savings.SavingsSalesArrangementType, offerInstanceId: request.OfferInstanceId));
        _logger.LogDebug("Sales arrangement #{salesArrangementId} created", salesArrangementId);

        // vytvorit produkt, pokud se jedna o finalni simulaci
        if (request.CreateProduct)
        {
            var productId = resolveProductResult(await _productService.CreateProductInstance(caseId, _configuration.Savings.SavingsProductInstanceType));
            _logger.LogDebug("Product #{productInstanceId} created", productId);
        }

        return salesArrangementId;
    }

    private int resolveProductResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<int> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            _ => throw new NotImplementedException()
        };

    private int resolveSalesArrangementResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<int> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            _ => throw new NotImplementedException()
        };

    private async Task<long> createCase(CreateCaseRequest request)
    {
        var caseModel = new DomainServices.CaseService.Contracts.CreateCaseRequest()
        {
            UserId = _userAccessor.User.Id,
            ProductInstanceType = _configuration.Savings.SavingsProductInstanceType,
            DateOfBirthNaturalPerson = request.Request.DateOfBirth,
            FirstNameNaturalPerson = request.Request.FirstName,
            Name = request.Request.LastName
        };
        if (request.Customer is not null)
            caseModel.Customer = new CIS.Infrastructure.gRPC.CisTypes.Identity(request.Customer);

        _logger.LogInformation("Create case with {model}", caseModel);

        return resolveCaseResult(await _caseService.CreateCase(caseModel));
    }
    
    private long resolveCaseResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            ErrorServiceCallResult e2 => throw new CIS.Core.Exceptions.CisValidationException(e2.Errors),
            _ => throw new NotImplementedException()
        };

    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly Infrastructure.Configuration.AppConfiguration _configuration;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public CreateCaseHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<CreateCaseHandler> logger,
        Infrastructure.Configuration.AppConfiguration configuration,
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService, 
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _productService = productService;
        _userAccessor = userAccessor;
        _logger = logger;
        _configuration = configuration;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
