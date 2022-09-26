using CIS.Core.Results;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Abstraction;

public interface IProductServiceAbstraction
{
    //Task<IServiceCallResult> CreateProductInstance(long caseId, int ProductInstanceTypeId, CancellationToken cancellationToken = default(CancellationToken));
    //Task<IServiceCallResult> GetHousingSavingsInstance(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken));
    //Task<IServiceCallResult> GetHousingSavingsInstanceBasicDetail(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken));
    //Task<IServiceCallResult> GetProductInstanceList(long caseId, CancellationToken cancellationToken = default(CancellationToken));
    //Task<IServiceCallResult> UpdateHousingSavingsInstance(long productInstanceId, CancellationToken cancellationToken = default(CancellationToken));


    /// <summary>
    /// Seznam produktů dle ID obchodního případu
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetProductListResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetProductList(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Detail produktu KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetMortgageResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetMortgage(long productId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vytvoření produktu KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Contracts.ProductIdReqRes] - OK;
    /// SimulationServiceErrorResult - chyba z EAS;
    /// ErrorServiceCallResult - chyba pri request kontrole;
    /// </returns>
    Task<IServiceCallResult> CreateMortgage(CreateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Aktualizace produktu KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Google.Protobuf.WellKnownTypes.Empty] - OK;
    /// SimulationServiceErrorResult - chyba z EAS;
    /// ErrorServiceCallResult - chyba pri request kontrole;
    /// </returns>
    Task<IServiceCallResult> UpdateMorgage(UpdateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vytvoření vazby customer/product
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Google.Protobuf.WellKnownTypes.Empty] - OK;
    /// SimulationServiceErrorResult - chyba z EAS;
    /// ErrorServiceCallResult - chyba pri request kontrole;
    /// </returns>
    Task<IServiceCallResult> CreateContractRelationship(CreateContractRelationshipRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Odstranění vazby customer/product
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Google.Protobuf.WellKnownTypes.Empty] - OK;
    /// SimulationServiceErrorResult - chyba z EAS;
    /// ErrorServiceCallResult - chyba pri request kontrole;
    /// </returns>
    Task<IServiceCallResult> DeleteContractRelationship(DeleteContractRelationshipRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Seznam klientu k produktu z KonsDB
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetCustomersOnProductResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetCustomersOnProduct(long productId, CancellationToken cancellationToken = default(CancellationToken));
}
