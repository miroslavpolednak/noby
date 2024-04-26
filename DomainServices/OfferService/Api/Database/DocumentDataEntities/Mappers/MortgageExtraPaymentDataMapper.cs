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
            FeeAmountDiscounted = basicParameters.FeeAmountDiscounted
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
            IsPaymentReduced = result.IsPaymentReduced,
            NewMaturityDate = result.NewMaturityDate,
            NewPaymentAmount = result.NewPaymentAmount,
            OtherUnpaidFees = result.OtherUnpaidFees,
            PrincipalAmount = result.PrincipalAmount,
            SanctionFreeAmount = result.SanctionFreeAmount,
            SanctionFreePeriodTo = result.SanctionFreePeriodTo,
            SanctionFreePeriodFrom = result.SanctionFreePeriodFrom,
            FeeAmountContracted = result.FeeAmountContracted,
            FeeCalculationBase = result.FeeCalculationBase,
            FeeTypeId = result.FeeTypeId,
            FixedRateValidFrom = result.FixedRateValidFrom,
            FixedRateValidTo = result.FixedRateValidTo
        };
    }

    public __Contracts.MortgageExtraPaymentBasicParameters MapFromDataBasicParameters(BasicParametersData basicParameters)
    {
        return new()
        {
            FeeAmountDiscounted = basicParameters.FeeAmountDiscounted
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
            IsPaymentReduced = output.IsPaymentReduced,
            NewMaturityDate = output.NewMaturityDate,
            NewPaymentAmount = output.NewPaymentAmount,
            OtherUnpaidFees = output.OtherUnpaidFees,
            PrincipalAmount = output.PrincipalAmount,
            SanctionFreeAmount = output.SanctionFreeAmount,
            SanctionFreePeriodFrom = output.SanctionFreePeriodFrom,
            SanctionFreePeriodTo = output.SanctionFreePeriodTo,
            FeeAmountContracted = output.FeeAmountContracted,
            FeeCalculationBase = output.FeeCalculationBase,
            FeeTypeId = output.FeeTypeId,
            FixedRateValidFrom = output.FixedRateValidFrom,
            FixedRateValidTo = output.FixedRateValidTo
        };
    }
}
