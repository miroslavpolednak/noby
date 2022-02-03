using CIS.Core.Results;
using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Abstraction;

public interface IOfferServiceAbstraction
{
    /// <summary>
    /// Simulace stavebniho sporeni
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Contracts.SimulateHousingsSavingsResponse] - OK;
    /// SimulationServiceErrorResult - chyba z EAS;
    /// ErrorServiceCallResult - chyba pri request kontrole;
    /// </returns>
    Task<IServiceCallResult> SimulateBuildingSavings(SimulateBuildingSavingsRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Detail simulace stavebniho sporeni
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetBuildingSavingsDataResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetBuildingSavingsData(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Stavebni sporeni - splatkovy kalendar sporeni
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetBuildingSavingsScheduleResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetBuildingSavingsDepositSchedule(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Stavebni sporeni - splatkovy kalendar uveru
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetBuildingSavingsScheduleResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetBuildingSavingsPaymentSchedule(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Stavebni sporeni - tisk nabidky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[PrintBuildingSavingsOfferResponse] - OK
    /// </returns>
    Task<IServiceCallResult> PrintBuildingSavingsOffer(PrintBuildingSavingsOfferRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Simulace KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Contracts.SimulateMortgageResponse] - OK;
    /// SimulationServiceErrorResult - chyba z EAS;
    /// ErrorServiceCallResult - chyba pri request kontrole;
    /// </returns>
    Task<IServiceCallResult> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Detail simulace KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetMortgageDataResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetMortgageData(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));
}
