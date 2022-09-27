using DomainServices.CodebookService.Contracts.Endpoints.DrawingDurations;
using DomainServices.CodebookService.Contracts.Endpoints.DrawingTypes;
using _OS = DomainServices.OfferService.Contracts;
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

    // settings for Full Payment Schedule [FPT]
    private static readonly EasWrapper.SimSettings ReqFpsSettings = new EasWrapper.SimSettings
    {
        rezimVolani = 2,
        enableResponseSplatkovyKalendarJednoduchy = 0,
        enableResponseSplatkovyKalendarPlny = 1,
        enableResponseOperace = 0,
        enableResponseRpsnToky = 0,
    };


    // loan
    private static EasWrapper.SimSettingsUver ToReqLoan(this _OS.MortgageSimulationInputs inputs, Dictionary<int, DrawingDurationItem> drawingDurationsById, Dictionary<int, DrawingTypeItem> drawingTypeById)
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
            periodaFixace = inputs.FixedRatePeriod!.Value,
            predpokladanaHodnotaZajisteni = inputs.CollateralAmount,
            denSplatky = inputs.PaymentDay!.Value,
        };

        var lhutaDocerpani = inputs.DrawingDurationId.HasValue ? drawingDurationsById.GetValueOrDefault(inputs.DrawingDurationId.Value)?.DrawingDuration : null;
        if (lhutaDocerpani.HasValue)
        {
            uver.lhutaDocerpani = lhutaDocerpani;
        }

        var typCerpani = inputs.DrawingTypeId.HasValue ? drawingTypeById.GetValueOrDefault(inputs.DrawingTypeId.Value)?.StarbuildId : null;
        if (typCerpani.HasValue)
        {
            uver.typCerpani = typCerpani.Value;
        }

        if (inputs.GuaranteeDateFrom != null)
        {
            uver.rozhodnyDenSazby = inputs.GuaranteeDateFrom;
            uver.datumZalozeni = inputs.GuaranteeDateFrom;
        }

        uver.kanal = 6; // [MOCK] (default 6)

        return uver;
    }


    // interest rate
    private static EasWrapper.SimSettingsUrokovaSazba ToReqInterestRate(this _OS.MortgageSimulationInputs inputs)
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
    private static EasWrapper.UcelUveru[] ToReqLoanPurposes(this _OS.MortgageSimulationInputs inputs)
    {
        var ucelyUveru = inputs.LoanPurposes.Select(i => new EasWrapper.UcelUveru
        {
            kodUcelu = i.LoanPurposeId
        });

        return ucelyUveru.ToArray();
    }


    // fee settings
    private static EasWrapper.SimSettingsPoplatky? ToReqFeeSettings(this _OS.MortgageSimulationInputs inputs, _OS.BasicParameters basicParameters)
    {
        if (inputs.FeeSettings == null)
        {
            return null;
        }

        var nastaveniPoplatku = new EasWrapper.SimSettingsPoplatky
        {
            dokument = inputs.FeeSettings.FeeTariffPurpose!.Value,
            vypisBu = inputs.FeeSettings.IsStatementCharged ? 1 : 0,
            vypisHu = basicParameters.StatementTypeId!.Value,
        };

        if (inputs.RiskLifeInsurance != null)
        {
            nastaveniPoplatku.rzpSuma = inputs.RiskLifeInsurance.Sum;
            nastaveniPoplatku.rzpFrekvence = inputs.RiskLifeInsurance.Frequency;
        }

        if (inputs.RealEstateInsurance != null)
        {
            nastaveniPoplatku.pojNemovSuma = inputs.RealEstateInsurance.Sum;
            nastaveniPoplatku.pojNemovFrekvence = inputs.RealEstateInsurance.Frequency;
        }

        return nastaveniPoplatku;
    }


    // marketing actions
    private static EasWrapper.MarketingovaAkce[] ToReqMarketingActions(this _OS.MortgageSimulationInputs inputs)
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
    private static EasWrapper.Poplatek[] ToReqFees(this _OS.MortgageSimulationInputs inputs)
    {
        var poplatky = inputs.Fees.Select(i => new EasWrapper.Poplatek
        {
            kodSB = i.FeeId,
            slevaIC = i.DiscountPercentage,
        });

        return poplatky.ToArray();
    }

    // operations
    private static EasWrapper.MimoradnaOperace[] ToReqExceptionalOperations(this _OS.MortgageSimulationInputs inputs)
    {
        var operation = new EasWrapper.MimoradnaOperace
        {
            klic = 1,                   // Na tento parametr se mockuje fixně hodnota 1
            suma = inputs.LoanAmount,   // Na tento parametr se mapuje hodnota z Offer.SimulationInputs.LoanAmount,
            //valuta =                  // Na tento parametr se mapuje hodnota z SalesArrangement.ExpectedDateOfDrawing (pokud není null) a pokud je null, pak se mapuje Offer.SimulationInputs.ExpectedDateOfDrawing,
        };

        var expectedDateOfDrawing = (DateTime?)inputs.ExpectedDateOfDrawing;

        if (expectedDateOfDrawing.HasValue)
        {
            operation.valuta = expectedDateOfDrawing.Value;
        }

        return new EasWrapper.MimoradnaOperace[] { operation };
    }

    /// <summary>
    /// Converts Offer object [SimulationInputs] to EasSimulationHT object [SimulationHTRequest].
    /// </summary>
    public static EasWrapper.SimulationHTRequest ToEasSimulationRequest(this _OS.MortgageSimulationInputs inputs, _OS.BasicParameters basicParameters, Dictionary<int, DrawingDurationItem> drawingDurationsById, Dictionary<int, DrawingTypeItem> drawingTypeById)
    {
        //return SampleRequest;

        // request
        return new EasWrapper.SimulationHTRequest
        {
            settings = ReqDefaultSettings,
            uver = inputs.ToReqLoan(drawingDurationsById, drawingTypeById),
            urokovaSazba = inputs.ToReqInterestRate(),
            ucelyUveru = inputs.ToReqLoanPurposes(),
            nastaveniPoplatku = inputs.ToReqFeeSettings(basicParameters),
            marketingoveAkce = inputs.ToReqMarketingActions(),
            mimoradneOperace = inputs.ToReqExceptionalOperations(),
            poplatky = inputs.ToReqFees(),
        };
    }

    /// <summary>
    /// Modifies EasSimulationHT object [SimulationHTRequest] to Full Payment Schedule [FPS] request.
    /// </summary>
    public static EasWrapper.SimulationHTRequest ToEasSimulationFullPaymentScheduleRequest(this EasWrapper.SimulationHTRequest request)
    {
        request.settings = ReqFpsSettings;

        // request
        return request;
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
        mimoradneOperace = new EasWrapper.MimoradnaOperace[]
        {
            new EasWrapper.MimoradnaOperace
            {
                klic=1,                             // Na tento parametr se mockuje fixně hodnota 1
                valuta=new DateTime(2022,12,31),    // Na tento parametr se mapuje hodnota z SalesArrangement.ExpectedDateOfDrawing (pokud není null) a pokud je null, pak se mapuje Offer.SimulationInputs.ExpectedDateOfDrawing,
                suma=300000.00m,                    // Na tento parametr se mapuje hodnota z Offer.SimulationInputs.LoanAmount,
            }
        },
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
    private static _OS.MortgageSimulationResults AddResResults(this _OS.MortgageSimulationResults results, EasWrapper.UverVysledky res)
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
    private static _OS.MortgageSimulationResults AddResResults(this _OS.MortgageSimulationResults results, EasWrapper.UrokovaSazba res)
    {
        results.LoanInterestRate = res.urokovaSazba;
        results.LoanInterestRateAnnounced = res.vyhlasovana;
        results.LoanInterestRateAnnouncedType = res.vyhlasovanaTyp;

        return results;
    }


    // payment schedule (simple)
    private static _OS.AdditionalMortgageSimulationResults AddResResults(this _OS.AdditionalMortgageSimulationResults results, EasWrapper.SplS[] res)
    {
        var items = res.Select(i => new _OS.PaymentScheduleSimple
        {
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
    private static _OS.AdditionalMortgageSimulationResults AddResResults(this _OS.AdditionalMortgageSimulationResults results, EasWrapper.MarketingovaAkce[] res)
    {
        var items = res.Select(i => new _OS.ResultMarketingAction
        {
            Code = i.typMaAkce,
            Requested = i.zaskrtnuto,
            Applied = i.uplatnena,
            MarketingActionId = i.kodMaAkce,
            Deviation = i.odchylkaSazby,
            Name = i.nazev,
        });

        results.MarketingActions.AddRange(items);

        return results;
    }

    // fees
    private static _OS.AdditionalMortgageSimulationResults AddResResults(this _OS.AdditionalMortgageSimulationResults results, EasWrapper.Poplatek[] res)
    {
        var toResultFee = (EasWrapper.Poplatek i) =>
        {
            var fee = new _OS.ResultFee();
            fee.FeeId = i.kodSB;
            fee.DiscountPercentage = i.slevaIC;
            fee.TariffSum = i.sumaSazebnikova;
            fee.ComposedSum = i.sumaSkladackova;
            fee.FinalSum = i.sumaVysledna;
            fee.MarketingActionId = i.kodMaAkce;
            fee.Name = i.textAplikace;
            fee.ShortNameForExample = i.textDokumentaceVzorovyPriklad;
            fee.TariffName = i.textDokumentaceSazebnik ?? String.Empty;
            fee.UsageText = i.pouziti ?? String.Empty;
            fee.TariffTextWithAmount = i.textDokumentaceSazebnikHodnota ?? String.Empty;
            fee.CodeKB = i.kodKB ?? String.Empty;
            fee.DisplayAsFreeOfCharge = i.zobrazitZdarma.ToBool();
            fee.IncludeInRPSN = i.zapocitatRPSN.ToBool();
            fee.Periodicity = i.periodicita ?? String.Empty;
            fee.AccountDateFrom = i.uctovatOd;
            return fee;
        };

        var items = res.Select(i => toResultFee(i));

        results.Fees.AddRange(items);

        return results;
    }


    /// <summary>
    /// Converts EasSimulationHT object [SimulationHTResponse] to Offer object [SimulationResults].
    /// </summary>
    public static _OS.MortgageSimulationResults ToSimulationResults(this EasWrapper.SimulationHTResponse easSimulationResponse)
    {
        var results = new _OS.MortgageSimulationResults()
            .AddResResults(easSimulationResponse.uverVysledky)                  // loan
            .AddResResults(easSimulationResponse.urokovaSazba);                  // interest rate

        return results;
    }

    /// <summary>
    /// Converts EAS SplF object [SplF] to Full Payment Schedule item object [PaymentScheduleFull].
    /// </summary>
    private static _OS.PaymentScheduleFull? ToPaymentScheduleFull(this EasWrapper.SplF easSplF)
    {
        if (easSplF is null)
            return null;

        return new _OS.PaymentScheduleFull
        {
            PaymentIndex = easSplF.n,
            PaymentNumber = easSplF.c ?? String.Empty,
            Date = easSplF.d ?? String.Empty,
            Amount = easSplF.p ?? String.Empty,
            Principal = easSplF.j ?? String.Empty,
            Interest = easSplF.u ?? String.Empty,
            RemainingPrincipal = easSplF.z ?? String.Empty,
        };
    }


    /// <summary>
    /// Converts EasSimulationHT object [SimulationHTResponse] to PaymentScheduleFull array [PaymentScheduleFull].
    /// </summary>
    public static List<_OS.PaymentScheduleFull>? ToFullPaymentSchedule(this EasWrapper.SimulationHTResponse easSimulationResponse)
    {
        if (easSimulationResponse.splatkovyKalendarPlny is null)
            return null;

        return easSimulationResponse.splatkovyKalendarPlny.Select(i => i.ToPaymentScheduleFull()).ToList()!;
    }

    public static _OS.AdditionalMortgageSimulationResults ToAdditionalSimulationResults(this EasWrapper.SimulationHTResponse easSimulationResponse)
    {
        var results = new _OS.AdditionalMortgageSimulationResults()
            .AddResResults(easSimulationResponse.splatkovyKalendarJednoduchy)   // payment schedule (simple)
            .AddResResults(easSimulationResponse.marketingoveAkce)              // marketing actions
            .AddResResults(easSimulationResponse.poplatky);

        return results;
    }
    #endregion

    #region Shared

    private static bool ToBool(this int? value)
        => value.GetValueOrDefault(0) == 1;

    #endregion

}
