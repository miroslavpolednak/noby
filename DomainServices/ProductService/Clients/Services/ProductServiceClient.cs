﻿using DomainServices.ProductService.Contracts;
using SharedTypes.GrpcTypes;

namespace DomainServices.ProductService.Clients.Services;

internal sealed class ProductServiceClient(Contracts.v1.ProductService.ProductServiceClient _service) 
    : IProductServiceClient
{
    public async Task<List<SearchProductsResponse.Types.SearchProductsItem>> SearchProducts(Identity? identity, CancellationToken cancellationToken = default)
        => (await _service.SearchProductsAsync(new SearchProductsRequest
        {
            Identity = identity
        }, cancellationToken: cancellationToken)).Products.ToList();

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

    public async Task<CreateMortgageResponse> CreateMortgage(CreateMortgageRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.CreateMortgageAsync(request, cancellationToken: cancellationToken);
    }
    
    public async Task UpdateMortgage(long productId, CancellationToken cancellationToken = default)
    {
        await _service.UpdateMortgageAsync(new UpdateMortgageRequest
        {
            ProductId = productId
        }, cancellationToken: cancellationToken);
    }

    public async Task<string?> UpdateMortgagePcpId(UpdateMortgagePcpIdRequest request, CancellationToken cancellationToken = default)
    {
        return (await _service.UpdateMortgagePcpIdAsync(request, cancellationToken: cancellationToken)).PcpId;
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
