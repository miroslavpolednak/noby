using DomainServices.OfferService.Contracts;

using EasWrapper = ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper;

namespace DomainServices.OfferService.Api;

internal static class EasSimulationExtensions
{

    #region InputsToRequest

    // settings
    private static readonly EasWrapper.SimSettings ReqDefaultSettings = new EasWrapper.SimSettings
    {
        rezimVolani = 2,
        enableResponseSplatkovyKalendarJednoduchy = 1,
        enableResponseSplatkovyKalendarPlny = 0,
        enableResponseOperace = 0,
        enableResponseRpsnToky = 0,
    };


    // loan
    private static EasWrapper.SimSettingsUver ToReqLoan(this SimulationInputs inputs)
    {
        var uver = new EasWrapper.SimSettingsUver
        {
            kodProduktu = inputs.ProductTypeId,
            druhUveru = inputs.LoanKindId,
            vyseUveru = inputs.LoanAmount,
            splatnostUveru = inputs.LoanDuration,
            rozhodnyDenSazby = inputs.GuaranteeDateFrom,
            datumZalozeni = inputs.GuaranteeDateFrom,
            indCenotvorbaOdchylka = -1 * ((decimal)inputs.InterestRateDiscount!),
            periodaFixace = inputs.FixedRatePeriod,
            predpokladanaHodnotaZajisteni = inputs.CollateralAmount,
            typCerpani = inputs.DrawingType!.Value,
            lhutaDocerpani = inputs.DrawingDuration,
        };

        uver.denSplatky = inputs.PaymentDay!.Value;

        if (inputs.GuaranteeDateFrom != null)
        {
            uver.rozhodnyDenSazby = inputs.GuaranteeDateFrom;
            uver.datumZalozeni = inputs.GuaranteeDateFrom;
        }

        uver.kanal = 6; // [MOCK] (default 6)

        return uver;
    }


    // interest rate
    private static EasWrapper.SimSettingsUrokovaSazba ToReqInterestRate(this SimulationInputs inputs)
    {
        var urokovaSazba = new EasWrapper.SimSettingsUrokovaSazba
        {
            developerId = inputs.Developer?.DeveloperId,
            developerProjektId = inputs.Developer?.ProjectId,
        };

        if (inputs.IsEmployeeBonusRequested.HasValue)
        {
            urokovaSazba.zadaZvyhodneni = inputs.IsEmployeeBonusRequested.Value ? 1 : 0;
        }

        return urokovaSazba;
    }


    // loan purposes
    private static EasWrapper.UcelUveru[] ToReqLoanPurposes(this SimulationInputs inputs)
    {
        var ucelyUveru = inputs.LoanPurposes.Select(i => new EasWrapper.UcelUveru
        {
            kodUcelu = i.LoanPurposeId
        });

        return ucelyUveru.ToArray();
    }


    // fee settings
    private static EasWrapper.SimSettingsPoplatky? ToReqFeeSettings(this SimulationInputs inputs)
    {
        if (inputs.FeeSettings == null)
        {
            return null;
        }

        var nastaveniPoplatku = new EasWrapper.SimSettingsPoplatky
        {
            // dokument = inputs.FeeSettings.FeeTariffPurpose,
            // vypisHu = inputs.FeeSettings.StatementTypeId,
            dokument = inputs.FeeSettings.FeeTariffPurpose!.Value,
            vypisHu = inputs.FeeSettings.StatementTypeId!.Value,
            vypisBu = inputs.FeeSettings.IsStatementCharged ? 1 : 0,
        };

        return nastaveniPoplatku;
    }


    // marketing actions
    private static EasWrapper.MarketingovaAkce[] ToReqMarketingActions(this SimulationInputs inputs)
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


    // fees
    private static EasWrapper.Poplatek[] ToReqFees(this SimulationInputs inputs)
    {
        var poplatky = inputs.Fees.Select(i => new EasWrapper.Poplatek
        {
            kodSB = i.FeeId,
            slevaIC = i.DiscountPercentage,
        });

        return poplatky.ToArray();
    }


