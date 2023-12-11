using CIS.Core.Attributes;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

[TransientService, SelfService]
internal sealed class AdditionalSimulationResultsDataMapper
{
    public AdditionalSimulationResultsData MapToData(SimulationHTResponse results)
    {
        var model = new AdditionalSimulationResultsData
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
                DisplayAsFreeOfCharge = t.zobrazitZdarma.ToBool(),
                IncludeInRPSN = t.zapocitatRPSN.ToBool(),
                Periodicity = t.periodicita ?? String.Empty,
                AccountDateFrom = t.uctovatOd,
            }),
            MarketingActions = results.marketingoveAkce.Select(t => new AdditionalSimulationResultsData.MarketingActionData
            {
                Code = t.typMaAkce,
                Requested = t.zaskrtnuto,
                Applied = t.uplatnena,
                MarketingActionId = t.kodMaAkce,
                Deviation = t.odchylkaSazby,
                Name = t.nazev,
            }).ToList(),
            PaymentScheduleSimple = results.splatkovyKalendarJednoduchy.Select(t => new AdditionalSimulationResultsData.PaymentScheduleSimpleData
            {
                PaymentIndex = t.n,
                PaymentNumber = t.cisloSplatky,
                Date = t.datumSplatky,
                Type = t.typSplatky,
                Amount = t.vyseSplatky
            }).ToList()
        };

        return model;
    }

    private static bool ToBool(this int? value)
        => value.GetValueOrDefault(0) == 1;
}
