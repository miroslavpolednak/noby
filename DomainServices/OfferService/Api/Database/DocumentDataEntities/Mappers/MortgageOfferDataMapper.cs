using CIS.Core.Attributes;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;
using static DomainServices.OfferService.Api.Database.DocumentDataEntities.MortgageOfferData;
using __Contracts = DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method
#pragma warning disable CA1822 // Mark members as static

[TransientService, SelfService]
internal sealed class MortgageOfferDataMapper
{
    public BasicParametersData MapToDataBasicParameters(__Contracts.MortgageOfferBasicParameters basicParameters)
    {
        return new()
        {
            StatementTypeId = basicParameters.StatementTypeId,
            FinancialResourcesOther = basicParameters.FinancialResourcesOther,
            FinancialResourcesOwn = basicParameters.FinancialResourcesOwn,
            GuaranteeDateTo = basicParameters.GuaranteeDateTo
        };
    }

    public SimulationInputsData MapToDataInputs(__Contracts.MortgageOfferSimulationInputs inputs)
    {
        return new()
        {
            CollateralAmount = inputs.CollateralAmount,
            DrawingDurationId = inputs.DrawingDurationId,
            DrawingTypeId = inputs.DrawingTypeId,
            ExpectedDateOfDrawing = inputs.ExpectedDateOfDrawing,
            FixedRatePeriod = inputs.FixedRatePeriod,
            GuaranteeDateFrom = inputs.GuaranteeDateFrom,
            InterestRateDiscount = inputs.InterestRateDiscount,
            IsEmployeeBonusRequested = inputs.IsEmployeeBonusRequested,
            LoanAmount = inputs.LoanAmount,
            LoanDuration = inputs.LoanDuration,
            LoanKindId = inputs.LoanKindId,
            LoanPurposes = inputs.LoanPurposes is null ? null : inputs.LoanPurposes.Select(t => new LoanPurposeData
            {
                LoanPurposeId = t.LoanPurposeId,
                Sum = t.Sum
            }).ToList(),
            PaymentDay = inputs.PaymentDay,
            ProductTypeId = inputs.ProductTypeId,
            MarketingActions = inputs.MarketingActions is null ? null : new()
            {
                Domicile = inputs.MarketingActions.Domicile,
                HealthRiskInsurance = inputs.MarketingActions.HealthRiskInsurance,
                IncomeLoanRatioDiscount = inputs.MarketingActions.IncomeLoanRatioDiscount,
                RealEstateInsurance = inputs.MarketingActions.RealEstateInsurance,
                UserVip = inputs.MarketingActions.UserVip
            },
            RealEstateInsurance = inputs.RealEstateInsurance is null ? null : new()
            {
                Frequency = inputs.RealEstateInsurance.Frequency,
                Sum = inputs.RealEstateInsurance.Sum
            },
            RiskLifeInsurance = inputs.RiskLifeInsurance is null ? null : new()
            {
                Frequency = inputs.RiskLifeInsurance.Frequency,
                Sum = inputs.RiskLifeInsurance.Sum
            },
            Fees = inputs.Fees is null ? null : inputs.Fees.Select(t => new InputFeeData
            {
                DiscountPercentage = t.DiscountPercentage,
                FeeId = t.FeeId
            }).ToList(),
            Developer = inputs.Developer is null ? null : new()
            {
                Description = inputs.Developer.Description,
                DeveloperId = inputs.Developer.DeveloperId,
                ProjectId = inputs.Developer.ProjectId
            },
            FeeSettings = inputs.FeeSettings is null ? null : new()
            {
                FeeTariffPurpose = inputs.FeeSettings.FeeTariffPurpose,
                IsStatementCharged = inputs.FeeSettings.IsStatementCharged
            },
        };
    }

