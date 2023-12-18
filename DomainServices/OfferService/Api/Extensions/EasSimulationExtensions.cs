using _OS = DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Contracts.v1;
using EasWrapper = ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;
using static DomainServices.OfferService.Api.Database.DocumentDataEntities.OfferData;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;

namespace DomainServices.OfferService.Api;

internal static class EasSimulationExtensions
{
    // marketing actions
    private static EasWrapper.MarketingovaAkce[] ToReqMarketingActions(this SimulationInputsData inputs)
    {
        var list = new List<EEasSimulationMarketingActionType>();

        if (inputs.MarketingActions != null)
        {
            if (inputs.MarketingActions.Domicile)
                list.Add(EEasSimulationMarketingActionType.DOMICILACE);

            if (inputs.MarketingActions.HealthRiskInsurance)
                list.Add(EEasSimulationMarketingActionType.RZP);

            if (inputs.MarketingActions.RealEstateInsurance)
                list.Add(EEasSimulationMarketingActionType.POJIST_NEM);

            if (inputs.MarketingActions.IncomeLoanRatioDiscount)
                list.Add(EEasSimulationMarketingActionType.VYSE_PRIJMU_UVERU);

            if (inputs.MarketingActions.UserVip)
                list.Add(EEasSimulationMarketingActionType.VIP_MAKLER);
        }

        var marketingoveAkce = list.Select(i => new EasWrapper.MarketingovaAkce { typMaAkce = i.ToString(), zaskrtnuto = 1 });
        return marketingoveAkce.ToArray();
    }

    public static EasWrapper.SimulationHTRequest ToEasSimulationRequest(this SimulationInputsData inputs, BasicParametersData basicParameters, Dictionary<int, DrawingDurationsResponse.Types.DrawingDurationItem> drawingDurationsById, Dictionary<int, DrawingTypesResponse.Types.DrawingTypeItem> drawingTypeById)
    {
        var lhutaDocerpani = inputs.DrawingDurationId.HasValue ? drawingDurationsById.GetValueOrDefault(inputs.DrawingDurationId.Value)?.DrawingDuration : null;
        var typCerpani = inputs.DrawingTypeId.HasValue ? drawingTypeById.GetValueOrDefault(inputs.DrawingTypeId.Value)?.StarbuildId : null;

        var model = new EasWrapper.SimulationHTRequest
        {
            settings = new SimSettings
            {
                rezimVolani = 2,
                enableResponseSplatkovyKalendarJednoduchy = 1,
                enableResponseSplatkovyKalendarPlny = 0,
                enableResponseOperace = 0,
                enableResponseRpsnToky = 0
            },
            uver = new EasWrapper.SimSettingsUver
            {
                kodProduktu = inputs.ProductTypeId,
                druhUveru = inputs.LoanKindId,
                vyseUveru = inputs.LoanAmount,
                splatnostUveru = inputs.LoanDuration,
                rozhodnyDenSazby = inputs.GuaranteeDateFrom,
                datumZalozeni = inputs.GuaranteeDateFrom,
                indCenotvorbaOdchylka = -1 * ((decimal)inputs.InterestRateDiscount!),
                periodaFixace = inputs.FixedRatePeriod!.Value,
                predpokladanaHodnotaZajisteni = inputs.CollateralAmount,
                denSplatky = inputs.PaymentDay!.Value,
                kanal = 6,
                lhutaDocerpani = lhutaDocerpani.HasValue ? lhutaDocerpani.Value : null
            },
            urokovaSazba = new EasWrapper.SimSettingsUrokovaSazba
            {
                developerId = inputs.Developer?.DeveloperId,
                developerProjektId = inputs.Developer?.ProjectId,
                zadaZvyhodneni = inputs.IsEmployeeBonusRequested.HasValue ? (inputs.IsEmployeeBonusRequested.Value ? 1 : 0) : null
            },
            ucelyUveru = inputs.LoanPurposes?.Select(i => new EasWrapper.UcelUveru
            {
                kodUcelu = i.LoanPurposeId
            }).ToArray(),
            nastaveniPoplatku = new EasWrapper.SimSettingsPoplatky
            {
                dokument = inputs.FeeSettings?.FeeTariffPurpose!.Value ?? 0,
                vypisBu = inputs.FeeSettings?.IsStatementCharged ?? false ? 1 : 0,
                vypisHu = basicParameters.StatementTypeId!.Value,
                rzpSuma = inputs.RiskLifeInsurance?.Sum,
                rzpFrekvence = inputs.RiskLifeInsurance?.Frequency,
                pojNemovSuma = inputs.RealEstateInsurance?.Sum,
                pojNemovFrekvence = inputs.RealEstateInsurance?.Frequency
            },
            marketingoveAkce = inputs.ToReqMarketingActions(),
            mimoradneOperace = new MimoradnaOperace[]
            {
                new EasWrapper.MimoradnaOperace
                {
                    klic = 1,
                    suma = inputs.LoanAmount.GetValueOrDefault()
                }
            },
            poplatky = inputs.Fees?.Select(i => new EasWrapper.Poplatek
            {
                kodSB = i.FeeId,
                slevaIC = i.DiscountPercentage.GetValueOrDefault(),
            }).ToArray(),
        };

        if (typCerpani.HasValue)
        {
            model.uver.typCerpani = typCerpani.Value;
        }
        if (inputs.ExpectedDateOfDrawing.HasValue)
        {
            model.mimoradneOperace[0].valuta = inputs.ExpectedDateOfDrawing.Value;
        }

        return model;
    }

    public static List<_OS.PaymentScheduleFull>? ToFullPaymentSchedule(this EasWrapper.SimulationHTResponse easSimulationResponse)
    {
        return easSimulationResponse.splatkovyKalendarPlny?
            .Where(t => t is not null)
            .Select(t => new _OS.PaymentScheduleFull
            {
                PaymentIndex = t.n,
                PaymentNumber = t.c ?? String.Empty,
                Date = t.d ?? String.Empty,
                Amount = t.p ?? String.Empty,
                Principal = t.j ?? String.Empty,
                Interest = t.u ?? String.Empty,
                RemainingPrincipal = t.z ?? String.Empty,
            })
            .ToList();
    }

    private static bool ToBool(this int? value)
        => value.GetValueOrDefault(0) == 1;
}
