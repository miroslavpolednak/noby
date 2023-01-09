﻿using CIS.Core.Results;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Clients;

public interface IProductServiceClient
{
    //Task<IServiceCallResult> CreateProductInstance(long caseId, int ProductInstanceTypeId, CancellationToken cancellationToken = default(CancellationToken));
    //Task<IServiceCallResult> GetHousingSavingsInstance(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken));
    //Task<IServiceCallResult> GetHousingSavingsInstanceBasicDetail(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken));
    //Task<IServiceCallResult> GetProductInstanceList(long caseId, CancellationToken cancellationToken = default(CancellationToken));
    //Task<IServiceCallResult> UpdateHousingSavingsInstance(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken));


    /// <summary>
    /// Seznam produktů dle ID obchodního případu
    /// </summary>
    Task<GetProductListResponse> GetProductList(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// todo:
    /// </summary>
    Task<GetProductObligationListResponse> GetProductObligationList(GetProductObligationListRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Detail produktu KB Hypotéky
    /// </summary>
    Task<GetMortgageResponse> GetMortgage(long productId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vytvoření produktu KB Hypotéky
    /// </summary>
    Task<ProductIdReqRes> CreateMortgage(CreateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Aktualizace produktu KB Hypotéky
    /// </summary>
    Task UpdateMortgage(UpdateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vytvoření vazby customer/product
    /// </summary>
    Task CreateContractRelationship(CreateContractRelationshipRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Odstranění vazby customer/product
    /// </summary>
    Task DeleteContractRelationship(DeleteContractRelationshipRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Seznam klientu k produktu z KonsDB
    /// </summary>
    Task<GetCustomersOnProductResponse> GetCustomersOnProduct(long productId, CancellationToken cancellationToken = default(CancellationToken));
}
