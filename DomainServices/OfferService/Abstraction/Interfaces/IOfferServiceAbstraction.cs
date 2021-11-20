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
    Task<IServiceCallResult> SimulateBuildingSavings(SimulateBuildingSavingsRequest request);

    /// <summary>
    /// Detail simulace stavebniho sporeni
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetBuildingSavingsDataResponse] - OK
    /// </returns>
    Task<IServiceCallResult> GetBuildingSavingsData(int offerInstanceId);

    /// <summary>
    /// Stavebni sporeni - splatkovy kalendar sporeni
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetBuildingSavingsScheduleResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetBuildingSavingsDepositSchedule(int offerInstanceId);

    /// <summary>
    /// Stavebni sporeni - splatkovy kalendar uveru
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetBuildingSavingsScheduleResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetBuildingSavingsPaymentSchedule(int offerInstanceId);

    /// <summary>
    /// Stavebni sporeni - tisk nabidky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[PrintBuildingSavingsOfferResponse] - OK
    /// </returns>
    Task<IServiceCallResult> PrintBuildingSavingsOffer(PrintBuildingSavingsOfferRequest request);
}
