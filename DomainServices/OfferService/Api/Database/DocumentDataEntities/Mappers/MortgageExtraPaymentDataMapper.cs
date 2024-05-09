using CIS.Core.Attributes;
using static DomainServices.OfferService.Api.Database.DocumentDataEntities.MortgageExtraPaymentData;
using __Contracts = DomainServices.OfferService.Contracts;

#pragma warning disable CA1822 // Mark members as static

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

[TransientService, SelfService]
internal sealed class MortgageExtraPaymentDataMapper
{
    public __Contracts.MortgageExtraPaymentFullData MapToFullData(MortgageExtraPaymentData data)
    {
        
        return new __Contracts.MortgageExtraPaymentFullData
        {
            SimulationInputs = MapFromDataInputs(data.SimulationInputs),
            SimulationResults = MapFromDataOutputs(data.SimulationOutputs),
            BasicParameters = MapFromDataBasicParameters(data.BasicParameters)
        };
    }

    public BasicParametersData MapToDataBasicParameters(__Contracts.MortgageExtraPaymentBasicParameters basicParameters)
    {
        return new()
        {
            FeeAmountDiscount = basicParameters.FeeAmountDiscount
        };
    }

    public SimulationInputsData MapToDataInputs(__Contracts.MortgageExtraPaymentSimulationInputs inputs)
    {
        return new()
        {
            ExtraPaymentAmount = inputs.ExtraPaymentAmount,
            ExtraPaymentDate = inputs.ExtraPaymentDate,
            ExtraPaymentReasonId = inputs.ExtraPaymentReasonId,
            IsExtraPaymentFullyRepaid = inputs.IsExtraPaymentFullyRepaid
        };
    }

    public SimulationOutputsData MapToDataOutputs(ExternalServices.EasSimulationHT.Dto.MortgageExtraPaymentResult result)
    {
        return new()
        {
            ExtraPaymentAmount = result.ExtraPaymentAmount,
            FeeAmount = result.FeeAmount,
            InterestAmount = result.InterestAmount,
            InterestCovid = result.InterestCovid,
            InterestOnLate = result.InterestOnLate,
            IsExtraPaymentFullyRepaid = result.IsExtraPaymentComplete,
            IsLoanOverdue = result.IsLoanOverdue,
			IsInstallmentReduced = result.IsInstallmentReduced,
            NewMaturityDate = result.NewMaturityDate,
            NewPaymentAmount = result.NewPaymentAmount,
            OtherUnpaidFees = result.OtherUnpaidFees,
            PrincipalAmount = result.PrincipalAmount,
            SanctionFreeAmount = result.SanctionFreeAmount,
            FixedRateSanctionFreePeriodFrom = result.FixedRateSanctionFreePeriodFrom,
            FixedRateSanctionFreePeriodTo = result.FixedRateSanctionFreePeriodTo,
            FeeAmountContracted = result.FeeAmountContracted,
            FeeCalculationBase = result.FeeCalculationBase,
            FeeTypeId = result.FeeTypeId,
            AnnualSanctionFreePeriodFrom = result.AnnualSanctionFreePeriodFrom,
            AnnualSanctionFreePeriodTo = result.AnnualSanctionFreePeriodTo
        };
    }

    public __Contracts.MortgageExtraPaymentBasicParameters MapFromDataBasicParameters(BasicParametersData basicParameters)
    {
        return new()
        {
            FeeAmountDiscount = basicParameters.FeeAmountDiscount
        };
    }

    public __Contracts.MortgageExtraPaymentSimulationInputs MapFromDataInputs(SimulationInputsData inputs)
    {
        return new()
        {
            ExtraPaymentAmount = inputs.ExtraPaymentAmount,
            ExtraPaymentDate = inputs.ExtraPaymentDate,
            ExtraPaymentReasonId = inputs.ExtraPaymentReasonId,
            IsExtraPaymentFullyRepaid = inputs.IsExtraPaymentFullyRepaid
        };
    }

    public __Contracts.MortgageExtraPaymentSimulationResults MapFromDataOutputs(SimulationOutputsData output)
    {
        return new()
        {
            ExtraPaymentAmount = output.ExtraPaymentAmount,
            FeeAmount = output.FeeAmount,
            InterestAmount = output.InterestAmount,
            InterestCovid = output.InterestCovid,
            InterestOnLate = output.InterestOnLate,
            IsExtraPaymentFullyRepaid = output.IsExtraPaymentFullyRepaid,
            IsLoanOverdue = output.IsLoanOverdue,
			IsInstallmentReduced = output.IsInstallmentReduced,
            NewMaturityDate = output.NewMaturityDate,
            NewPaymentAmount = output.NewPaymentAmount,
            OtherUnpaidFees = output.OtherUnpaidFees,
            PrincipalAmount = output.PrincipalAmount,
            SanctionFreeAmount = output.SanctionFreeAmount,
            FixedRateSanctionFreePeriodFrom = output.FixedRateSanctionFreePeriodFrom,
            FixedRateSanctionFreePeriodTo = output.FixedRateSanctionFreePeriodTo,
            FeeAmountContracted = output.FeeAmountContracted,
            FeeCalculationBase = output.FeeCalculationBase,
            FeeTypeId = output.FeeTypeId,
            AnnualSanctionFreePeriodFrom = output.AnnualSanctionFreePeriodFrom,
            AnnualSanctionFreePeriodTo = output.AnnualSanctionFreePeriodTo
        };
    }
}