    /// <summary>
    /// Converts Offer object [SimulationInputs] to EasSimulationHT object [SimulationHTRequest].
    /// </summary>
    public static EasWrapper.SimulationHTRequest ToEasSimulationRequest(this SimulationInputs inputs)
    {
        //return SampleRequest;

        // request
        return new EasWrapper.SimulationHTRequest
        {
            settings = ReqDefaultSettings,
            uver = inputs.ToReqLoan(),
            urokovaSazba = inputs.ToReqInterestRate(),
            ucelyUveru = inputs.ToReqLoanPurposes(),
            nastaveniPoplatku = inputs.ToReqFeeSettings(),
            marketingoveAkce = inputs.ToReqMarketingActions(),
            poplatky = inputs.ToReqFees(),
        };
    }

    public static EasWrapper.SimulationHTRequest SampleRequest = new EasWrapper.SimulationHTRequest
    {
        settings = ReqDefaultSettings,
        uver = new EasWrapper.SimSettingsUver
        {
            datumZalozeni = new DateTime(2022, 2, 1),
            kodProduktu = 20001,
            kanal = 6,
            druhUveru = 2000,
            denSplatky = 25,
            vyseUveru = 3150020,
            splatnostUveru = 36,
            rozhodnyDenSazby = new DateTime(2022, 2, 1),
            indCenotvorbaOdchylka = -0.3m,
            periodaFixace = 24,
            predpokladanaHodnotaZajisteni = 6500000,
            lhutaDocerpani = 0,
            //< eas:typCerpani > 0 </ eas:typCerpani >          < !--NEW:v006 - 0 / 1, kde 1 = postupne cerpani-- >
        },
        //urokovaSazba = new EasWrapper.SimSettingsUrokovaSazba
        //{
        //    zadaZvyhodneni = 0,
        //    developerId = 0,
        //    developerProjektId = 0,
        //},
        //mimoradneOperace = new EasWrapper.MimoradnaOperace[]
        //{
        //    new EasWrapper.MimoradnaOperace
        //    {
        //        klic=1,
        //        valuta=new DateTime(2022,12,31),
        //        suma=300000.00m,
        //        poznamka= "Cerpani"
        //    }
        //},
        ucelyUveru = new EasWrapper.UcelUveru[] {
            new EasWrapper.UcelUveru{  kodUcelu = 201},
            new EasWrapper.UcelUveru{  kodUcelu = 202}
        },
        //nastaveniPoplatku = new EasWrapper.SimSettingsPoplatky { dokument = 0, vypisHu = 3, vypisBu = 1 },
        //marketingoveAkce = new EasWrapper.MarketingovaAkce[]
        //{
        //    new EasWrapper.MarketingovaAkce{zaskrtnuto=1,typMaAkce="DOMICILACE"},
        //    new EasWrapper.MarketingovaAkce{zaskrtnuto=1,typMaAkce="RZP"}
        //},
        //poplatky = new EasWrapper.Poplatek[]
        //{
        //    new EasWrapper.Poplatek{kodSB=2001, slevaIC=50.00m}
        //},
    };

    #endregion

    #region ResponseToResults

    // loan
    private static SimulationResults AddResResults(this SimulationResults results, EasWrapper.UverVysledky res)
    {
        results.LoanAmount = res.vyseUveru;
        results.LoanDuration = res.splatnostUveru;
        results.LoanDueDate = res.splatnostUveruDatum;
        results.LoanPaymentAmount = res.splatkaUveru;
        results.LoanInterestRateProvided = res.sazbaPoskytnuta;
        results.EmployeeBonusLoanCode = res.kodZvyhodneni;
        results.LoanToValue = res.LTV;
        results.ContractSignedDate = res.datumPodpisuSmlouvy;
        results.DrawingDateTo = res.datumDocerpani;
        results.AnnuityPaymentsDateFrom = res.datumZahajeniAnuitnihoSplaceni;
        results.AnnuityPaymentsCount = res.pocetAnuitnichSplatek;
        results.Aprc = res.rpsn;
        results.LoanTotalAmount = res.celkoveNakladyUveru;
        //results.AprcRefix = res.rpsnRefix;
        //results.LoanTotalAmountRefix = res.celkoveNakladyUveruRefix;

        return results;
    }


