using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Clients;

public interface IProductServiceClient
{
    /// <summary>
    /// Seznam produktů dle ID obchodního případu
    /// </summary>
    Task<GetProductListResponse> GetProductList(long caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// todo:
    /// </summary>
    Task<GetProductObligationListResponse> GetProductObligationList(GetProductObligationListRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Detail produktu KB Hypotéky
    /// </summary>
    Task<GetMortgageResponse> GetMortgage(long productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vytvoření produktu KB Hypotéky
    /// </summary>
    /// <returns>ProductId</returns>
    Task<long> CreateMortgage(CreateMortgageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aktualizace produktu KB Hypotéky
    /// </summary>
    Task UpdateMortgage(UpdateMortgageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vytvoření vazby customer/product
    /// </summary>
    Task CreateContractRelationship(long partnerId, long productId, int contractRelationshipTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Odstranění vazby customer/product
    /// </summary>
    Task DeleteContractRelationship(long partnerId, long productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Seznam klientu k produktu z KonsDB
    /// </summary>
    Task<GetCustomersOnProductResponse> GetCustomersOnProduct(long productId, CancellationToken cancellationToken = default);

    Task<GetCaseIdResponse> GetCaseId(GetCaseIdRequest request, CancellationToken cancellationToken = default);

    Task<GetCovenantDetailResponse> GetCovenantDetail(long caseId, int order, CancellationToken cancellationToken = default);
    
    Task<GetCovenantListResponse> GetCovenantList(long caseId, CancellationToken cancellationToken = default);
}
