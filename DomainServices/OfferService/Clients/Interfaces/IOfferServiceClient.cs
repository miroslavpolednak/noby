using CIS.Core.Results;
using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Clients;

public interface IOfferServiceClient
{

    /// <summary>
    /// Základní data simulace (bez inputs a outputs) 
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetOfferResponse] - OK
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10000; Offer #{offerId} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 10001; OfferId is not specified</exception>
    Task<IServiceCallResult> GetOffer(int offerId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Získání základních informací o simulaci KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetMortgageOfferResponse] - OK
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10000; Offer #{offerId} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 10001; OfferId is not specified</exception>
    Task<IServiceCallResult> GetMortgageOffer(int offerId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Získání detailních informací o simulaci KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetMortgageOfferDetailResponse] - OK
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10000; Offer #{offerId} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 10001; OfferId is not specified</exception>
    Task<IServiceCallResult> GetMortgageOfferDetail(int offerId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Simulace KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Contracts.SimulateMortgageResponse] - OK;
    /// SimulationServiceErrorResult - chyba z EAS;
    /// ErrorServiceCallResult - chyba pri request kontrole;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10002; Default 'PaymentDay' not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10003; ResourceProcessId is missing or is in invalid format</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10004; SimulationInputs are not specified</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10005; SimulationInputs.ProductTypeId is not specified</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10006; SimulationInputs.LoanKindId is not specified</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10007; SimulationInputs.GuaranteeDateFrom can't be older then {AppDefaults.MaxGuaranteeInDays} days</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10008; SimulationInputs.MarketingActions are not specified</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10009; SimulationInputs.FixedRatePeriod is not specified</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10010; SimulationInputs.LoanAmount is not specified</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10011; SimulationInputs.LoanDuration is not specified</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10018; SimulationInputs.CollateralAmount is not specified</exception>
    Task<IServiceCallResult> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Získání plného splátkového kalendáře KB Hypotéky
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetMortgageOfferFPScheduleResponse] - OK
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10000; Offer #{offerId} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 10001; OfferId is not specified</exception>
    Task<IServiceCallResult> GetMortgageOfferFPSchedule(int offerId, CancellationToken cancellationToken = default(CancellationToken));

}
