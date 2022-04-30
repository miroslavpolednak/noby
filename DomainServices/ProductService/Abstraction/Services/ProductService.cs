using CIS.Core.Results;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;
using DomainServices.ProductService.Contracts;
using CIS.DomainServicesSecurity.Abstraction;

namespace DomainServices.ProductService.Abstraction.Services;

internal class ProductService : IProductServiceAbstraction
{

    #region Construction

    private readonly ILogger<ProductService> _logger;
    private readonly Contracts.v1.ProductService.ProductServiceClient _service;
    private readonly ICisUserContextHelpers _userContext;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public ProductService(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<ProductService> logger,
        Contracts.v1.ProductService.ProductServiceClient service,
        ICisUserContextHelpers userContext)
    {
        _userAccessor = userAccessor;
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }

    #endregion

    public async Task<IServiceCallResult> GetProductList(int caseId, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetProductList), caseId);

        var result = await _userContext.AddUserContext(async () => await _service.GetProductListAsync(new CaseIdRequest() { CaseId = caseId }, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<GetProductListResponse>(result);
    }

    public async Task<IServiceCallResult> GetMortgage(int productId, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetMortgage), productId);

        var result = await _userContext.AddUserContext(async () => await _service.GetMortgageAsync(new ProductIdReqRes() { ProductId = productId }, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<GetMortgageResponse>(result);
    }

    public async Task<IServiceCallResult> CreateMortgage(CreateMortgageRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(CreateMortgage));

        var result = await _userContext.AddUserContext(async () => await _service.CreateMortgageAsync(request, cancellationToken: cancellationToken));

        _logger.LogDebug("Abstraction CreateMortgage saved as #{id}", result.ProductId);

        return new SuccessfulServiceCallResult<ProductIdReqRes>(result);
    }

    public async Task<IServiceCallResult> UpdateMorgage(UpdateMortgageRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(UpdateMorgage));

        var result = await _userContext.AddUserContext(async () => await _service.UpdateMorgageAsync(request, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> CreateContractRelationship(CreateContractRelationshipRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(CreateContractRelationship));

        var result = await _userContext.AddUserContext(async () => await _service.CreateContractRelationshipAsync(request, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> DeleteContractRelationship(DeleteContractRelationshipRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(DeleteContractRelationship));

        var result = await _userContext.AddUserContext(async () => await _service.DeleteContractRelationshipAsync(request, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult();
    }

}
