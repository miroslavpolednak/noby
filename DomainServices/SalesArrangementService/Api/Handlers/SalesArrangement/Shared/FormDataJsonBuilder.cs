using System.Text.Json;
using System.Linq;

using CIS.Infrastructure.gRPC.CisTypes;

using DomainServices.SalesArrangementService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.CustomerService.Contracts;


namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement.Shared
{
    public class FormDataJsonBuilder
    {

        #region Construction

        public FormData Data { get; init; }

        public FormDataJsonBuilder(FormData data)
        {
            Data = data;
        }

        #endregion

        //public string BuildJson(EFormType formType)
        //{
        //    string? json;

        //    switch (formType)
        //    {
        //        case EFormType.F3601:
        //            json = BuildJson3601();
        //            break;

        //        case EFormType.F3602:
        //            json = BuildJson3602();
        //            break;

        //        default:
        //            throw new CisArgumentException(99999, $"Form type #{formType} is not supported.", nameof(formType));  //TODO: ErrorCode
        //    }

        //    return json;

        //}

        //private static readonly Dictionary<EFormType, string[]> FormKeys = new Dictionary<EFormType, string[]>
        //{
        //    {EFormType.F3601,
        //        new string[] {
        //        "cislo_smlouvy",
        //        "case_id",
        //        "stav_zadosti",
        //        "business_case_ID",
        //        "risk_segment",
        //        "datum_uzavreni_obchodu",
        //        "kanal_ziskani",
        //        "datum_vytvoreni_zadosti",
        //        "datum_prvniho_podpisu",
        //        "uv_produkt",
        //        "uv_druh",
        //        "indikativni_LTV",
        //        "indikativni_LTC",
        //        "termin_cerpani_do",
        //        "seznam_mark_akci",
        //        "sazba_vyhlasovana",
        //        "sazba_skladacka",
        //        "sazba_poskytnuta",
        //        "vyhlasovanaTyp",
        //        "vyse_uveru",
        //        "anuitni_splatka",
        //        "kodZvyhodneni",
        //        "splatnost_uv_mesice",
        //        "fixace_uv_mesice",
        //        "individualni_cenotvorba_odchylka",
        //        "predp_termin_cerpani",
        //        "den_splaceni",
        //        "forma_splaceni",
        //        "seznam_poplatku",
        //        "seznam_ucelu",
        //        "seznam_objektu",
        //        "seznam_ucastniku",
        //        "zprostredkovano_3_stranou",
        //        "sjednal_CPM",
        //        "sjednal_ICP",
        //        "mena_prijmu",
        //        "mena_bydliste",
        //        "predp_hodnota_nem_zajisteni",
        //        "typ_cerpani",
        //        "datum_garance_us",
        //        "garance_us_platnost_do",
        //        "fin_kryti_vlastni_zdroje",
        //        "fin_kryti_cizi_zdroje",
        //        "fin_kryti_celkem",
        //        "zpusob_podpisu_smluv_dok",
        //        "seznam_domacnosti",
        //        }
        //    },

        //    {EFormType.F3602,
        //        new string[] {
        //        "cislo_smlouvy",
        //        //"case_id",
        //        //"stav_zadosti",
        //        //"business_case_ID",
        //        //"risk_segment",
        //        //"datum_uzavreni_obchodu",
        //        //"kanal_ziskani",
        //        //"datum_vytvoreni_zadosti",
        //        //"datum_prvniho_podpisu",
        //        //"uv_produkt",
        //        //"uv_druh",
        //        //"indikativni_LTV",
        //        //"indikativni_LTC",
        //        //"termin_cerpani_do",
        //        //"seznam_mark_akci",
        //        //"sazba_vyhlasovana",
        //        //"sazba_skladacka",
        //        //"sazba_poskytnuta",
        //        //"vyhlasovanaTyp",
        //        //"vyse_uveru",
        //        //"anuitni_splatka",
        //        //"kodZvyhodneni",
        //        //"splatnost_uv_mesice",
        //        //"fixace_uv_mesice",
        //        //"individualni_cenotvorba_odchylka",
        //        //"predp_termin_cerpani",
        //        //"den_splaceni",
        //        //"forma_splaceni",
        //        //"seznam_poplatku",
        //        //"seznam_ucelu",
        //        //"seznam_objektu",
        //        //"seznam_ucastniku",
        //        //"zprostredkovano_3_stranou",
        //        //"sjednal_CPM",
        //        //"sjednal_ICP",
        //        //"mena_prijmu",
        //        //"mena_bydliste",
        //        //"predp_hodnota_nem_zajisteni",
        //        //"typ_cerpani",
        //        //"datum_garance_us",
        //        //"garance_us_platnost_do",
        //        //"fin_kryti_vlastni_zdroje",
        //        //"fin_kryti_cizi_zdroje",
        //        //"fin_kryti_celkem",
        //        //"zpusob_podpisu_smluv_dok",
        //        //"seznam_domacnosti",
        //        }
        //    },

        //};

        //object? GetRootValue(string key)
        //{
        //    if (!FormKeys[formType].Contains(key))
        //    {
        //        return null;
        //    }

        //    object? value = null;

        //    switch (key)
        //    {

        //        case "cislo_smlouvy": value = null; break; = Data.Arrangement.ContractNumber,
        //    case "case_id": value = null; break; // = Data.Arrangement.CaseId.ToJsonString(),
        //        case "stav_zadosti": value = null; break; // = Data.SalesArrangementStatesById[Data.Arrangement.State].StarbuildId.ToJsonString(),
        //        case "business_case_ID": value = null; break; // = Data.Arrangement.RiskBusinessCaseId,                                                                 // SalesArrangement
        //        case "risk_segment": value = null; break; // = Data.Arrangement.RiskSegment,
        //        case "datum_uzavreni_obchodu": value = null; break; // = riskBusinessCaseExpirationDate.ToJsonString(),                                                 // Default: CurrentDate + 90 dní
        //        case "kanal_ziskani": value = null; break; // = Data.Arrangement.ChannelId.ToJsonString(),                                                              // SalesArrangement - vyplněno na základě usera
        //        case "datum_vytvoreni_zadosti": value = null; break; // = actualDate.ToJsonString(),                                                                    // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
        //        case "datum_prvniho_podpisu": value = null; break; // = firstSignedDate.ToJsonString(),                                                                      // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum

        //        //uv_produkt": value = null; break; // = Data.Offer.ProductTypeId.ToJsonString(),                                                                      // ??? SalesArrangement nemá být z OfferProductTypeId ???
        //        case "uv_produkt": value = null; break; // = Data.ProductType.Id.ToJsonString(),
        //        case "uv_druh": value = null; break; // = Data.Offer.SimulationInputs.LoanKindId.ToJsonString(),                                                             // OfferInstance
        //        case "indikativni_LTV": value = null; break; // = Data.Offer.SimulationResults.LoanToValue.ToJsonString(),                                                   // OfferInstance
        //        case "indikativni_LTC": value = null; break; // = Data.Arrangement.LoanToCost.ToJsonString(),                                                                // OfferInstance -> SalesArrangement !!! moved from offer to arrangement in D1-2
        //        case "termin_cerpani_do": value = null; break; // = ((DateTime)Data.Offer.SimulationResults.DrawingDateTo).ToJsonString(),
        //        case "seznam_mark_akci": value = null; break; // = Array.Empty<object>(),                                                                               // [MOCK] OfferInstance (default empty array)
        //        case "sazba_vyhlasovana": value = null; break; // = Data.Offer.SimulationResults.LoanInterestRateAnnounced.ToJsonString(),                                   // OfferInstance
        //        case "sazba_skladacka": value = null; break; // = Data.Offer.SimulationResults.LoanInterestRate.ToJsonString(),                                              // OfferInstance
        //        case "sazba_poskytnuta": value = null; break; // = Data.Offer.SimulationResults.LoanInterestRate.ToJsonString(),                                             // OfferInstance": value = null; break; // = sazba_skladacka
        //        case "vyhlasovanaTyp": value = null; break; // = Data.Offer.SimulationResults.LoanInterestRateAnnouncedType.ToJsonString(),                                  // OfferInstance 
        //        case "vyse_uveru": value = null; break; // = Data.Offer.SimulationResults.LoanAmount.ToJsonString(),                                                         // OfferInstance
        //        case "anuitni_splatka": value = null; break; // = Data.Offer.SimulationResults.LoanPaymentAmount.ToJsonString(),                                             // OfferInstance
        //        case "kodZvyhodneni": value = null; break; // = Data.Offer.SimulationResults.EmployeeBonusLoanCode.ToJsonString(),                                           // OfferInstance
        //        case "splatnost_uv_mesice": value = null; break; // = Data.Offer.SimulationResults.LoanDuration.ToJsonString(),                                              // OfferInstance (kombinace dvou vstupů roky + měsíce na FE)
        //        case "fixace_uv_mesice":
        //        //case "value" value = null; break; // = Data.Offer.SimulationInputs.FixedRatePeriod.ToJsonString(),                                               // OfferInstance - na FE je to v rocích a je to číselník ?
        //            case "individualni_cenotvorba_odchylka": value = null; break; // = Data.Offer.SimulationInputs.InterestRateDiscount.ToJsonString(),
        //        case "predp_termin_cerpani": value = null; break; // = Data.Arrangement.Mortgage?.ExpectedDateOfDrawing.ToJsonString(),                                      // SalesArrangement 
        //        case "den_splaceni": value = null; break; // = Data.Offer.SimulationInputs.PaymentDay.ToJsonString(),                                                        // OfferInstance
        //        case "forma_splaceni": value = null; break; // = 1.ToJsonString(),                                                                                      // [MOCK] OfferInstance (default 1)  
        //        case "seznam_poplatku": value = null; break; // = Data.Offer.SimulationResults.Fees?.Select(i": value = null; break; // => MapFee(i)).ToArray() ?? Array.Empty<object>(),              // Data.Offer.SimulationResults.Fees
        //        case "seznam_ucelu": value = null; break; // = Data.Offer.SimulationInputs.LoanPurposes?.Select(i": value = null; break; // => MapLoanPurpose(i)).ToArray() ?? Array.Empty<object>(),  // OfferInstance - 1..5 ??? má se brát jen prvních 5 účelů ?
        //        case "seznam_objektu": value = null; break; // = Data.Arrangement.Mortgage?.LoanRealEstates.ToList().Select((i, index)": value = null; break; // => MapLoanRealEstate(i, index + 1)).ToArray() ?? Array.Empty<object>(), // SalesArrangement - 0..3 ???
        //        case "seznam_ucastniku": value = null; break; // = Data.CustomersOnSa?.Select(i": value = null; break; // => MapCustomerOnSA(i)).ToArray() ?? Array.Empty<object>(),                   // CustomerOnSA, Customer
        //        case "zprostredkovano_3_stranou": value = null; break; // = false.ToJsonString(),                                                                       // [MOCK] SalesArrangement - dle typu Usera (na offer zatím nemáme, dohodnuta mockovaná hodnota FALSE)
        //        case "sjednal_CPM": value = null; break; // = Data.User!.CPM,                                                                                                // User
        //        case "sjednal_ICP": value = null; break; // = Data.User!.ICP,                                                                                                // User
        //                                                 // VIP_makler": value = null; break; // = 0.ToJsonString(),                                                                                       // [MOCK] User (default 0) !!! removed in D1-2
        //        case "mena_prijmu": value = null; break; // = Data.Arrangement.Mortgage?.IncomeCurrencyCode,                                                                 // SalesArrangement
        //        case "mena_bydliste": value = null; break; // = Data.Arrangement.Mortgage?.ResidencyCurrencyCode,                                                            // SalesArrangement

        //        // zpusob_zasilani_vypisu": value = null; break; // = Data.Offer.SimulationResults.StatementTypeId.ToJsonString(),                                     // Offerinstance !!! removed in D1-2 (moved to fees without DV mapping)
        //        case "predp_hodnota_nem_zajisteni": value = null; break; // = Data.Offer.SimulationInputs.CollateralAmount.ToJsonString(),                                   // Offerinstance
        //        case "typ_cerpani": value = null; break; // = Data.Offer.SimulationInputs.DrawingType.ToJsonString(),
        //        case "datum_garance_us": value = null; break; // = Data.Arrangement.OfferGuaranteeDateFrom.ToJsonString(),
        //        case "garance_us_platnost_do": value = null; break; // = Data.Offer.BasicParameters.GuaranteeDateTo.ToJsonString(),                                          // Data.Offer.BasicParameters.GuaranteeDateTo
        //        case "fin_kryti_vlastni_zdroje": value = null; break; // = Data.Offer.BasicParameters.FinancialResourcesOwn.ToJsonString(),                                  // OfferInstance
        //        case "fin_kryti_cizi_zdroje": value = null; break; // = Data.Offer.BasicParameters.FinancialResourcesOther.ToJsonString(),                                   // OfferInstance
        //        case "fin_kryti_celkem": value = null; break; // = financialResourcesTotal.ToJsonString(),                                                              // OfferInstance
        //        case "zpusob_podpisu_smluv_dok": value = null; break; // = Data.Arrangement.Mortgage?.SignatureTypeId.ToJsonString(),                                        // SalesArrangement
        //        case "seznam_domacnosti": value = null; break; // = Data.Households?.Select(i": value = null; break; // => MapHousehold(i)).ToArray() ?? Array.Empty<object>(),

        //    };

        //    return value;
        //};


        public static readonly string MockDokumentId = "9876543210"; // TODO: dočasný mock - odstranit až si to Assecco odladí

        public string BuildJson(EFormType formType, bool ignoreNullValues = true)
        {
            var actualDate = DateTime.Now.Date;

            var householdsByCustomerOnSAId = Data.CustomersOnSa.ToDictionary(i => i.CustomerOnSAId, i => Data.Households.Where(h => h.CustomerOnSAId1 == i.CustomerOnSAId || h.CustomerOnSAId2 == i.CustomerOnSAId).ToArray());
            var firstEmploymentType = Data.EmploymentTypes.OrderBy(i => i.Id).FirstOrDefault();

            int[] idsObligationTypeLoan = new int[] { 1, 2, 5 };  // Hypotéční nebo spotřebitelský úvěr
            int[] idsObligationTypeCredit = new int[] { 3, 4 };   // Kreditní karta nebo povolený debet

            object? MapHousehold(Contracts.Household i)
            {
                if (i == null)
                {
                    return null;
                }

                var householdTypeId = i.HouseholdTypeId;
                if (formType == EFormType.F3602)
                {
                    householdTypeId = 2;
                }

                return new
                {
                    cislo_domacnosti = i.HouseholdTypeId.ToJsonString(),
                    role_domacnosti = householdTypeId.ToJsonString(),
                    //pocet_deti_0_10let = i.Data.ChildrenUpToTenYearsCount.ToJsonString(),
                    //pocet_deti_nad_10let = i.Data.ChildrenOverTenYearsCount.ToJsonString(),
                    pocet_deti_0_10let = (i.Data.ChildrenUpToTenYearsCount ?? 0).ToJsonString(),
                    pocet_deti_nad_10let = (i.Data.ChildrenOverTenYearsCount ?? 0).ToJsonString(),
                    sporeni = i.Expenses.SavingExpenseAmount.ToJsonString(),
                    pojisteni = i.Expenses.InsuranceExpenseAmount.ToJsonString(),
                    naklady_na_bydleni = i.Expenses.HousingExpenseAmount.ToJsonString(),
                    ostatni_vydaje = i.Expenses.OtherExpenseAmount.ToJsonString(),
                    vyporadani_majetku = i.Data.PropertySettlementId.ToJsonString(),
                    manzel_pristupuje_k_dluhu = i.Data.AreBothPartnersDeptors.ToJsonString(),
                    druh_druzka = i.Data.AreCustomersPartners.ToJsonString(),
                };
            }

            object? MapLoanPurpose(LoanPurpose i)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    uv_ucel = i.LoanPurposeId.ToJsonString(),
                    uv_ucel_suma = i.Sum.ToJsonString(),
                };
            }

            object? MapFee(ResultFee i)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    kod_poplatku = i.FeeId.ToJsonString(),
                    suma_poplatku_sazebnik = i.TariffSum.ToJsonString(),
                    suma_poplatku_skladacka = i.ComposedSum.ToJsonString(),
                    suma_poplatku_vysledna = i.FinalSum.ToJsonString(),
                };
            }

            object? MapMarketingAction(ResultMarketingAction i)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    typMaAkce = i.Code,
                    zaskrtnuto = i.Requested.ToJsonString(),
                    uplatnena = i.Applied.ToJsonString(),
                    kodMaAkce = i.MarketingActionId.ToJsonString(),
                    odchylkaSazby = i.Deviation.ToJsonString(),
                };
            }

            object? MapLoanRealEstate(SalesArrangementParametersMortgage.Types.LoanRealEstate i, int rowNumber)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    cislo_objektu_uveru = rowNumber.ToJsonString(),
                    typ_nemovitosti = i.RealEstateTypeId.ToJsonString(),
                    objekt_uv_je_zajisteni = i.IsCollateral.ToJsonString(),
                    ucel_porizeni = i.RealEstatePurchaseTypeId.ToJsonString(),
                };
            }

            object? MapAddress(GrpcAddress i)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    typ_adresy = i.AddressTypeId.ToJsonString(),
                    ulice = i.Street,
                    cislo_popisne = i.BuildingIdentificationNumber,
                    cislo_orientacni = i.LandRegistryNumber,
                    ulice_dodatek = i.DeliveryDetails,
                    psc = i.Postcode.ToPostCodeJsonString(),
                    misto = i.City,
                    stat = i.CountryId.ToJsonString(),
                };
            }

            object? MapIdentificationDocument(IdentificationDocument i)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    cislo_dokladu = i.Number,
                    typ_dokladu = i.IdentificationDocumentTypeId.ToJsonString(),
                    vydal = i.IssuedBy,
                    vydal_datum = i.IssuedOn.ToJsonString(),
                    vydal_stat = i.IssuingCountryId.ToJsonString(),
                    platnost_do = i.ValidTo.ToJsonString(),

                };
            }

            object? MapContact(Contact i)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    typ_kontaktu = i.ContactTypeId.ToJsonString(),
                    hodnota_kontaktu = i.Value,
                };
            }

            object? MapCustomerObligation(Obligation i, int rowNumber)
            {
                if (i == null)
                {
                    return null;
                }

                decimal? vyseKorekceZavazkuJistina = null;

                if (idsObligationTypeLoan. Contains(i.ObligationTypeId ?? 0))
                {
                    vyseKorekceZavazkuJistina = i.Correction?.LoanPrincipalAmountCorrection;
                }

                if (idsObligationTypeCredit.Contains(i.ObligationTypeId ?? 0))
                {
                    vyseKorekceZavazkuJistina = i.Correction?.CreditCardLimitCorrection;
                }

                return new
                {
                    cislo_zavazku = rowNumber.ToJsonString(),
                    druh_zavazku = i.ObligationTypeId.ToJsonString(),
                    vyse_splatky = i.InstallmentAmount.ToJsonString(),
                    vyse_nesplacene_jistiny = i.LoanPrincipalAmount.ToJsonString(),
                    vyse_limitu = i.CreditCardLimit.ToJsonString(),
                    veritel_kod_banky = i.Creditor?.CreditorId,
                    veritel_nazev = i.Creditor?.Name,
                    mimo_entitu_mandanta = i.Creditor?.IsExternal?.ToJsonString(),
                    zpusob_korekce_zavazku = i.Correction?.CorrectionTypeId?.ToJsonString(),
                    vyse_korekce_zavazku_o_spl = i.Correction?.InstallmentAmountCorrection?.ToJsonString(),
                    vyse_korekce_zavazku_o_jistina = vyseKorekceZavazkuJistina.ToJsonString(),
                    vyse_konsolid_jistiny = i.LoanPrincipalAmountConsolidated?.ToJsonString(),
                };
            }

            object? MapCustomerIncome(IncomeInList iil, int rowNumber)
            {
                if (iil == null)
                {
                    return null;
                }

                string? GetAddressNumber(GrpcAddress? address)
                {
                    if (address == null)
                    {
                        return null;
                    }

                    //složit string ve formátu "BuildingIdentificationNumber/LandRegistryNumber"
                    var parts = new string?[] { address.BuildingIdentificationNumber, address.LandRegistryNumber };

                    var number = String.Join("/", parts.Where(i => !string.IsNullOrEmpty(i)));

                    return String.IsNullOrEmpty(number) ? null : number;
                }

                var i = Data.IncomesById[iil.IncomeId];

                var employmentTypeId = i.Employement?.Job?.EmploymentTypeId ?? firstEmploymentType?.Id;
                var countryId = i.Employement?.Employer?.CountryId;

                return new
                {
                    prvni_pracovni_sml_od = i.Employement?.Job?.FirstWorkContractSince.ToJsonString(),
                    poradi_prijmu = rowNumber.ToJsonString(),
                    zdroj_prijmu_hlavni = iil.IncomeTypeId.ToJsonString(),
                    typ_pracovniho_pomeru = employmentTypeId.ToJsonString(),
                    klient_ve_vypovedni_lhute = i.Employement?.Job?.JobNoticePeriod.ToJsonString(),                 // Pokud je parametr null, mapujeme 0
                    klient_ve_zkusebni_lhute = i.Employement?.Job?.JobTrialPeriod.ToJsonString(),                   // Pokud je parametr null, mapujeme 0
                    prijem_ze_zahranici = countryId.HasValue ? (countryId.Value != 16).ToJsonString() : null,       // Pokud Employer.Address.CountryId = 16 mapujeme 0(= ne), v ostatních případech 1(= ano)
                    domicilace_prijmu_ze_zamestnani = 0.ToJsonString(),
                    typ_dokumentu = 1.ToJsonString(),                                                               // Pro příjem ze zaměstnání default "1"
                    pracovni_smlouva_aktualni_od = i.Employement?.Job?.CurrentWorkContractSince.ToJsonString(),
                    pracovni_smlouva_aktualni_do = i.Employement?.Job?.CurrentWorkContractTo.ToJsonString(),
                    hruby_rocny_prijem = i.Employement?.Job?.GrossAnnualIncome.ToJsonString(),
                    zamestnavatel_nazov = i.Employement?.Employer?.Name,
                    zamestnavatel_rc_ico = new List<string> { i.Employement?.Employer?.Cin, i.Employement?.Employer?.BirthNumber }.FirstOrDefault(i => !String.IsNullOrEmpty(i)),   // pouze jedna z hodnot, neměly by být zadány obě
                    //zamestnavatel_sidlo_ulice = i.Employement?.Employer?.Address?.Street,
                    //zamestnavatel_sidlo_cislo_popisne_orientacni = GetAddressNumber(i.Employement?.Employer?.Address),  //složit string ve formátu "BuildingIdentificationNumber/LandRegistryNumber"
                    //zamestnavatel_sidlo_mesto = i.Employement?.Employer?.Address?.City,
                    //zamestnavatel_sidlo_psc = i.Employement?.Employer?.Address?.Postcode.ToPostCodeJsonString(),
                    zamestnavatel_sidlo_stat = i.Employement?.Employer?.CountryId?.ToJsonString(),
                    //zamestnavatel_telefonni_cislo = i.Employement?.Employer?.PhoneNumber,
                    //zamestnavatel_okec = i.Employement?.Employer?.ClassificationOfEconomicActivitiesId.ToJsonString(),
                    zamestnavatel_pracovni_sektor = 5.ToJsonString(),          // default pro Drop1-2, v Drop1-3 bude odstraněno
                    zamestnavatel_senzitivni_sektor = 0.ToJsonString(),        // default pro Drop1-2, v Drop1-3 bude odstraněno
                    //povolani = i.Employement?.Job?.JobType.ToJsonString(),
                    zamestnan_jako = i.Employement?.Job?.JobDescription,
                    prijem_vyse = iil.Sum.ToJsonString(),
                    prijem_mena = iil.CurrencyCode,
                    zrazky_ze_mzdy_rozhodnuti = i.Employement?.WageDeduction?.DeductionDecision.ToJsonString(),
                    zrazky_ze_mzdy_splatky = i.Employement?.WageDeduction?.DeductionPayments.ToJsonString(),
                    zrazky_ze_mzdy_ostatni = i.Employement?.WageDeduction?.DeductionOther.ToJsonString(),
                    prijem_potvrzeni_vystavila_ext_firma = i.Employement?.IncomeConfirmation?.ConfirmationByCompany.ToJsonString(),
                    //prijem_potvrzeni_misto_vystaveni =  i.Employement?.IncomeConfirmation?.ConfirmationPlace,
                    prijem_potvrzeni_datum = i.Employement?.IncomeConfirmation?.ConfirmationDate.ToJsonString(),
                    prijem_potvrzeni_osoba = i.Employement?.IncomeConfirmation?.ConfirmationPerson,
                    prijem_potvrzeni_kontakt = i.Employement?.IncomeConfirmation?.ConfirmationContact,
                };
            }

            object? MapCustomer(Contracts.CustomerOnSA i)
            {
                if (i == null)
                {
                    return null;
                }

                // do JSON věty jdou pouze Customers s Kb identitou
                var identityKb = i.CustomerIdentifiers.Single(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
                var identityMp = i.CustomerIdentifiers.Single(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp); // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!
                var c = Data.CustomersByIdentityCode[identityKb.ToCode()];

                var cIdentificationDocument = MapIdentificationDocument(c.IdentificationDocument);
                var cIdentificationDocuments = (cIdentificationDocument == null) ? Array.Empty<object>() : new object[1] { cIdentificationDocument };
                var cCitizenshipCountriesId = c.NaturalPerson?.CitizenshipCountriesId?.ToList().FirstOrDefault();
                var cDegreeBeforeId = c.NaturalPerson?.DegreeBeforeId;
                var cGenderId = c.NaturalPerson?.GenderId;

                #region Fake

                // ----------------------------------------------------------------------------------------------------------------------------------
                // Fake for Drop1-2
                // ----------------------------------------------------------------------------------------------------------------------------------
                object? MapCustomerF3602()
                {
                    return new
                    {
                        rodne_cislo = "5458083246",

                        kb_id = 703274075.ToJsonString(),
                        mp_id = 200121760.ToJsonString(),                                   // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!
                                                                                            // spolecnost_KB =                                                              // Customer - pokud načteme z CM ??? OP! 

                        titul_pred = cDegreeBeforeId.HasValue ? Data.AcademicDegreesBeforeById[cDegreeBeforeId.Value].Name : null,    // (použít Name, nikoliv jen Id) 
                                                                                                                                      // titul_za = c.NaturalPerson?.DegreeAfterId,                                    // ??? (použít Name, nikoliv jen Id), ve vzorovém JSONu ani není - neposílat
                        prijmeni_nazev = "Pavlíková",
                        prijmeni_rodne = c.NaturalPerson?.BirthName,
                        jmeno = "Ivana",
                        datum_narozeni = (new DateTime(1954, 8, 8)).ToJsonString(),
                        misto_narozeni_obec = c.NaturalPerson?.PlaceOfBirth,
                        misto_narozeni_stat = c.NaturalPerson?.BirthCountryId.ToJsonString(),
                        pohlavi = "Z",
                        statni_prislusnost = 16.ToJsonString(),                    // vzít první
                        zamestnanec = 0.ToJsonString(),                                                 // [MOCK] OfferInstance (default 0)
                        rezident = 0.ToJsonString(),                                                    // [MOCK] OfferInstance (default 0)
                        PEP = c.NaturalPerson?.IsPoliticallyExposed.ToJsonString(),
                        seznam_adres = c.Addresses?.Select(i => MapAddress(i)).ToArray() ?? Array.Empty<object>(),
                        seznam_dokladu = cIdentificationDocuments,                                      // ??? mělo by to být pole, nikoliv jeden objekt ???
                        seznam_kontaktu = c.Contacts?.Select(i => MapContact(i)).ToArray() ?? Array.Empty<object>(),
                        rodinny_stav = c.NaturalPerson?.MaritalStatusStateId.ToJsonString(),
                        //druh_druzka = (i.HasPartner ?? false).ToJsonString(),
                        vzdelani = c.NaturalPerson?.EducationLevelId.ToJsonString(),
                        seznam_prijmu = i.Incomes?.ToList().Select((i, index) => MapCustomerIncome(i, index + 1)).ToArray() ?? Array.Empty<object>(),
                        seznam_zavazku = i.Obligations?.ToList().Select((i, index) => MapCustomerObligation(i, index + 1)).ToArray() ?? Array.Empty<object>(),
                        prijem_sbiran = 0.ToJsonString(),                                               // [MOCK] (default 0) out of scope
                        uzamcene_prijmy = false.ToJsonString(),                                         // [MOCK] (default 0) jinak z c.LockedIncomeDateTime.HasValue.ToJsonString(),
                                                                                                        // datum_posledniho_uzam_prijmu = c.LockedIncomeDateTime.ToJsonString(),        // ??? chybí implementace!
                    };
                }

                if (formType == EFormType.F3602)
                {
                    return MapCustomerF3602();
                }
                // ----------------------------------------------------------------------------------------------------------------------------------

                #endregion

                return new
                {
                    rodne_cislo = c.NaturalPerson?.BirthNumber,

                    kb_id = identityKb.IdentityId.ToJsonString(),
                    mp_id = identityMp.IdentityId.ToJsonString(),                                   // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!
                    // spolecnost_KB =                                                              // Customer - pokud načteme z CM ??? OP! 

                    titul_pred = cDegreeBeforeId.HasValue ? Data.AcademicDegreesBeforeById[cDegreeBeforeId.Value].Name : null,    // (použít Name, nikoliv jen Id) 
                    // titul_za = c.NaturalPerson?.DegreeAfterId,                                    // ??? (použít Name, nikoliv jen Id), ve vzorovém JSONu ani není - neposílat
                    prijmeni_nazev = c.NaturalPerson?.LastName,
                    prijmeni_rodne = c.NaturalPerson?.BirthName,
                    jmeno = c.NaturalPerson?.FirstName,
                    datum_narozeni = c.NaturalPerson?.DateOfBirth.ToJsonString(),
                    misto_narozeni_obec = c.NaturalPerson?.PlaceOfBirth,
                    misto_narozeni_stat = c.NaturalPerson?.BirthCountryId.ToJsonString(),
                    pohlavi = cGenderId.HasValue ? Data.GendersById[cGenderId.Value].StarBuildJsonCode : null,
                    statni_prislusnost = cCitizenshipCountriesId.ToJsonString(),                    // vzít první
                    zamestnanec = 0.ToJsonString(),                                                 // [MOCK] OfferInstance (default 0)
                    rezident = 0.ToJsonString(),                                                    // [MOCK] OfferInstance (default 0)
                    PEP = c.NaturalPerson?.IsPoliticallyExposed.ToJsonString(),
                    seznam_adres = c.Addresses?.Select(i => MapAddress(i)).ToArray() ?? Array.Empty<object>(),
                    seznam_dokladu = cIdentificationDocuments,                                      // ??? mělo by to být pole, nikoliv jeden objekt ???
                    seznam_kontaktu = c.Contacts?.Select(i => MapContact(i)).ToArray() ?? Array.Empty<object>(),
                    rodinny_stav = c.NaturalPerson?.MaritalStatusStateId.ToJsonString(),
                    //druh_druzka = (i.HasPartner ?? false).ToJsonString(),
                    vzdelani = c.NaturalPerson?.EducationLevelId.ToJsonString(),
                    seznam_prijmu = i.Incomes?.ToList().Select((i, index) => MapCustomerIncome(i, index + 1)).ToArray() ?? Array.Empty<object>(),
                    seznam_zavazku = i.Obligations?.ToList().Select((i, index) => MapCustomerObligation(i, index + 1)).ToArray() ?? Array.Empty<object>(),
                    prijem_sbiran = 0.ToJsonString(),                                               // [MOCK] (default 0) out of scope
                    uzamcene_prijmy = false.ToJsonString(),                                         // [MOCK] (default 0) jinak z c.LockedIncomeDateTime.HasValue.ToJsonString(),
                                                                                                    // datum_posledniho_uzam_prijmu = c.LockedIncomeDateTime.ToJsonString(),        // ??? chybí implementace!
                };
            }

            object? MapCustomerOnSA(Contracts.CustomerOnSA i)
            {
                if (i == null)
                {
                    return null;
                }

                var roleId = i.CustomerRoleId;
                if (formType == EFormType.F3602)
                {
                    roleId = 2;
                }

                var household = householdsByCustomerOnSAId![i.CustomerOnSAId].First();  // ??? co když je stejné CustomerOnSAId ve vícero households
                return new
                {
                    role = roleId.ToJsonString(),                     // CustomerOnSA
                    zmocnenec = (i.CustomerOnSAId == Data.Arrangement.Mortgage?.Agent).ToJsonString(),   // zmocnenec = True pro customera,jehoz CustomerOnSaId je rovno hodnotě parametru Agent a false pro všechny ostatní případy.
                    cislo_domacnosti = household.HouseholdTypeId.ToJsonString(),// CustomerOnSA ??? brát Houshold.CustomerOnSAId (1 nebo 2)
                    klient = MapCustomer(i),
                };
            }

            long? FindZmocnenecMpId()
            {
                //zmocnenec_mp_id, Pozn.: Přemapování musí proběhnout z CustomerOnSaId na PartnerId, což je ID, kde Identitní schéma je MPID.
                // 1) Z SA vezmu CustomerOnSAId (Data.Arrangement.Mortgage?.Agent)
                // 2) Najdu si CustomerOnSA odpovídající tomuto CustomerOnSAId
                // 3) Vezmu MP identitu z toho customera a z ní IdentityId

                var agent = Data.Arrangement.Mortgage?.Agent;

                var customer = agent.HasValue ? Data.CustomersOnSa?.FirstOrDefault(c => c.CustomerOnSAId == agent.Value) : null;

                var identityMp = customer?.CustomerIdentifiers.SingleOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp);

                return identityMp?.IdentityId;
            }

            // root
            var loanAmount = (decimal)Data.Offer.SimulationResults.LoanAmount;
            var financialResourcesOwn = Data.Offer.BasicParameters.FinancialResourcesOwn.ToDecimal() ?? 0;
            var financialResourcesOther = Data.Offer.BasicParameters.FinancialResourcesOther.ToDecimal() ?? 0;
            var financialResourcesTotal = (loanAmount + financialResourcesOwn + financialResourcesOther);

            DateTime riskBusinessCaseExpirationDate = (Data.Arrangement.RiskBusinessCaseExpirationDate is not null) ? (DateTime)Data.Arrangement.RiskBusinessCaseExpirationDate! : actualDate.AddDays(90).Date;
            DateTime firstSignedDate = (Data.Arrangement.FirstSignedDate is not null) ? (DateTime)Data.Arrangement.FirstSignedDate! : actualDate;
            var seznamIdFormulare = new object[] { new { id_formulare = 0.ToJsonString() } };

            var user_cpm = "99806569";
            var user_icp = "114306569";

            object data = new { };

            switch (formType)
            {
                case EFormType.F3601:

                    var developer = Data.Offer.SimulationInputs.Developer;
                    var developerDescription = (developer == null) ? null : String.Join(",", (new List<string> { developer.NewDeveloperName ?? String.Empty, developer.NewDeveloperCin ?? String.Empty, developer.NewDeveloperProjectName ?? String.Empty }));

                    var insuranceSumRiskLife = Data.Offer.SimulationInputs.RiskLifeInsurance == null ? (decimal?)null : (decimal)Data.Offer.SimulationInputs.RiskLifeInsurance.Sum;
                    var insuranceSumRealEstate = Data.Offer.SimulationInputs.RealEstateInsurance == null ? (decimal?)null : (decimal)Data.Offer.SimulationInputs.RealEstateInsurance.Sum;

                    var typCerpani = Data.Offer.SimulationInputs.DrawingType.HasValue ? Data.DrawingTypeById.GetValueOrDefault(Data.Offer.SimulationInputs.DrawingType.Value)?.StarbuildId : null;

                    data = new
                    {
                        cislo_smlouvy = Data.Arrangement.ContractNumber,
                        case_id = Data.Arrangement.CaseId.ToJsonString(),
                        stav_zadosti = Data.SalesArrangementStatesById[Data.Arrangement.State].StarbuildId.ToJsonString(),
                        business_case_ID = Data.Arrangement.RiskBusinessCaseId,                                                                 // SalesArrangement
                        risk_segment = Data.Arrangement.RiskSegment,
                        laa_id = Data.Arrangement.LoanApplicationAssessmentId,
                        datum_uzavreni_obchodu = Data.Arrangement.RiskBusinessCaseExpirationDate?.ToJsonString(),
                        kanal_ziskani = Data.Arrangement.ChannelId.ToJsonString(),                                                              // SalesArrangement - vyplněno na základě usera
                        datum_vytvoreni_zadosti = actualDate.ToJsonString(),                                                                    // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
                        datum_prvniho_podpisu = firstSignedDate.ToJsonString(),                                                                      // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
                        uv_produkt = Data.ProductType.Id.ToJsonString(),
                        uv_druh = Data.Offer.SimulationInputs.LoanKindId.ToJsonString(),                                                             // OfferInstance
                        indikativni_LTV = Data.Offer.SimulationResults.LoanToValue.ToJsonString(),                                                   // OfferInstance
                        //indikativni_LTC = Data.Arrangement.LoanToCost.ToJsonString(),                                                                // OfferInstance -> SalesArrangement !!! moved from offer to arrangement in D1-2
                        termin_cerpani_do = ((DateTime)Data.Offer.SimulationResults.DrawingDateTo).ToJsonString(),
                        sazba_vyhlasovana = Data.Offer.SimulationResults.LoanInterestRateAnnounced.ToJsonString(),                                   // OfferInstance
                        sazba_skladacka = Data.Offer.SimulationResults.LoanInterestRate.ToJsonString(),                                              // OfferInstance
                        sazba_poskytnuta = Data.Offer.SimulationResults.LoanInterestRate.ToJsonString(),                                             // OfferInstance = sazba_skladacka
                        vyhlasovanaTyp = Data.Offer.SimulationResults.LoanInterestRateAnnouncedType.ToJsonString(),                                  // OfferInstance 
                        vyse_uveru = Data.Offer.SimulationResults.LoanAmount.ToJsonString(),                                                         // OfferInstance
                        anuitni_splatka = Data.Offer.SimulationResults.LoanPaymentAmount.ToJsonString(),                                             // OfferInstance
                        uv_zvyhodneni = Data.Offer.SimulationResults.EmployeeBonusLoanCode.ToJsonString(),                                           // OfferInstance
                        splatnost_uv_mesice = Data.Offer.SimulationResults.LoanDuration.ToJsonString(),                                              // OfferInstance (kombinace dvou vstupů roky + měsíce na FE)
                        fixace_uv_mesice = Data.Offer.SimulationInputs.FixedRatePeriod.ToJsonString(),                                               // OfferInstance - na FE je to v rocích a je to číselník ?
                        individualni_cenotvorba_odchylka = Data.Offer.SimulationInputs.InterestRateDiscount.ToJsonString(),
                        predp_termin_cerpani = Data.Arrangement.Mortgage?.ExpectedDateOfDrawing.ToJsonString(),                                      // SalesArrangement 
                        den_splaceni = Data.Offer.SimulationInputs.PaymentDay.ToJsonString(),                                                        // OfferInstance
                        developer_id = Data.Offer.SimulationInputs.Developer?.DeveloperId.ToJsonString(),
                        developer_projekt_id = Data.Offer.SimulationInputs.Developer?.ProjectId.ToJsonString(),
                        developer_popis = developerDescription,
                        forma_splaceni = 1.ToJsonString(),                                                                                      // [MOCK] OfferInstance (default 1)  
                        seznam_mark_akci = Data.Offer.AdditionalSimulationResults.MarketingActions?.Select(i => MapMarketingAction(i)).ToArray() ?? Array.Empty<object>(),
                        seznam_poplatku = Data.Offer.AdditionalSimulationResults.Fees?.Select(i => MapFee(i)).ToArray() ?? Array.Empty<object>(),              // Data.Offer.SimulationResults.Fees
                        seznam_ucelu = Data.Offer.SimulationInputs.LoanPurposes?.Select(i => MapLoanPurpose(i)).ToArray() ?? Array.Empty<object>(),  // OfferInstance - 1..5 ??? má se brát jen prvních 5 účelů ?
                        seznam_objektu = Data.Arrangement.Mortgage?.LoanRealEstates.ToList().Select((i, index) => MapLoanRealEstate(i, index + 1)).ToArray() ?? Array.Empty<object>(), // SalesArrangement - 0..3 ???
                        seznam_ucastniku = Data.CustomersOnSa?.Select(i => MapCustomerOnSA(i)).ToArray() ?? Array.Empty<object>(),                   // CustomerOnSA, Customer
                        zprostredkovano_3_stranou = false.ToJsonString(),                                                                           // [MOCK] SalesArrangement - dle typu Usera (na offer zatím nemáme, dohodnuta mockovaná hodnota FALSE)
                        sjednal_CPM = user_cpm,                                                                                                   // [MOCK] 90400037  //Data.User!.CPM
                        sjednal_ICP = user_icp,                                                                                                  // [MOCK] 110000037 //Data.User!.ICP
                        // VIP_makler = 0.ToJsonString(),                                                                                           // [MOCK] User (default 0) !!! removed in D1-2
                        mena_prijmu = Data.Arrangement.Mortgage?.IncomeCurrencyCode,                                                                 // SalesArrangement
                        mena_bydliste = Data.Arrangement.Mortgage?.ResidencyCurrencyCode,                                                            // SalesArrangement
                        zpusob_zasilani_vypisu = Data.Offer.BasicParameters.StatementTypeId.ToJsonString(),                                         // Offerinstance.SimulationInputs.FeeSettings.StatementTypeId -> Offerinstance.BasicParameters.StatementTypeId
                        predp_hodnota_nem_zajisteni = Data.Offer.SimulationInputs.CollateralAmount.ToJsonString(),                                   // Offerinstance
                        typ_cerpani = typCerpani.ToJsonString(),
                        datum_garance_us = Data.Arrangement.OfferGuaranteeDateFrom.ToJsonString(),
                        garance_us_platnost_do = Data.Offer.BasicParameters.GuaranteeDateTo.ToJsonString(),                                          // Data.Offer.BasicParameters.GuaranteeDateTo
                        fin_kryti_vlastni_zdroje = Data.Offer.BasicParameters.FinancialResourcesOwn.ToJsonString(),                                  // OfferInstance
                        fin_kryti_cizi_zdroje = Data.Offer.BasicParameters.FinancialResourcesOther.ToJsonString(),                                   // OfferInstance
                        fin_kryti_celkem = financialResourcesTotal.ToJsonString(),                                                                   // OfferInstance
                        zpusob_podpisu_smluv_dok = Data.Arrangement.Mortgage?.ContractSignatureTypeId.ToJsonString(),                                   // Codebook SignatureTypes
                        zpusob_podpisu_zadosti = Data.Arrangement.Mortgage?.SalesArrangementSignatureTypeId.ToJsonString(),                             // Codebook SignatureTypes
                        souhlas_el_forma_komunikace = Data.Arrangement.Mortgage?.AgentConsentWithElCom.ToJsonString(),
                        zmenovy_navrh = 0.ToJsonString(),                                                                                               // Default: 0
                        seznam_domacnosti = Data.Households?.Select(i => MapHousehold(i)).ToArray() ?? Array.Empty<object>(),
                        zmocnenec_mp_id = FindZmocnenecMpId().ToJsonString(),

                        RZP_suma = insuranceSumRiskLife.ToJsonString(),
                        pojisteni_nem_suma = insuranceSumRealEstate.ToJsonString(),

                        seznam_id_formulare = seznamIdFormulare,
                        ea_kod = "608248",

                        //tests
                        cislo_dokumentu = MockDokumentId,
                    };
                    break;

                case EFormType.F3602:
                    data = new
                    {
                        cislo_smlouvy = Data.Arrangement.ContractNumber,
                        case_id = Data.Arrangement.CaseId.ToJsonString(),
                        //stav_zadosti = Data.SalesArrangementStatesById[Data.Arrangement.State].StarbuildId.ToJsonString(),
                        business_case_ID = Data.Arrangement.RiskBusinessCaseId,                                                                 // SalesArrangement
                        //risk_segment = Data.Arrangement.RiskSegment,
                        //datum_uzavreni_obchodu = riskBusinessCaseExpirationDate.ToJsonString(),                                                 // Default: CurrentDate + 90 dní
                        //kanal_ziskani = Data.Arrangement.ChannelId.ToJsonString(),                                                              // SalesArrangement - vyplněno na základě usera
                        datum_vytvoreni_zadosti = actualDate.ToJsonString(),                                                                    // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
                        //datum_prvniho_podpisu = firstSignedDate.ToJsonString(),                                                                      // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
                        //uv_produkt = Data.ProductType.Id.ToJsonString(),
                        uv_druh = Data.Offer.SimulationInputs.LoanKindId.ToJsonString(),                                                             // OfferInstance
                        //indikativni_LTV = Data.Offer.SimulationResults.LoanToValue.ToJsonString(),                                                   // OfferInstance
                        //indikativni_LTC = Data.Arrangement.LoanToCost.ToJsonString(),                                                                // OfferInstance -> SalesArrangement !!! moved from offer to arrangement in D1-2
                        //termin_cerpani_do = ((DateTime)Data.Offer.SimulationResults.DrawingDateTo).ToJsonString(),
                        //seznam_mark_akci = Array.Empty<object>(),                                                                               // [MOCK] OfferInstance (default empty array)
                        //sazba_vyhlasovana = Data.Offer.SimulationResults.LoanInterestRateAnnounced.ToJsonString(),                                   // OfferInstance
                        //sazba_skladacka = Data.Offer.SimulationResults.LoanInterestRate.ToJsonString(),                                              // OfferInstance
                        //sazba_poskytnuta = Data.Offer.SimulationResults.LoanInterestRate.ToJsonString(),                                             // OfferInstance = sazba_skladacka
                        //vyhlasovanaTyp = Data.Offer.SimulationResults.LoanInterestRateAnnouncedType.ToJsonString(),                                  // OfferInstance 
                        vyse_uveru = Data.Offer.SimulationResults.LoanAmount.ToJsonString(),                                                         // OfferInstance
                        anuitni_splatka = Data.Offer.SimulationResults.LoanPaymentAmount.ToJsonString(),                                             // OfferInstance
                        //kodZvyhodneni = Data.Offer.SimulationResults.EmployeeBonusLoanCode.ToJsonString(),                                           // OfferInstance
                        splatnost_uv_mesice = Data.Offer.SimulationResults.LoanDuration.ToJsonString(),                                              // OfferInstance (kombinace dvou vstupů roky + měsíce na FE)
                        fixace_uv_mesice = Data.Offer.SimulationInputs.FixedRatePeriod.ToJsonString(),                                               // OfferInstance - na FE je to v rocích a je to číselník ?
                        //individualni_cenotvorba_odchylka = Data.Offer.SimulationInputs.InterestRateDiscount.ToJsonString(),
                        //predp_termin_cerpani = Data.Arrangement.Mortgage?.ExpectedDateOfDrawing.ToJsonString(),                                      // SalesArrangement 
                        den_splaceni = Data.Offer.SimulationInputs.PaymentDay.ToJsonString(),                                                        // OfferInstance
                        //forma_splaceni = 1.ToJsonString(),                                                                                      // [MOCK] OfferInstance (default 1)  
                        //seznam_poplatku = Data.Offer.SimulationResults.Fees?.Select(i => MapFee(i)).ToArray() ?? Array.Empty<object>(),              // Data.Offer.SimulationResults.Fees
                        //seznam_ucelu = Data.Offer.SimulationInputs.LoanPurposes?.Select(i => MapLoanPurpose(i)).ToArray() ?? Array.Empty<object>(),  // OfferInstance - 1..5 ??? má se brát jen prvních 5 účelů ?
                        //seznam_objektu = Data.Arrangement.Mortgage?.LoanRealEstates.ToList().Select((i, index) => MapLoanRealEstate(i, index + 1)).ToArray() ?? Array.Empty<object>(), // SalesArrangement - 0..3 ???
                        seznam_ucastniku = Data.CustomersOnSa?.Select(i => MapCustomerOnSA(i)).ToArray() ?? Array.Empty<object>(),                   // CustomerOnSA, Customer
                        //zprostredkovano_3_stranou = false.ToJsonString(),                                                                       // [MOCK] SalesArrangement - dle typu Usera (na offer zatím nemáme, dohodnuta mockovaná hodnota FALSE)
                        sjednal_CPM = user_cpm,                                                                                                   // [MOCK] 90400037  //Data.User!.CPM
                        sjednal_ICP = user_icp,                                                                                                  // [MOCK] 110000037 //Data.User!.ICP
                        //// VIP_makler = 0.ToJsonString(),                                                                                       // [MOCK] User (default 0) !!! removed in D1-2
                        //mena_prijmu = Data.Arrangement.Mortgage?.IncomeCurrencyCode,                                                                 // SalesArrangement
                        //mena_bydliste = Data.Arrangement.Mortgage?.ResidencyCurrencyCode,                                                            // SalesArrangement

                        //// zpusob_zasilani_vypisu = Data.Offer.SimulationResults.StatementTypeId.ToJsonString(),                                     // Offerinstance !!! removed in D1-2 (moved to fees without DV mapping)
                        //predp_hodnota_nem_zajisteni = Data.Offer.SimulationInputs.CollateralAmount.ToJsonString(),                                   // Offerinstance
                        //typ_cerpani = Data.Offer.SimulationInputs.DrawingType.ToJsonString(),
                        //datum_garance_us = Data.Arrangement.OfferGuaranteeDateFrom.ToJsonString(),
                        //garance_us_platnost_do = Data.Offer.BasicParameters.GuaranteeDateTo.ToJsonString(),                                          // Data.Offer.BasicParameters.GuaranteeDateTo
                        //fin_kryti_vlastni_zdroje = Data.Offer.BasicParameters.FinancialResourcesOwn.ToJsonString(),                                  // OfferInstance
                        //fin_kryti_cizi_zdroje = Data.Offer.BasicParameters.FinancialResourcesOther.ToJsonString(),                                   // OfferInstance
                        //fin_kryti_celkem = financialResourcesTotal.ToJsonString(),                                                                   // OfferInstance
                        zpusob_podpisu_smluv_dok = Data.Arrangement.Mortgage?.ContractSignatureTypeId.ToJsonString(),                                        // SalesArrangement
                        seznam_domacnosti = Data.Households?.Select(i => MapHousehold(i)).ToArray() ?? Array.Empty<object>(),

                        seznam_id_formulare = seznamIdFormulare,
                        ea_kod = "608243",

                        //tests
                        cislo_dokumentu = MockDokumentId,
                    };
                    break;

            }

            var options = new JsonSerializerOptions { 
                DefaultIgnoreCondition = ignoreNullValues ? System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull : System.Text.Json.Serialization.JsonIgnoreCondition.Never,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-character-encoding
            };

            var json = JsonSerializer.Serialize(data, options);

            return json;
        }

        #region JSON example

    /*
    {
    "cislo_smlouvy": "HF00000000055", //SalesArrangement
    "case_id": "2928155", //Case
    "business_case_ID": "0", //SalesArrangement - na základě volání RBC
    "kanal_ziskani": "4", //SalesArrangement - vyplněno na základě usera
    "datum_vytvoreni_zadosti": "25.01.2022", //SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
    "datum_prvniho_podpisu": "25.01.2022", //SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
    "uv_produkt": "20001", //SalesArrangement
    "uv_druh": "2000", //OfferInstance
    "indikativni_LTV": "1", //OfferInstance
    "indikativni_LTC": "1", //OfferInstance
    "seznam_mark_akci": [ //OfferInstance
    {
        "kodMAakce": "1",
        "typMaAkce": "DOMICILACE",
        "zaskrtnuto": "1",
        "uplatnena": "1",
        "odchylkaSazby": "-0,05"
    }
    ],
    "individualni_sleva_us": "0", //OfferInstance - default 0
    "garance_us": "0", //OfferInstance - default 0 
    "sazba_vyhlasovana": "1", //OfferInstance
    "sazba_skladacka": "1", //OfferInstance
    "sazba_poskytnuta": "1", //OfferInstance = sazba_skladacka
    "vyhlasovanaTyp": "1", //OfferInstance
    "vyse_uveru": "100000", //OfferInstance
    "anuitni_splatka": "1000", //OfferInstance
    "splatnost_uv_mesice": "120", //OfferInstance (kombinace dvou vstupů roky + měsíce na FE)
    "fixace_uv_mesice": "36", //OfferInstance - na FE je to v rocích a je to číselník ?
    "predp_termin_cerpani": "01.02.2022", //SalesArrangement
    "den_splaceni": "15", //OfferInstance default=15
    "forma_splaceni": "1", //OfferInstance default = inkaso
    "seznam_poplatku": [ //OfferInstance - celý objekt vůbec nebude - TBD - diskuse k simulaci
    {
        "kod_poplatku": "2001",
        "suma_poplatku_sazebnik": "1000",
        "suma_poplatku_skladacka": "1000",
        "suma_poplatku_vysledna": "1000"
    }
    ],
    "seznam_ucelu": [ //OfferInstance - 1..5
    {
        "uv_ucel": "4", //offerInstance
        "uv_ucel_suma": "100000" //offerInstance
    }
    ],
    "seznam_objektu": [ //SalesArrangement - 0..3
    {
        "cislo_objektu_uveru": "12345",
        "typ_nemovitosti": "2",
        "objekt_uv_je_zajisteni": "0",
        "ucel_porizeni": "1"
    }
    ],
    "seznam_ucastniku": [
    {
        "role": "1", //CustomerOnSA
        "zmocnenec": "0", //CustomerOnSA
        "cislo_domacnosti": "1", //CustomerOnSA
        "klient": {
            "rodne_cislo": "7253021435", //Customer
            "kb_id": "0", //Customer
            "mp_id": "0", //CustomerOnSA
            "spolecnost_KB": "", //Customer - pokud načteme z CM
            "titul_pred": "Mgr.", //Customer
            "prijmeni_nazev": "KLIENTKA AAAAAAAAAT", //Customer
            "prijmeni_rodne": "", //Customer
            "jmeno": "Irena", //Customer
            "datum_narozeni": "02.03.1972", //Customer
            "misto_narozeni_obec": "Litomyšl", //Customer
            "misto_narozeni_stat": "16", //Customer
            "pohlavi": "Z", //Customer
            "statni_prislusnost": "16", //Customer
            "zamestnanec": "0", //Customer
            "rezident": "1", //Customer
            "PEP": "0", //Customer
            "seznam_adres": [ //Customer
                {
                    "typ_adresy": "1",
                    "ulice": "Milady Horákové",
                    "cislo_popisne": "12360",
                    "cislo_orientacni": "",     //pokud načteme z CM
                    "ulice_dodatek": "",    //pokud načteme z CM
                    "psc": "11000",
                    "misto": "PRAHA",
                    "stat": "16"
                }
            ],
            "seznam_dokladu": [ //Customer
                {
                    "cislo_dokladu": "ABC123",
                    "typ_dokladu": "1",
                    "vydal": "MěÚ Litomyšl",
                    "vydal_datum": "01.01.2020",
                    "vydal_stat": "16",
                    "platnost_do": "31.07.2028"
                }
            ],
            "seznam_kontaktu": [ //Customer
                {
                    "typ_kontaktu": "1",    //mobil
                    "hodnota_kontaktu": "111111111"
                },
                {
                    "typ_kontaktu": "5",    //email
                    "hodnota_kontaktu": "klientkaaaaaaaaaat@seznam.cz"
                }
            ],
            "rodinny_stav": "2", //Customer
            "druh_druzka": "0", //CustomerOnSA
            "vzdelani": "3", //Customer
            "prijmy": [ //Incomes
                {
                    "prvni_pracovni_sml_od": "28.01.2020",
                    "posledni_zamestnani_od": "28.01.2020",
                    "poradi_prijmu": "1",
                    "zdroj_prijmu_hlavni": "1",
                    "typ_pracovniho_pomeru": "3",
                    "klient_ve_vypovedni_lhute": "0",
                    "klient_ve_zkusebni_lhute": "0",
                    "prijem_ze_zahranici": "0",
                    "domicilace_prijmu_ze_zamestnani": "0",
                    "pracovni_smlouva_aktualni_od": "",
                    "pracovni_smlouva_aktualni_do": "",
                    "zamestnavatel_nazov": "",
                    "zamestnavatel_rc_ico": "",
                    "zamestnavatel_sidlo_ulice": "",
                    "zamestnavatel_sidlo_cislo_popisne_orientacni": "",
                    "zamestnavatel_sidlo_mesto": "",
                    "zamestnavatel_sidlo_psc": "",
                    "zamestnavatel_sidlo_stat": "",
                    "zamestnavatel_telefonni_cislo": "",
                    "zamestnavatel_okec": "",
                    "zamestnavatel_pracovni_sektor": "",
                    "zamestnavatel_senzitivni_sektor": "0",
                    "povolani": "1",
                    "zamestnan_jako": "",
                    "prijem_vyse": "50000",
                    "prijem_mena": "CZK",
                    "zrazky_ze_mzdy_rozhodnuti": "",
                    "zrazky_ze_mzdy_splatky": "",
                    "zrazky_ze_mzdy_ostatni": "",
                    "prijem_potvrzeni_vystavila_ext_firma": "0",
                    "prijem_potvrzeni_misto_vystaveni": "",
                    "prijem_potvrzeni_datum": "",
                    "prijem_potvrzení_osoba": "",
                    "prijem_potvrzeni_kontakt": ""
                }
            ],
            "zavazky": [ //Expenses
                {
                    "cislo_zavazku": "1",
                    "druh_zavazku": "1",
                    "vyse_splatky": "10000",
                    "vyse_nesplacene_jistiny": "600000",
                    "vyse_limitu": "",
                    "mimo_entitu_mandanta": "0"
                }
            ],
            "prijem_sbiran": "0", //CustomerOnSA
            "uzamcene_prijmy": "0", //CustomerOnSA
            "datum_posledniho_uzam_prijmu": "" //CustomerOnSA
        }
    }
    ],
    "zprostredkovano_3_stranou": "0", //SalesArrangement - dle typu Usera
    "sjednal_CPM": "999666", //User
    "sjednal_ICP": "222111", //User
    "mena_prijmu": "CZK", //SalesArrangement
    "mena_bydliste": "CZK", //SalesArrangement
    "zpusob_zasilani_vypisu": "1", //Offerinstance
    "predp_hodnota_nem_zajisteni": "100000", //Offerinstance
    "fin_kryti_vlastni_zdroje": "2000000", //Offerinstnace
    "fin_kryti_cizi_zdroje": "0", //Offerinstance
    "fin_kryti_celkem": "2000000", //Offerinstance
    "zpusob_podpisu_smluv_dok": "3", //SalesArrangement
    "seznam_domacnosti": ["cislo_domacnosti": "1"          {
        "pocet_deti_0_10let": "1", //Household
        "pocet_deti_nad_10let": "0", //Household
        "cislo_domacnosti": "1", //Household
        "sporeni": "1000", //Household
        "pojisteni": "500", //Household
        "naklady_na_bydleni": "10000", //Household
        "ostatni_vydaje": "6000", //Household
        "vyporadani_majetku": "0" //Household
    }
    ]
    }
    */
    #endregion


    #region SampleFormData

    public static Eas.EasWrapper.CheckFormData BuildSampleFormData3601()
        {
            var sampleData = "{\"cislo_dokumentu\":\"9876543210\",\"cislo_smlouvy\":\"1234567890\",\"datum_spisania\":\"01.11.2021\",\"CPM\":\"123456\",\"ICP\":\"654321\",\"formular_kod_ex\":\"2\",\"seznam_ucastniku\":[{\"klient\":{\"rodne_cislo_ico\":\"1234569024\",\"titul_pred\":\"Ing.\",\"prijmeni_nazev\":\"Testovic\",\"jmeno\":\"Test\",\"titul_za\":\"\"},\"seznam_adres\":[{\"typ_adresy\":\"trvala\",\"ulice\":\"Testovacia\",\"cislo_popisne\":\"32\",\"cislo_orientacni\":\"21\",\"ulice_dodatek\":\"\",\"psc\":\"60012\",\"misto\":\"Brno\",\"stat\":\"\"}]},\"role\":\"vlastnik\"}]}";
            sampleData = "{}";
            sampleData = "{cislo_dokumentu:\"9876543210\", cislo_smlouvy: \"1234567890\", datum_spisania: \"01.11.2021\" }";

            var formData = new Eas.EasWrapper.CheckFormData()
            {
                formular_id = 3601001,
                cislo_smlouvy = "1234567890",
                dokument_id = "9876543210",
                datum_prijeti = new DateTime(2022, 1, 1),
                data = sampleData
            };

            return formData;
        }

        #endregion

    }
}