    // interest rate
    private static SimulationResults AddResResults(this SimulationResults results, EasWrapper.UrokovaSazba res)
    {
        results.LoanInterestRate = res.urokovaSazba;
        results.LoanInterestRateAnnounced = res.vyhlasovana;
        results.LoanInterestRateAnnouncedType = res.vyhlasovanaTyp;

        return results;
    }


    // payment schedule (simple)
    private static SimulationResults AddResResults(this SimulationResults results, EasWrapper.SplS[] res)
    {
        var items = res.Select(i => new PaymentScheduleSimple {
            PaymentIndex = i.n,
            PaymentNumber = i.cisloSplatky,
            Date = i.datumSplatky,  // ´string´ field - SimulationService is responsible for correct formating
            Type = i.typSplatky,
            Amount = i.vyseSplatky  // ´string´ field - SimulationService is responsible for correct formating
        });

        results.PaymentScheduleSimple.AddRange(items);
        
        return results;
    }


    // marketing actions
    private static SimulationResults AddResResults(this SimulationResults results, EasWrapper.MarketingovaAkce[] res)
    {
        var items = res.Select(i => new ResultMarketingAction
        {
            Code = i.typMaAkce,
            Requested = i.zaskrtnuto,
            Applied = i.uplatnena,
            MarketingActionId = i.kodMaAkce,
            Deviation = i.odchylkaSazby,
        });

        results.MarketingActions.AddRange(items);

        return results;
    }

    // fees
    private static SimulationResults AddResResults(this SimulationResults results, EasWrapper.Poplatek[] res)
    {
        var items = res.Select(i => new ResultFee
        {
            FeeId = i.kodSB,
            DiscountPercentage = i.slevaIC,
            TariffSum = i.sumaSazebnikova,
            ComposedSum = i.sumaSkladackova,
            FinalSum = i.sumaVysledna,
            MarketingActionId = i.kodMaAkce,
            Name = i.textAplikace,
            ShortNameForExample= i.textDokumentaceVzorovyPriklad,
            TariffName = i.textDokumentaceSazebnik,
            UsageText = i.pouziti,
            TariffTextWithAmount = i.textDokumentaceSazebnikHodnota,
            CodeKB = i.kodKB,
            DisplayAsFreeOfCharge = i.zobrazitZdarma.ToBool(),
            IncludeInRPSN = i.zapocitatRPSN.ToBool(),
            Periodicity = i.periodicita,
            AccountDateFrom = i.uctovatOd,
        });

        results.Fees.AddRange(items);

        return results;
    }


    /// <summary>
    /// Converts EasSimulationHT object [SimulationHTResponse] to Offer object [SimulationResults].
    /// </summary>
    public static SimulationResults ToSimulationResults(this EasWrapper.SimulationHTResponse easSimulationResponse)
    {
        var results = new SimulationResults()
            .AddResResults(easSimulationResponse.uverVysledky)                  // loan
            .AddResResults(easSimulationResponse.urokovaSazba)                  // interest rate
            .AddResResults(easSimulationResponse.splatkovyKalendarJednoduchy)   // payment schedule (simple)
            .AddResResults(easSimulationResponse.marketingoveAkce)              // marketing actions
            .AddResResults(easSimulationResponse.poplatky);                     // fees

        return results;
    }

    #endregion

    #region Shared

    private static bool ToBool(this int? value)
    {
        if (!value.HasValue)
        {
            return false;
        }

        return value.Value == 1 ? true : false;
    }

    #endregion

}
