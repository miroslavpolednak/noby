﻿using Amazon.Runtime.Internal.Transform;
using CIS.Core.ErrorCodes;

namespace DomainServices.OfferService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int OfferNotFound = 10000;
    public const int OfferIdIsEmpty = 10001;
    public const int ResourceProcessIdIsEmpty = 10003;
    public const int SimulationInputsIsEmpty = 10004;
    public const int ProductTypeIdIsEmpty = 10005;
    public const int LoanKindIdIsEmpty = 10006;
    public const int GuaranteeDateFromTooOld = 10007;
    public const int MarketingActionsIsEmpty = 10008;
    public const int FixedRatePeriodIsEmpty = 10009;
    public const int LoanAmountIsEmpty = 10010;
    public const int LoanDurationIsEmpty = 10011;
    public const int CollateralAmountIsEmpty = 10018;
    public const int GuaranteeDateToSet = 10019;
    public const int CreditWorthinessNullInput = 10021;
    public const int MortgageRetentionAmountNotValid = 10026;
    public const int MortgageRetentionAmountIndividualPriceNotValid = 10027;
    public const int CaseIdNotFoundOnOffer = 10029;
    public const int CaseIdIsEmpty = 10030;
    public const int ResponseCodeTypeIdNotFound = 10031;
    public const int CaseIdNotFoundInKonsDb = 10032;
    
    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { OfferNotFound, "Offer ID {PropertyValue} not found" },
            { OfferIdIsEmpty, "OfferId is not specified" },
            { CaseIdIsEmpty, "CaseId is not specified" },
            { ResourceProcessIdIsEmpty, "ResourceProcessId is missing or is in invalid format" },
            { SimulationInputsIsEmpty, "SimulationInputs are not specified" },
            { ProductTypeIdIsEmpty, "SimulationInputs.ProductTypeId is not specified" },
            { LoanKindIdIsEmpty, "SimulationInputs.LoanKindId is not specified" },
            { GuaranteeDateFromTooOld, $"SimulationInputs.GuaranteeDateFrom can't be older then {AppDefaults.MaxGuaranteeInDays} days" },
            { MarketingActionsIsEmpty, "SimulationInputs.MarketingActions are not specified" },
            { FixedRatePeriodIsEmpty, "SimulationInputs.FixedRatePeriod is not specified" },
            { LoanAmountIsEmpty, "SimulationInputs.LoanAmount is not specified" },
            { LoanDurationIsEmpty, "SimulationInputs.LoanDuration is not specified" },
            { CollateralAmountIsEmpty, "SimulationInputs.CollateralAmount is not specified" },
            { GuaranteeDateToSet, "BasicParameters.GuaranteeDateTo is auto generated parameter - can't be set by consumer" },
            { CreditWorthinessNullInput, "Credit Worthiness was requested but the input is null" },
            { MortgageRetentionAmountNotValid, "SimulateMortgageRetentionRequest.BasicParameters.Amount is not valid" },
            { MortgageRetentionAmountIndividualPriceNotValid, "SimulateMortgageRetentionRequest.BasicParameters.AmountIndividualPrice is not valid" },
            { CaseIdNotFoundOnOffer, "Offer.CaseId is empty" },
            { ResponseCodeTypeIdNotFound, "ResponseCodeTypeId not found in codebook" },
            { CaseIdNotFoundInKonsDb, "CaseId not found in konsDb view" }
        });

        return Messages;
    }
}