    public SimulationOutputsData MapToDataOutputs(SimulationHTResponse results)
    {
        var model = new MortgageOfferData.SimulationOutputsData
        {
            LoanAmount = results.uverVysledky.vyseUveru,
            LoanDuration = results.uverVysledky.splatnostUveru,
            LoanDueDate = results.uverVysledky.splatnostUveruDatum,
            LoanPaymentAmount = results.uverVysledky.splatkaUveru,
            LoanInterestRateProvided = results.uverVysledky.sazbaPoskytnuta,
            EmployeeBonusLoanCode = results.uverVysledky.kodZvyhodneni,
            LoanToValue = results.uverVysledky.LTV,
            ContractSignedDate = results.uverVysledky.datumPodpisuSmlouvy,
            DrawingDateTo = results.uverVysledky.datumDocerpani,
            AnnuityPaymentsDateFrom = results.uverVysledky.datumZahajeniAnuitnihoSplaceni,
            AnnuityPaymentsCount = results.uverVysledky.pocetAnuitnichSplatek,
            Aprc = results.uverVysledky.rpsn,
            LoanTotalAmount = results.uverVysledky.celkoveNakladyUveru,
            //AprcRefix = results.rpsnRefix,
            //LoanTotalAmountRefix = results.celkoveNakladyUveruRefix,
            LoanInterestRate = results.urokovaSazba.urokovaSazba,
            LoanInterestRateAnnounced = results.urokovaSazba.vyhlasovana,
            LoanInterestRateAnnouncedType = results.urokovaSazba.vyhlasovanaTyp
        };

        if (results.errorInfo?.warningy?.Any() ?? false)
        {
            model.Warnings = results.errorInfo.warningy.Select(t => new MortgageOfferData.SimulationResultWarningData
            {
                WarningCode = t.kodWarningu,
                WarningInternalMessage = t.publicMsg ?? "",
                WarningText = t.textWarningu ?? ""
            }).ToList();
        }

        return model;
    }

