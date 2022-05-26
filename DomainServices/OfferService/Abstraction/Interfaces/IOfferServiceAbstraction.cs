using CIS.Core.Results;
using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Abstraction;

public interface IOfferServiceAbstraction
{

    /// <summary>
    /// Základní data simulace (bez inputs a outputs) 
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetOfferResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetOffer(int offerId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Získání základních informací o simulaci KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetMortgageOfferResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetMortgageOffer(int offerId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Získání detailních informací o simulaci KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetMortgageOfferDetailResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetMortgageOfferDetail(int offerId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Simulace KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Contracts.SimulateMortgageResponse] - OK;
    /// SimulationServiceErrorResult - chyba z EAS;
    /// ErrorServiceCallResult - chyba pri request kontrole;
    /// </returns>
    Task<IServiceCallResult> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken));

}
