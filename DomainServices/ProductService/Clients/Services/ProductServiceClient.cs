﻿using Microsoft.Extensions.Logging;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Clients.Services;

internal sealed class ProductServiceClient : IProductServiceClient
{

    #region Construction

    private readonly ILogger<ProductServiceClient> _logger;
    private readonly Contracts.v1.ProductService.ProductServiceClient _service;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public ProductServiceClient(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<ProductServiceClient> logger,
        Contracts.v1.ProductService.ProductServiceClient service)
    {
        _userAccessor = userAccessor;
        _service = service;
        _logger = logger;
    }

    #endregion

    public async Task<GetProductListResponse> GetProductList(long caseId, CancellationToken cancellationToken = default)
    {
        return await _service.GetProductListAsync(new GetProductListRequest { CaseId = caseId }, cancellationToken: cancellationToken);
    }

    public async Task<GetProductObligationListResponse> GetProductObligationList(GetProductObligationListRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.GetProductObligationListAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<GetMortgageResponse> GetMortgage(long productId, CancellationToken cancellationToken = default)
    {
        return await _service.GetMortgageAsync(new GetMortgageRequest { ProductId = productId }, cancellationToken: cancellationToken);
    }

    public async Task<long> CreateMortgage(CreateMortgageRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateMortgageAsync(request, cancellationToken: cancellationToken);
        return result.ProductId;
    }
    
    public async Task UpdateMortgage(UpdateMortgageRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateMortgageAsync(request, cancellationToken: cancellationToken);
    }

    public async Task CreateContractRelationship(long partnerId, long productId, int contractRelationshipTypeId, CancellationToken cancellationToken = default)
    {
        await _service.CreateContractRelationshipAsync(new CreateContractRelationshipRequest
        {
            ProductId = productId,
            Relationship = new Relationship
            {
                ContractRelationshipTypeId = contractRelationshipTypeId,
                PartnerId = partnerId
            }
        }, cancellationToken: cancellationToken);
    }

    public async Task DeleteContractRelationship(long partnerId, long productId, CancellationToken cancellationToken = default)
    {
        await _service.DeleteContractRelationshipAsync(new DeleteContractRelationshipRequest
        {
            PartnerId = partnerId,
            ProductId = productId,
        }, cancellationToken: cancellationToken);
    }

    public async Task<GetCustomersOnProductResponse> GetCustomersOnProduct(long productId, CancellationToken cancellationToken = default)
    {
        return await _service.GetCustomersOnProductAsync(new GetCustomersOnProductRequest { ProductId = productId }, cancellationToken: cancellationToken);
    }

    public async Task<GetCaseIdResponse> GetCaseId(GetCaseIdRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.GetCaseIdAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<GetCovenantDetailResponse> GetCovenantDetail(long caseId, int order, CancellationToken cancellationToken = default)
    {
        return await _service.GetCovenantDetailAsync(new GetCovenantDetailRequest { CaseId = caseId, Order = order }, cancellationToken: cancellationToken);
    }

    public async Task<GetCovenantListResponse> GetCovenantList(long caseId, CancellationToken cancellationToken = default)
    {
        return await _service.GetCovenantListAsync(new GetCovenantListRequest { CaseId = caseId }, cancellationToken: cancellationToken);
    }

    public async Task CancelMortgage(long caseId, CancellationToken cancellationToken = default)
    {
        await _service.CancelMortgageAsync(new CancelMortgageRequest { ProductId = caseId }, cancellationToken: cancellationToken);
    }
}
