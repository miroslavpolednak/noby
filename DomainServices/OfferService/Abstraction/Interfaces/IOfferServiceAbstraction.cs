using CIS.Core.Results;
using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Abstraction;

public interface IOfferServiceAbstraction
{

    /// <summary>
    /// Základní data simulace (bez inputs a outputs) 
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetOfferInstanceResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetOfferInstance(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Detail simulace KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetMortgageDataResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetMortgageData(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));

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
