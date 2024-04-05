using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Clients.v1;

public interface IOfferServiceClient
{
    /// <summary>
    /// Základní data simulace (bez inputs a outputs) 
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10000; Offer #{offerId} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 10001; OfferId is not specified</exception>
    Task<GetOfferResponse> GetOffer(int offerId, CancellationToken cancellationToken = default);

    Task<List<Contracts.GetOfferListResponse.Types.GetOfferListItem>> GetOfferList(long caseId, OfferTypes OfferType, bool ommitParametersFromResponse = false, CancellationToken cancellationToken = default);

    Task<ValidateOfferIdResponse> ValidateOfferId(int offerId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Získání detailních informací o simulaci KB Hypotéky
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10000; Offer #{offerId} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 10001; OfferId is not specified</exception>
    Task<GetMortgageDetailResponse> GetMortgageDetail(int offerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Simulace KB Hypotéky
    /// </summary>
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
    Task<SimulateMortgageResponse> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default);

    Task<SimulateMortgageRetentionResponse> SimulateMortgageRetention(SimulateMortgageRetentionRequest request, CancellationToken cancellationToken = default);

    Task<SimulateMortgageRefixationResponse> SimulateMortgageRefixation(SimulateMortgageRefixationRequest request, CancellationToken cancellationToken = default);

    Task<SimulateMortgageExtraPaymentResponse> SimulateMortgageExtraPayment(SimulateMortgageExtraPaymentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Získání plného splátkového kalendáře KB Hypotéky
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 10000; Offer #{offerId} not found</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 10001; OfferId is not specified</exception>
    Task<GetMortgageOfferFPScheduleResponse> GetMortgageOfferFPSchedule(int offerId, CancellationToken cancellationToken = default);

    Task<GetOfferDeveloperResponse> GetOfferDeveloper(int offerId, CancellationToken cancellationToken  = default);

    Task UpdateOffer(UpdateOfferRequest request, CancellationToken cancellationToken = default);

    Task<decimal> GetInterestRate(long caseId, DateTime futureInterestRateValidTo, CancellationToken cancellationToken = default);

    Task<int> CreateResponseCode(CreateResponseCodeRequest request, CancellationToken cancellationToken = default);

    Task<List<Contracts.GetResponseCodeListResponse.Types.GetResponseCodeItem>> GetResponseCodeList(long caseId, CancellationToken cancellationToken = default);
}
