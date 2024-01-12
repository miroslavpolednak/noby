using CIS.Core.Attributes;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;
using __Contracts = DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

#pragma warning disable CA1822 // Mark members as static

[TransientService, SelfService]
internal sealed class AdditionalSimulationResultsDataMapper
{
    public AdditionalSimulationResultsData MapToData(SimulationHTResponse results)
    {
        return new AdditionalSimulationResultsData
        {
            Fees = results.poplatky.Select(t => new AdditionalSimulationResultsData.FeeData
            {
                FeeId = t.kodSB,
                DiscountPercentage = t.slevaIC,
                TariffSum = t.sumaSazebnikova,
                ComposedSum = t.sumaSkladackova,
                FinalSum = t.sumaVysledna,
                MarketingActionId = t.kodMaAkce,
                Name = t.textAplikace,
                ShortNameForExample = t.textDokumentaceVzorovyPriklad,
                TariffName = t.textDokumentaceSazebnik ?? String.Empty,
                UsageText = t.pouziti ?? String.Empty,
                TariffTextWithAmount = t.textDokumentaceSazebnikHodnota ?? String.Empty,
                CodeKB = t.kodKB ?? String.Empty,
                DisplayAsFreeOfCharge = t.zobrazitZdarma.GetValueOrDefault() == 1,
                IncludeInRPSN = t.zapocitatRPSN.GetValueOrDefault() == 1,
                Periodicity = t.periodicita ?? String.Empty,
                AccountDateFrom = t.uctovatOd,
            }).ToList(),
            MarketingActions = results.marketingoveAkce is null ? null : results.marketingoveAkce.Select(t => new AdditionalSimulationResultsData.MarketingActionData
            {
                Code = t.typMaAkce,
                Requested = t.zaskrtnuto,
                Applied = t.uplatnena,
                MarketingActionId = t.kodMaAkce,
                Deviation = t.odchylkaSazby,
                Name = t.nazev,
            }).ToList(),
            PaymentScheduleSimple = results.splatkovyKalendarJednoduchy is null ? null : results.splatkovyKalendarJednoduchy.Select(t => new AdditionalSimulationResultsData.PaymentScheduleData
            {
                PaymentIndex = t.n,
                PaymentNumber = t.cisloSplatky,
                Date = t.datumSplatky,
                Type = t.typSplatky,
                Amount = t.vyseSplatky
            }).ToList()
        };
    }

    public __Contracts.AdditionalMortgageSimulationResults MapFromDataToSingle(AdditionalSimulationResultsData? data)
    {
        var model = new __Contracts.AdditionalMortgageSimulationResults();

        if (data?.Fees is not null)
        {
            model.Fees.AddRange(data.Fees.Select(t => new __Contracts.ResultFee
            {
                FeeId = t.FeeId,
                DiscountPercentage = t.DiscountPercentage,
                TariffSum = t.TariffSum,
                ComposedSum = t.ComposedSum,
                FinalSum = t.FinalSum,
                MarketingActionId = t.MarketingActionId,
                Name = t.Name,
                ShortNameForExample = t.ShortNameForExample,
                TariffName = t.TariffName,
                UsageText = t.UsageText,
                TariffTextWithAmount = t.TariffTextWithAmount,
                CodeKB = t.CodeKB,
                DisplayAsFreeOfCharge = t.DisplayAsFreeOfCharge,
                IncludeInRPSN = t.IncludeInRPSN,
                Periodicity = t.Periodicity,
                AccountDateFrom = t.AccountDateFrom
            }));
        }

        if (data?.MarketingActions is not null)
        {
            model.MarketingActions.AddRange(data.MarketingActions.Select(t => new __Contracts.ResultMarketingAction
            {
                Code = t.Code,
                Requested = t.Requested,
                Applied = t.Applied,
                MarketingActionId = t.MarketingActionId,
                Deviation = t.Deviation,
                Name = t.Name
            }));
        }

        if (data?.PaymentScheduleSimple is not null)
        {
            model.PaymentScheduleSimple.AddRange(data.PaymentScheduleSimple.Select(t => new __Contracts.PaymentScheduleSimple
            {
                PaymentIndex = t.PaymentIndex,
                PaymentNumber = t.PaymentNumber,
                Date = t.Date,
                Type = t.Type,
                Amount = t.Amount
            }));
        }

        return model;
    }
}
