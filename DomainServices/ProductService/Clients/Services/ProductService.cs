using Microsoft.Extensions.Logging;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Clients.Services;

internal class ProductService : IProductServiceClient
{

    #region Construction

    private readonly ILogger<ProductService> _logger;
    private readonly Contracts.v1.ProductService.ProductServiceClient _service;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public ProductService(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<ProductService> logger,
        Contracts.v1.ProductService.ProductServiceClient service)
    {
        _userAccessor = userAccessor;
        _service = service;
        _logger = logger;
    }

    #endregion

    public async Task<GetProductListResponse> GetProductList(long caseId, CancellationToken cancellationToken = default)
    {
        return await _service.GetProductListAsync(new CaseIdRequest() { CaseId = caseId }, cancellationToken: cancellationToken);
    }

    public async Task<GetProductObligationListResponse> GetProductObligationList(GetProductObligationListRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.GetProductObligationListAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<GetMortgageResponse> GetMortgage(long productId, CancellationToken cancellationToken = default)
    {
        return await _service.GetMortgageAsync(new ProductIdReqRes() { ProductId = productId }, cancellationToken: cancellationToken);
    }

    public async Task<ProductIdReqRes> CreateMortgage(CreateMortgageRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.CreateMortgageAsync(request, cancellationToken: cancellationToken);
    }
    
    public async Task UpdateMortgage(UpdateMortgageRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateMortgageAsync(request, cancellationToken: cancellationToken);
    }

    public async Task CreateContractRelationship(CreateContractRelationshipRequest request, CancellationToken cancellationToken = default)
    {
        await _service.CreateContractRelationshipAsync(request, cancellationToken: cancellationToken);
    }

    public async Task DeleteContractRelationship(DeleteContractRelationshipRequest request, CancellationToken cancellationToken = default)
    {
        await _service.DeleteContractRelationshipAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<GetCustomersOnProductResponse> GetCustomersOnProduct(long productId, CancellationToken cancellationToken = default)
    {
        return await _service.GetCustomersOnProductAsync(new ProductIdReqRes() { ProductId = productId }, cancellationToken: cancellationToken);
    }

}
