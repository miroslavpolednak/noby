using CIS.Core.Attributes;
using DomainServices.OfferService.Contracts;
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

    public MortgageOfferSimulationResults MapToSimulationResults(MortgageOfferData.SimulationOutputsData data)
    {
        var result = new MortgageOfferSimulationResults()
        {
            ContractSignedDate = data.ContractSignedDate,
            AnnuityPaymentsCount = data.AnnuityPaymentsCount,
            AnnuityPaymentsDateFrom = data.AnnuityPaymentsDateFrom,
            Aprc = data.Aprc,
            DrawingDateTo = data.DrawingDateTo,
            EmployeeBonusDeviation = data.EmployeeBonusDeviation,
            EmployeeBonusLoanCode = data.EmployeeBonusLoanCode,
            LoanAmount = data.LoanAmount,
            LoanDueDate = data.LoanDueDate,
            LoanInterestRate = data.LoanInterestRate,
            LoanInterestRateAnnounced = data.LoanInterestRateAnnounced,
            LoanInterestRateAnnouncedType = data.LoanInterestRateAnnouncedType,
            LoanInterestRateProvided = data.LoanInterestRateProvided,
            LoanPaymentAmount = data.LoanPaymentAmount,
            LoanToValue = data.LoanToValue,
            LoanTotalAmount = data.LoanTotalAmount,
            MarketingActionsDeviation = data.MarketingActionsDeviation,
            LoanDuration = data.LoanDuration
        };

        if (data.Warnings is not null)
        {
            result.Warnings.AddRange(data.Warnings.Select(t => new __Contracts.SimulationResultWarning
            {
                WarningCode = t.WarningCode,
                WarningInternalMessage = t.WarningInternalMessage,
                WarningText = t.WarningText
            }));
        }

        return result;
    }

    public MortgageOfferFullData MapToFullData(MortgageOfferData data)
    {
        var result = new MortgageOfferFullData
        {
            BasicParameters = new()
            {
                StatementTypeId = data.BasicParameters.StatementTypeId,
                FinancialResourcesOther = data.BasicParameters.FinancialResourcesOther,
                FinancialResourcesOwn = data.BasicParameters.FinancialResourcesOwn,
                GuaranteeDateTo = data.BasicParameters.GuaranteeDateTo
            },
            SimulationInputs = new()
            {
                CollateralAmount = data.SimulationInputs.CollateralAmount,
                DrawingDurationId = data.SimulationInputs.DrawingDurationId,
                DrawingTypeId = data.SimulationInputs.DrawingTypeId,
                ExpectedDateOfDrawing = data.SimulationInputs.ExpectedDateOfDrawing,
                FixedRatePeriod = data.SimulationInputs.FixedRatePeriod,
                GuaranteeDateFrom = data.SimulationInputs.GuaranteeDateFrom,
                InterestRateDiscount = data.SimulationInputs.InterestRateDiscount,
                IsEmployeeBonusRequested = data.SimulationInputs.IsEmployeeBonusRequested,
                LoanAmount = data.SimulationInputs.LoanAmount,
                LoanDuration = data.SimulationInputs.LoanDuration,
                LoanKindId = data.SimulationInputs.LoanKindId,
                PaymentDay = data.SimulationInputs.PaymentDay,
                ProductTypeId = data.SimulationInputs.ProductTypeId,
                MarketingActions = data.SimulationInputs.MarketingActions is null ? null : new()
                {
                    Domicile = data.SimulationInputs.MarketingActions.Domicile,
                    HealthRiskInsurance = data.SimulationInputs.MarketingActions.HealthRiskInsurance,
                    IncomeLoanRatioDiscount = data.SimulationInputs.MarketingActions.IncomeLoanRatioDiscount,
                    RealEstateInsurance = data.SimulationInputs.MarketingActions.RealEstateInsurance,
                    UserVip = data.SimulationInputs.MarketingActions.UserVip
                },
                RealEstateInsurance = data.SimulationInputs.RealEstateInsurance is null ? null : new()
                {
                    Frequency = data.SimulationInputs.RealEstateInsurance.Frequency,
                    Sum = data.SimulationInputs.RealEstateInsurance.Sum
                },
                RiskLifeInsurance = data.SimulationInputs.RiskLifeInsurance is null ? null : new()
                {
                    Frequency = data.SimulationInputs.RiskLifeInsurance.Frequency,
                    Sum = data.SimulationInputs.RiskLifeInsurance.Sum
                },
                Developer = data.SimulationInputs.Developer is null ? null : new()
                {
                    Description = data.SimulationInputs.Developer.Description,
                    DeveloperId = data.SimulationInputs.Developer.DeveloperId,
                    ProjectId = data.SimulationInputs.Developer.ProjectId
                },
                FeeSettings = data.SimulationInputs.FeeSettings is null ? null : new()
                {
                    IsStatementCharged = data.SimulationInputs.FeeSettings.IsStatementCharged,
                    FeeTariffPurpose = data.SimulationInputs.FeeSettings.FeeTariffPurpose
                }
            },
            SimulationResults = MapToSimulationResults(data.SimulationOutputs)
        };

        if (data.SimulationInputs.LoanPurposes is not null)
        {
            result.SimulationInputs.LoanPurposes.AddRange(data.SimulationInputs.LoanPurposes.Select(t => new __Contracts.LoanPurpose
            {
                LoanPurposeId = t.LoanPurposeId,
                Sum = t.Sum
            }));
        }

        if (data.SimulationInputs.Fees is not null)
        {
            result.SimulationInputs.Fees.AddRange(data.SimulationInputs.Fees.Select(t => new __Contracts.InputFee
            {
                DiscountPercentage = t.DiscountPercentage,
                FeeId = t.FeeId
            }));
        }

        return result;
    }
}