    public (__Contracts.MortgageOfferBasicParameters? BasicParameters, __Contracts.MortgageOfferSimulationInputs? SimulationInputs, __Contracts.MortgageOfferSimulationResults? SimulationResults) MapFromDataToSingle(
        MortgageOfferData.BasicParametersData? basicParameters,
        MortgageOfferData.SimulationInputsData? simulationInputs,
        MortgageOfferData.SimulationOutputsData? simulationOutputs)
    {
        __Contracts.MortgageOfferBasicParameters? basicParametersModel = basicParameters is null ? null : new __Contracts.MortgageOfferBasicParameters
        {
            StatementTypeId = basicParameters.StatementTypeId,
            FinancialResourcesOther = basicParameters.FinancialResourcesOther,
            FinancialResourcesOwn = basicParameters.FinancialResourcesOwn,
            GuaranteeDateTo = basicParameters.GuaranteeDateTo
        };

        __Contracts.MortgageOfferSimulationInputs? simulationInputsModel = null;
        if (simulationInputs is not null)
        {
            simulationInputsModel = new __Contracts.MortgageOfferSimulationInputs
            {
                CollateralAmount = simulationInputs.CollateralAmount,
                DrawingDurationId = simulationInputs.DrawingDurationId,
                DrawingTypeId = simulationInputs.DrawingTypeId,
                ExpectedDateOfDrawing = simulationInputs.ExpectedDateOfDrawing,
                FixedRatePeriod = simulationInputs.FixedRatePeriod,
                GuaranteeDateFrom = simulationInputs.GuaranteeDateFrom,
                InterestRateDiscount = simulationInputs.InterestRateDiscount,
                IsEmployeeBonusRequested = simulationInputs.IsEmployeeBonusRequested,
                LoanAmount = simulationInputs.LoanAmount,
                LoanDuration = simulationInputs.LoanDuration,
                LoanKindId = simulationInputs.LoanKindId,
                PaymentDay = simulationInputs.PaymentDay,
                ProductTypeId = simulationInputs.ProductTypeId,
                MarketingActions = simulationInputs.MarketingActions is null ? null : new()
                {
                    Domicile = simulationInputs.MarketingActions.Domicile,
                    HealthRiskInsurance = simulationInputs.MarketingActions.HealthRiskInsurance,
                    IncomeLoanRatioDiscount = simulationInputs.MarketingActions.IncomeLoanRatioDiscount,
                    RealEstateInsurance = simulationInputs.MarketingActions.RealEstateInsurance,
                    UserVip = simulationInputs.MarketingActions.UserVip
                },
                RealEstateInsurance = simulationInputs.RealEstateInsurance is null ? null : new()
                {
                    Frequency = simulationInputs.RealEstateInsurance.Frequency,
                    Sum = simulationInputs.RealEstateInsurance.Sum
                },
                RiskLifeInsurance = simulationInputs.RiskLifeInsurance is null ? null : new()
                {
                    Frequency = simulationInputs.RiskLifeInsurance.Frequency,
                    Sum = simulationInputs.RiskLifeInsurance.Sum
                },
                Developer = simulationInputs.Developer is null ? null : new()
                {
                    Description = simulationInputs.Developer.Description,
                    DeveloperId = simulationInputs.Developer.DeveloperId,
                    ProjectId = simulationInputs.Developer.ProjectId
                },
                FeeSettings = simulationInputs.FeeSettings is null ? null : new()
                {
                    IsStatementCharged = simulationInputs.FeeSettings.IsStatementCharged,
                    FeeTariffPurpose = simulationInputs.FeeSettings.FeeTariffPurpose
                }
            };

            if (simulationInputs.LoanPurposes is not null)
            {
                simulationInputsModel.LoanPurposes.AddRange(simulationInputs.LoanPurposes.Select(t => new __Contracts.LoanPurpose
                {
                    LoanPurposeId = t.LoanPurposeId,
                    Sum = t.Sum
                }));
            }

            if (simulationInputs.Fees is not null)
            {
                simulationInputsModel.Fees.AddRange(simulationInputs.Fees.Select(t => new __Contracts.InputFee
                {
                    DiscountPercentage = t.DiscountPercentage,
                    FeeId = t.FeeId
                }));
            }
        }

        __Contracts.MortgageOfferSimulationResults? simulationResultsModel = null;
        if (simulationOutputs is not null)
        {
            simulationResultsModel = new __Contracts.MortgageOfferSimulationResults
            {
                ContractSignedDate = simulationOutputs.ContractSignedDate,
                AnnuityPaymentsCount = simulationOutputs.AnnuityPaymentsCount,
                AnnuityPaymentsDateFrom = simulationOutputs.AnnuityPaymentsDateFrom,
                Aprc = simulationOutputs.Aprc,
                DrawingDateTo = simulationOutputs.DrawingDateTo,
                EmployeeBonusDeviation = simulationOutputs.EmployeeBonusDeviation,
                EmployeeBonusLoanCode = simulationOutputs.EmployeeBonusLoanCode,
                LoanAmount = simulationOutputs.LoanAmount,
                LoanDueDate = simulationOutputs.LoanDueDate,
                LoanInterestRate = simulationOutputs.LoanInterestRate,
                LoanInterestRateAnnounced = simulationOutputs.LoanInterestRateAnnounced,
                LoanInterestRateAnnouncedType = simulationOutputs.LoanInterestRateAnnouncedType,
                LoanInterestRateProvided = simulationOutputs.LoanInterestRateProvided,
                LoanPaymentAmount = simulationOutputs.LoanPaymentAmount,
                LoanToValue = simulationOutputs.LoanToValue,
                LoanTotalAmount = simulationOutputs.LoanTotalAmount,
                MarketingActionsDeviation = simulationOutputs.MarketingActionsDeviation,
                LoanDuration = simulationOutputs.LoanDuration
            };

            if (simulationOutputs.Warnings is not null)
            {
                simulationResultsModel.Warnings.AddRange(simulationOutputs.Warnings.Select(t => new __Contracts.SimulationResultWarning
                {
                    WarningCode = t.WarningCode,
                    WarningInternalMessage = t.WarningInternalMessage,
                    WarningText = t.WarningText
                }));
            }
        }

        return (basicParametersModel, simulationInputsModel, simulationResultsModel);
    }
}
