using System.Text.Json;

using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;

using DomainServices.SalesArrangementService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;


namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement.Shared
{
    public class FormDataJsonBuilder
    {

        #region Construction

        public FormData Data { get; init; }

        private EProductTypeKind ProductTypeKind { get; init; }

        public FormDataJsonBuilder(FormData data)
        {
            Data = data;
            ProductTypeKind = ParseProductTypeKind(data);
        }

        #endregion


        #region ProductType Specific

        /// <summary>
        /// Určuje produktový typ na základě dat (ProductType, LoanKind)
        /// </summary>
        private static EProductTypeKind ParseProductTypeKind(FormData data)
        {
            var typeKindCode = $"{data.ProductType.Id}_{data.Offer.SimulationInputs.LoanKindId}";

            var productTypeKind = EProductTypeKind.Unknown;

            switch (typeKindCode)
            {
                case "20001_2000": productTypeKind = EProductTypeKind.KBMortgage; break;                    // Hypoteční úvěr(standard) - Produkt: 20001 / Druh: 2000
                case "20010_2000": productTypeKind = EProductTypeKind.KBAmericanMortgage; break;            // Americká hypotéka - Produkt: 20010 / Druh: 2000
                case "20001_2001": productTypeKind = EProductTypeKind.KBMortgageWithoutRealEstate; break;   // HÚ bez nemovitosti - Produkt: 20001 / Druh: 2001
                default: productTypeKind = EProductTypeKind.Unknown; break;                                 // Pokud není uvedeno, plnění atributů celého objektu není produktově závislé
            }

            return productTypeKind;
        }

        /// <summary>
        /// Mapa atributů, které jsou závislé na produktovém typu
        /// </summary>
        private static readonly Dictionary<EJsonKey, EProductTypeKind> SpecificJsonKeys = new Dictionary<EJsonKey, EProductTypeKind>
        {
            {EJsonKey.UvDruh, EProductTypeKind.KBMortgage | EProductTypeKind.KBMortgageWithoutRealEstate },
            {EJsonKey.PojisteniNemSuma, EProductTypeKind.KBMortgage | EProductTypeKind.KBAmericanMortgage },
            {EJsonKey.DeveloperId, EProductTypeKind.KBMortgage },
            {EJsonKey.DeveloperProjektId, EProductTypeKind.KBMortgage },
            {EJsonKey.DeveloperPopis, EProductTypeKind.KBMortgage },
            {EJsonKey.SeznamUcelu, EProductTypeKind.KBMortgage | EProductTypeKind.KBAmericanMortgage },
            {EJsonKey.SeznamObjektu, EProductTypeKind.KBMortgage },
            {EJsonKey.FinKrytiVlastniZdroje, EProductTypeKind.KBMortgage | EProductTypeKind.KBMortgageWithoutRealEstate },
            {EJsonKey.FinKrytiCiziZdroje, EProductTypeKind.KBMortgage | EProductTypeKind.KBMortgageWithoutRealEstate },
            {EJsonKey.FinKrytiCelkem, EProductTypeKind.KBMortgage | EProductTypeKind.KBMortgageWithoutRealEstate },
        };

        /// <summary>
        /// Určuje, zda se má být atribut [key] obsažen ve výsledném JSONu 
        /// </summary>
        private bool FillKey(EJsonKey key)
        {
            if (this.ProductTypeKind == EProductTypeKind.Unknown)
            {
                return true;
            }

            if (!SpecificJsonKeys.ContainsKey(key))
            {
                return true;
            }

            return this.ProductTypeKind.HasFlag(SpecificJsonKeys[key]);
        }

        /// <summary>
        /// Pokud má být atribut [key] obsažen ve výsledném JSONu, vrátí hodnotu [value]. V opačném případě vrátí [null]. 
        /// </summary>
        private object? WhenFillKey(EJsonKey key, object? value)
        {
            return FillKey(key) ? value : null;
        }

        #endregion

        public static readonly string MockDokumentId = "9876543210"; // TODO: dočasný mock - odstranit až si to Assecco odladí

        private List<Household> GetHouseholdsByFormType(EFormType formType)
        {
            var households = new List<Household>();

            switch (formType)
            {
                case EFormType.F3601:
                    households = Data.Households.Where(i => ((HouseholdTypes)i.HouseholdTypeId) == HouseholdTypes.Main).ToList();
                    break;

                case EFormType.F3602:
                    households = Data.Households.Where(i => ((HouseholdTypes)i.HouseholdTypeId) == HouseholdTypes.Codebtor || ((HouseholdTypes)i.HouseholdTypeId) == HouseholdTypes.Garantor).ToList();
                    break;
            }

            return households;
        }

        public string BuildJson(EFormType formType, string formId, bool ignoreNullValues = true)
        {
            // CNFL: https://wiki.kb.cz/display/HT/DROP1.3
            // CNFL: https://wiki.kb.cz/display/HT/RIP%28v2%29+-+POST+LoanApplication

            var actualDate = DateTime.Now.Date;

            // seřadit podle HouseholdTypeId a číslovat vzestupně 1, 2, 3; bude se upravovat v závislosti na FormType, pro F360 bude '1'(pouze hlavní domácnost), pro F3602 dosavadní logika (ručitelská a spoludlužnická domácnost):
            var households = GetHouseholdsByFormType(formType);
            var householdsSorted = households.OrderBy(i => i.HouseholdTypeId).ToList() ?? new List<Household>();
            var householdNumbersById = householdsSorted.ToDictionary(i => i.HouseholdId, i => householdsSorted.IndexOf(i) + 1);

            var customerOnSAIds1 = households.Where(i => i.CustomerOnSAId1.HasValue).Select(i => i.CustomerOnSAId1);
            var customerOnSAIds2 = households.Where(i => i.CustomerOnSAId2.HasValue).Select(i => i.CustomerOnSAId2);
            var customerOnSAIds = customerOnSAIds1.Concat(customerOnSAIds2);

            var customersOnSa = Data.CustomersOnSa.Where(i => customerOnSAIds.Contains(i.CustomerOnSAId)).ToList();

            //var householdsByCustomerOnSAId = Data.CustomersOnSa.ToDictionary(i => i.CustomerOnSAId, i => households.Where(h => h.CustomerOnSAId1 == i.CustomerOnSAId || h.CustomerOnSAId2 == i.CustomerOnSAId).ToArray());
            var householdsByCustomerOnSAId = customersOnSa.ToDictionary(i => i.CustomerOnSAId, i => households.Where(h => h.CustomerOnSAId1 == i.CustomerOnSAId || h.CustomerOnSAId2 == i.CustomerOnSAId).ToArray());

            var firstEmploymentType = Data.EmploymentTypes.OrderBy(i => i.Id).FirstOrDefault();

            var obligationTypeAmountIds = Data.ObligationTypeIdsByObligationProperty["amount"] ?? new List<int>();

            object? MapHousehold(Household i, int cisloDomacnosti)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    cislo_domacnosti = householdNumbersById[i.HouseholdId].ToJsonString(),
                    pocet_deti_0_10let = (i.Data.ChildrenUpToTenYearsCount ?? 0).ToJsonString(),
                    pocet_deti_nad_10let = (i.Data.ChildrenOverTenYearsCount ?? 0).ToJsonString(),
                    sporeni = i.Expenses.SavingExpenseAmount.ToJsonString(),
                    pojisteni = i.Expenses.InsuranceExpenseAmount.ToJsonString(),
                    naklady_na_bydleni = i.Expenses.HousingExpenseAmount.ToJsonString(),
                    ostatni_vydaje = i.Expenses.OtherExpenseAmount.ToJsonString(),
                    vyporadani_majetku = (i.Data.PropertySettlementId ?? 0).ToJsonString(),
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
                    slevaIC = i.DiscountPercentage.ToJsonString(),
                    kodMaAkce = i.MarketingActionId.ToJsonString(),
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
                    typ_adresy = i.AddressTypeId.ToJsonString(),    // D1.3 zrušení defaultu, plníme reálnými daty; Hodnota odpovídají v číselníku sloupci SbJsonValue
                    ulice = i.Street ?? i.City,                     // D1.3 ACE změnilo kardinalitu zpět na 1..1 s tím, že se případně plní atributem 136 [misto]
                    cislo_popisne = i.BuildingIdentificationNumber, // D1.3 změna kardinality z 1..1 na 0..1
                    cislo_orientacni = i.LandRegistryNumber,        // D1.3 změna datového typu z Int na String

                    cislo_evidencni = i.EvidenceNumber,
                    ulice_dodatek = i.DeliveryDetails,
                    psc = i.Postcode.ToPostCodeJsonString(),
                    misto = i.City,
                    stat = i.CountryId.ToJsonString(),
                    cast_obce = i.CityDistrict,
                    obvod_praha = i.PragueDistrict,
                    uzemni_celek = i.CountrySubdivision,
                    adresni_bod_id = i.AddressPointId,
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

                var vyseKonsolidJistiny = (decimal?)i.LoanPrincipalAmountConsolidated;
                var vyseNesplaceneJistiny = (decimal?)i.LoanPrincipalAmount;
                var vyseLimitu = (decimal?)i.CreditCardLimit;

                /*
                dynamické plnění NOBY → JSON

                Pro korekce typu "1", "2", "4" automaticky default = "0"
                Pro ObligationCorrectionType typu: "3"

                Částečná konsolidace = "1", pokud:
                    pro Obligation.ObligationTypeId s ObligationProperty = "amount" platí: vyse_konsolid_jistiny < vyse_nesplacene_jistiny
                    pro Obligation.ObligationTypeId s ObligationProperty = "limit" platí: vyse_konsolid_jistiny < vyse_limitu


                Částečná konsolidace = "0", pokud:
                    pro Obligation.ObligationTypeId s ObligationProperty = "amount" platí: vyse_konsolid_jistiny = vyse_nesplacene_jistiny
                    pro Obligation.ObligationTypeId s ObligationProperty = "limit" platí: vyse_konsolid_jistiny = vyse_limitu
                */

                var castecnaKonsolidace = false;
                if (i.Correction.CorrectionTypeId == 3)
                {
                    castecnaKonsolidace = (obligationTypeAmountIds!.Contains(i.ObligationTypeId!.Value))
                        ? vyseKonsolidJistiny < vyseNesplaceneJistiny
                        : vyseKonsolidJistiny < vyseLimitu;
                }

                decimal? vyseKorekceZavazkuJistina = obligationTypeAmountIds!.Contains(i.ObligationTypeId!.Value) ? i.Correction?.LoanPrincipalAmountCorrection : i.Correction?.CreditCardLimitCorrection;

                return new
                {
                    cislo_zavazku = rowNumber.ToJsonString(),
                    druh_zavazku = i.ObligationTypeId.ToJsonString(),
                    vyse_splatky = i.InstallmentAmount.ToJsonString(),
                    vyse_nesplacene_jistiny = vyseNesplaceneJistiny.ToJsonString(),
                    vyse_limitu = vyseLimitu.ToJsonString(),
                    veritel_kod_banky = i.Creditor?.CreditorId,
                    veritel_nazev = i.Creditor?.Name,
                    mimo_entitu_mandanta = i.Creditor?.IsExternal?.ToJsonString(),
                    zpusob_korekce_zavazku = i.Correction?.CorrectionTypeId?.ToJsonString(),
                    vyse_korekce_zavazku_o_jistina = vyseKorekceZavazkuJistina.ToJsonString(),
                    vyse_korekce_zavazku_o_spl = i.Correction?.InstallmentAmountCorrection?.ToJsonString(),
                    castecna_konsolidace = castecnaKonsolidace.ToJsonString(),
                    vyse_konsolid_jistiny = vyseKonsolidJistiny.ToJsonString(),
                };
            }

            object? MapCustomerIncomeEmployment(IncomeInList? iil, int rowNumber)
            {
                if (iil == null)
                {
                    return null;
                }

                //string? GetAddressNumber(GrpcAddress? address)
                //{
                //    if (address == null)
                //    {
                //        return null;
                //    }

                //    //složit string ve formátu "BuildingIdentificationNumber/LandRegistryNumber"
                //    var parts = new string?[] { address.BuildingIdentificationNumber, address.LandRegistryNumber };

                //    var number = String.Join("/", parts.Where(i => !string.IsNullOrEmpty(i)));

                //    return String.IsNullOrEmpty(number) ? null : number;
                //}

                var i = Data.IncomesById[iil.IncomeId];

                var employmentTypeId = i.Employement?.Job?.EmploymentTypeId ?? firstEmploymentType?.Id;

                return new
                {
                    poradi_prijmu = rowNumber.ToJsonString(),
                    typ_pracovniho_pomeru = employmentTypeId.ToJsonString(),

                    klient_ve_vypovedni_lhute = i.Employement?.Job?.IsInProbationaryPeriod.ToJsonString(),                  // Pokud je parametr null, mapujeme 0
                    klient_ve_zkusebni_lhute = i.Employement?.Job?.IsInTrialPeriod.ToJsonString(),                          // Pokud je parametr null, mapujeme 0

                    prijem_ze_zahranici_zpusob_vykonu = i.Employement?.ForeignIncomeTypeId.ToJsonString(),
                    prvni_pracovni_sml_od = i.Employement?.Job?.FirstWorkContractSince.ToJsonString(),
                    pracovni_smlouva_aktualni_od = i.Employement?.Job?.CurrentWorkContractSince.ToJsonString(),
                    pracovni_smlouva_aktualni_do = i.Employement?.Job?.CurrentWorkContractTo.ToJsonString(),

                    zamestnavatel_nazov = i.Employement?.Employer?.Name,

                    // pouze jedna z hodnot, neměly by být zadány obě:
                    zamestnavatel_rc_ico = new List<string?> { i!.Employement?.Employer?.Cin, i!.Employement?.Employer?.BirthNumber }.FirstOrDefault(i => !String.IsNullOrEmpty(i)),
                    zamestnavatel_sidlo_stat = i.Employement?.Employer?.CountryId?.ToJsonString(),

                    zamestnan_jako = i.Employement?.Job?.JobDescription,
                    prijem_vyse = iil.Sum.ToJsonString(),
                    prijem_mena = iil.CurrencyCode,
                    hruby_rocny_prijem = i.Employement?.Job?.GrossAnnualIncome.ToJsonString(),

                    srazky_ze_mzdy_rozhodnuti = i.Employement?.WageDeduction?.DeductionDecision.ToJsonString(),
                    srazky_ze_mzdy_splatky = i.Employement?.WageDeduction?.DeductionPayments.ToJsonString(),
                    srazky_ze_mzdy_ostatni = i.Employement?.WageDeduction?.DeductionOther.ToJsonString(),

                    prijem_potvrzeni_vystavila_ext_firma = i.Employement?.IncomeConfirmation?.IsIssuedByExternalAccountant.ToJsonString(),
                    prijem_potvrzeni_datum = i.Employement?.IncomeConfirmation?.ConfirmationDate.ToJsonString(),
                    prijem_potvrzeni_osoba = i.Employement?.IncomeConfirmation?.ConfirmationPerson,
                    prijem_potvrzeni_kontakt = i.Employement?.IncomeConfirmation?.ConfirmationContact,

                    typ_dokumentu = 1.ToJsonString(),                                                               // Pro příjem ze zaměstnání default "1"
                };
            }

            object? MapCustomerIncomeEntrepreneur(IncomeInList? iil, int rowNumber)
            {
                if (iil == null)
                {
                    return null;
                }

                var i = Data.IncomesById[iil.IncomeId];

                return new
                {
                    poradi_prijmu = rowNumber.ToJsonString(),

                    // Vyplní se nenulová hodnota z Income.Data.Cin nebo Income.Data.BirthNumber:
                    rc_ico = new List<string?> { i.Entrepreneur?.Cin, i.Entrepreneur?.BirthNumber }.FirstOrDefault(i => !String.IsNullOrEmpty(i)),

                    sidlo_stat = i.Entrepreneur?.CountryOfResidenceId.ToJsonString(),
                    prijem_vyse = iil.Sum.ToJsonString(),
                    prijem_mena = iil.CurrencyCode,

                    typ_dokumentu = 2.ToJsonString(),
                    // datum_zahajeni_podnikatelske_cinnosti =          // Neplníme
                };
            }

            object? MapCustomerIncomeRent(IncomeInList? iil, int rowNumber)
            {
                if (iil == null)
                {
                    return null;
                }

                return new
                {
                    poradi_prijmu = rowNumber.ToJsonString(),
                    prijem_vyse = iil.Sum.ToJsonString(),
                    prijem_mena = iil.CurrencyCode,
                    // typ_dokumentu =                          // Neplníme
                };
            }

            object? MapCustomerIncomeOther(IncomeInList? iil, int rowNumber)
            {
                if (iil == null)
                {
                    return null;
                }

                var i = Data.IncomesById[iil.IncomeId];

                return new
                {
                    poradi_prijmu = rowNumber.ToJsonString(),
                    zdroj_prijmu_ostatni = i.Other?.IncomeOtherTypeId.ToJsonString(),
                    prijem_vyse = iil.Sum.ToJsonString(),
                    prijem_mena = iil.CurrencyCode,
                    // typ_dokumentu =                          // Neplníme
                };
            }

            object? MapCustomer(CustomerOnSA i)
            {
                // CNFL: https://wiki.kb.cz/display/HT/Customer+D1.3

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

                var taxResidencyCountryId = c.NaturalPerson?.TaxResidencyCountryId;
                var taxResidencyCountryCode = taxResidencyCountryId.HasValue ? (Data.CountriesById.ContainsKey(taxResidencyCountryId.Value) ? Data.CountriesById[taxResidencyCountryId.Value].ShortName : null) : null;

                var household = householdsByCustomerOnSAId![i.CustomerOnSAId].First();

                var incomes = i.Incomes?.ToList() ?? new List<IncomeInList>();
                var incomesEmployment = incomes.Where(i => i.IncomeTypeId == 1).ToList();   // Příjmy ze zaměstnání
                var incomeEntrepreneur = incomes.FirstOrDefault(i => i.IncomeTypeId == 2);  // Prijem z danoveho priznani
                var incomeRent = incomes.FirstOrDefault(i => i.IncomeTypeId == 3);          // Prijem z pronajmu
                var incomesOther = incomes.Where(i => i.IncomeTypeId == 4).ToList();        // Prijmy ostatní

                return new
                {
                    rodne_cislo = c.NaturalPerson?.BirthNumber,
                    // segment =    // pro D1.3 neplníme, bude se řešit později
                    kb_id = identityKb.IdentityId.ToJsonString(),
                    mp_id = identityMp.IdentityId.ToJsonString(),                                   // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!
                    //datum_svadby =    // D1.3 nepracujeme se zástavci, zatím nesbíráme a neplníme
                    titul_pred = cDegreeBeforeId.HasValue ? Data.AcademicDegreesBeforeById[cDegreeBeforeId.Value].Name : null,    // (použít Name, nikoliv jen Id) 
                    prijmeni_nazev = c.NaturalPerson?.LastName,
                    prijmeni_rodne = c.NaturalPerson?.BirthName,
                    jmeno = c.NaturalPerson?.FirstName,
                    datum_narozeni = c.NaturalPerson?.DateOfBirth.ToJsonString(),
                    misto_narozeni_obec = c.NaturalPerson?.PlaceOfBirth,
                    misto_narozeni_stat = c.NaturalPerson?.BirthCountryId.ToJsonString(),
                    pohlavi = cGenderId.HasValue ? Data.GendersById[cGenderId.Value].StarBuildJsonCode : null,
                    statni_prislusnost = cCitizenshipCountriesId.ToJsonString(),
                    pravni_omezeni_typ = c.NaturalPerson?.IsLegallyIncapable,
                    pravni_omezeni_do = c.NaturalPerson?.LegallyIncapableUntil.ToJsonString(),
                    rezident = (taxResidencyCountryCode?.ToUpperInvariant() == "CZ").ToJsonString(),
                    PEP = c.NaturalPerson?.IsPoliticallyExposed.ToJsonString(),
                    seznam_adres = c.Addresses?.OrderBy(i => i.AddressTypeId).Select(i => MapAddress(i)).ToArray() ?? Array.Empty<object>(),
                    seznam_dokladu = cIdentificationDocuments,
                    seznam_kontaktu = c.Contacts?.OrderBy(i => i.ContactTypeId).Select(i => MapContact(i)).ToArray() ?? Array.Empty<object>(),
                    rodinny_stav = c.NaturalPerson?.MaritalStatusStateId.ToJsonString(),
                    je_fatca = 0.ToJsonString(),    // pro D1.3 default 0, bude se řešit později
                    druh_druzka = (household?.Data?.AreCustomersPartners == true).ToJsonString(),
                    vzdelani = c.NaturalPerson?.EducationLevelId.ToJsonString(),

                    seznam_prijmu_zam = incomesEmployment?.OrderBy(i => i.IncomeId).Select((i, index) => MapCustomerIncomeEmployment(i, incomes!.IndexOf(i) + 1)).ToArray() ?? Array.Empty<object>(),
                    prijem_dp = incomeEntrepreneur is null ? null : MapCustomerIncomeEntrepreneur(incomeEntrepreneur, incomes!.IndexOf(incomeEntrepreneur) + 1),
                    prijem_naj = incomeRent is null ? null : MapCustomerIncomeRent(incomeRent, incomes!.IndexOf(incomeRent) + 1),
                    seznam_prijmu_ost = incomesOther?.OrderBy(i => i.IncomeId).Select((i, index) => MapCustomerIncomeOther(i, incomes!.IndexOf(i) + 1)).ToArray() ?? Array.Empty<object>(),

                    seznam_zavazku = i.Obligations?.OrderBy(i => i.ObligationId).Select((i, index) => MapCustomerObligation(i, index + 1)).ToArray() ?? Array.Empty<object>(),
                    uzamcene_prijmy = ((DateTime?)i.LockedIncomeDateTime).HasValue.ToJsonString(),
                    datum_posledniho_uzam_prijmu = i.LockedIncomeDateTime.ToJsonString(),
                };
            }

            object? MapCustomerOnSA(CustomerOnSA i)
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

                var household = householdsByCustomerOnSAId![i.CustomerOnSAId].First();
                return new
                {
                    role = roleId.ToJsonString(),
                    cislo_domacnosti = householdNumbersById[household.HouseholdId].ToJsonString(),
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

            //DateTime riskBusinessCaseExpirationDate = (Data.Arrangement.RiskBusinessCaseExpirationDate is not null) ? (DateTime)Data.Arrangement.RiskBusinessCaseExpirationDate! : actualDate.AddDays(90).Date;
            DateTime firstSignedDate = (Data.Arrangement.FirstSignedDate is not null) ? (DateTime)Data.Arrangement.FirstSignedDate! : actualDate;
            var seznamIdFormulare = new object[] { new { id_formulare = 0.ToJsonString() } }; // v D1.3 zatím ponechat, bude se řešit později (info od HH)

            var user_cpm = "99806569";  // [MOCK] 90400037  //Data.User!.CPM // ???
            var user_icp = "114306569"; // [MOCK] 110000037 //Data.User!.ICP // ???

            object data = new { };

            switch (formType)
            {
                case EFormType.F3601:

                    var developer = Data.Offer.SimulationInputs.Developer;
                    var developerDescription = (developer == null) ? null : String.Join(",", (new List<string> { developer.NewDeveloperName ?? String.Empty, developer.NewDeveloperCin ?? String.Empty, developer.NewDeveloperProjectName ?? String.Empty }));

                    var insuranceSumRiskLife = Data.Offer.SimulationInputs.RiskLifeInsurance == null ? (decimal?)null : (decimal)Data.Offer.SimulationInputs.RiskLifeInsurance.Sum;
                    var insuranceSumRealEstate = Data.Offer.SimulationInputs.RealEstateInsurance == null ? (decimal?)null : (decimal)Data.Offer.SimulationInputs.RealEstateInsurance.Sum;

                    var typCerpani = Data.Offer.SimulationInputs.DrawingTypeId.HasValue ? Data.DrawingTypeById.GetValueOrDefault(Data.Offer.SimulationInputs.DrawingTypeId.Value)?.StarbuildId : null;
                    var lhutaUkonceniCerpani = Data.Offer.SimulationInputs.DrawingDurationId.HasValue ? Data.DrawingDurationById.GetValueOrDefault(Data.Offer.SimulationInputs.DrawingDurationId.Value)?.DrawingDuration : null;

                    data = new
                    {
                        cislo_smlouvy = Data.Arrangement.ContractNumber,
                        case_id = Data.Arrangement.CaseId.ToJsonString(),
                        stav_zadosti = Data.SalesArrangementStatesById[Data.Arrangement.State].StarbuildId.ToJsonString(),
                        business_case_ID = Data.Arrangement.RiskBusinessCaseId,
                        risk_segment = Data.Arrangement.RiskSegment,
                        laa_id = Data.Arrangement.LoanApplicationAssessmentId,
                        datum_uzavreni_obchodu = Data.Arrangement.RiskBusinessCaseExpirationDate?.ToJsonString(),
                        kanal_ziskani = Data.Arrangement.ChannelId.ToJsonString(),
                        datum_vygenerovani_dokumentu = actualDate.ToJsonString(),                                                                    // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
                        datum_prvniho_podpisu = firstSignedDate.ToJsonString(),
                        uv_produkt = Data.ProductType.Id.ToJsonString(),
                        uv_druh = WhenFillKey(EJsonKey.UvDruh, Data.Offer.SimulationInputs.LoanKindId.ToJsonString()),
                        indikativni_LTV = Data.Offer.SimulationResults.LoanToValue.ToJsonString(),                                                   // OfferInstance
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
                        developer_id = WhenFillKey(EJsonKey.DeveloperId, Data.Offer.SimulationInputs.Developer?.DeveloperId.ToJsonString()),
                        developer_projekt_id = WhenFillKey(EJsonKey.DeveloperProjektId, Data.Offer.SimulationInputs.Developer?.ProjectId.ToJsonString()),
                        developer_popis = WhenFillKey(EJsonKey.DeveloperPopis, developerDescription),
                        forma_splaceni = 1.ToJsonString(),                                                                                          // [MOCK] OfferInstance (default 1) // ???  
                        seznam_mark_akci = Data.Offer.AdditionalSimulationResults.MarketingActions?.OrderBy(i => i.MarketingActionId).Select(i => MapMarketingAction(i)).ToArray() ?? Array.Empty<object>(),
                        seznam_poplatku = Data.Offer.AdditionalSimulationResults.Fees?.OrderBy(i => i.FeeId).Select(i => MapFee(i)).ToArray() ?? Array.Empty<object>(),              // Data.Offer.SimulationResults.Fees
                        seznam_ucelu = WhenFillKey(EJsonKey.SeznamUcelu, Data.Offer.SimulationInputs.LoanPurposes?.OrderBy(i => i.LoanPurposeId).Select(i => MapLoanPurpose(i)).ToArray() ?? Array.Empty<object>()),
                        seznam_objektu = WhenFillKey(EJsonKey.SeznamObjektu, Data.Arrangement.Mortgage?.LoanRealEstates.OrderBy(i => i.RealEstateTypeId).Select((i, index) => MapLoanRealEstate(i, index + 1)).ToArray() ?? Array.Empty<object>()),
                        seznam_ucastniku = customersOnSa?.OrderBy(i => i.CustomerOnSAId).Select(i => MapCustomerOnSA(i)).ToArray() ?? Array.Empty<object>(),
                        zprostredkovano_3_stranou = false.ToJsonString(),                                                                           // [MOCK] SalesArrangement - dle typu Usera (na offer zatím nemáme, dohodnuta mockovaná hodnota FALSE) // ???
                        sjednal_CPM = user_cpm,
                        sjednal_ICP = user_icp,
                        mena_prijmu = Data.Arrangement.Mortgage?.IncomeCurrencyCode,
                        mena_bydliste = Data.Arrangement.Mortgage?.ResidencyCurrencyCode,
                        zpusob_zasilani_vypisu = Data.Offer.BasicParameters.StatementTypeId.ToJsonString(),
                        predp_hodnota_nem_zajisteni = Data.Offer.SimulationInputs.CollateralAmount.ToJsonString(),
                        typ_cerpani = typCerpani.ToJsonString(),
                        lhuta_ukonceni_cerpani = lhutaUkonceniCerpani.ToJsonString(),
                        datum_garance_us = Data.Arrangement.OfferGuaranteeDateFrom.ToJsonString(),
                        garance_us_platnost_do = Data.Offer.BasicParameters.GuaranteeDateTo.ToJsonString(),
                        fin_kryti_vlastni_zdroje = WhenFillKey(EJsonKey.FinKrytiVlastniZdroje, Data.Offer.BasicParameters.FinancialResourcesOwn.ToJsonString()),                                  // OfferInstance
                        fin_kryti_cizi_zdroje = WhenFillKey(EJsonKey.FinKrytiCiziZdroje, Data.Offer.BasicParameters.FinancialResourcesOther.ToJsonString()),                                   // OfferInstance
                        fin_kryti_celkem = WhenFillKey(EJsonKey.FinKrytiCelkem, financialResourcesTotal.ToJsonString()),                                                                   // OfferInstance
                        zpusob_podpisu_smluv_dok = Data.Arrangement.Mortgage?.ContractSignatureTypeId.ToJsonString(),                                // Codebook SignatureTypes
                        zpusob_podpisu_zadosti = Data.Arrangement.Mortgage?.SalesArrangementSignatureTypeId.ToJsonString(),                          // Codebook SignatureTypes                        
                        souhlas_el_forma_komunikace = Data.Arrangement.Mortgage?.AgentConsentWithElCom.ToJsonString(),
                        seznam_domacnosti = households?.OrderBy(i => i.HouseholdTypeId).Select((i, index) => MapHousehold(i, index + 1)).ToArray() ?? Array.Empty<object>(),
                        zmocnenec_mp_id = FindZmocnenecMpId().ToJsonString(),

                        RZP_suma = insuranceSumRiskLife.ToJsonString(),
                        pojisteni_nem_suma = WhenFillKey(EJsonKey.PojisteniNemSuma, insuranceSumRealEstate.ToJsonString()),

                        zadaZvyhodneni = (Data.Offer.SimulationInputs.IsEmployeeBonusRequested == true).ToJsonString(),
                        datum_zahajeni_anuitniho_splaceni = Data.Offer.SimulationResults.AnnuityPaymentsDateFrom.ToJsonString(),
                        splatnost_uveru_datum = Data.Offer.SimulationResults.LoanDueDate.ToJsonString(),
                        pocet_anuitnich_splatek = Data.Offer.SimulationResults.AnnuityPaymentsCount.ToJsonString(),

                        business_id_formulare = formId,
                        seznam_id_formulare = seznamIdFormulare,
                        ea_kod = "608248",

                        //tests
                        cislo_dokumentu = MockDokumentId,                                                                                               // Pro D1.3 zůstává MOCK, bude se řešit v D1.4
                    };
                    break;

                case EFormType.F3602:
                    data = new
                    {
                        cislo_smlouvy = Data.Arrangement.ContractNumber,
                        case_id = Data.Arrangement.CaseId.ToJsonString(),
                        business_case_ID = Data.Arrangement.RiskBusinessCaseId,
                        datum_vytvoreni_zadosti = actualDate.ToJsonString(),
                        seznam_ucastniku = customersOnSa?.OrderBy(i => i.CustomerOnSAId).Select(i => MapCustomerOnSA(i)).ToArray() ?? Array.Empty<object>(),
                        sjednal_CPM = user_cpm,
                        sjednal_ICP = user_icp,
                        zpusob_podpisu_smluv_dok = Data.Arrangement.Mortgage?.ContractSignatureTypeId.ToJsonString(),
                        zmenovy_navrh = 0.ToJsonString(),                                                                                               // Default: 0, pouze v F3602
                        seznam_domacnosti = households?.OrderBy(i => i.HouseholdTypeId).Select((i, index) => MapHousehold(i, index + 1)).ToArray() ?? Array.Empty<object>(),

                        business_id_formulare = formId,
                        seznam_id_formulare = seznamIdFormulare,
                        ea_kod = "608243",

                        //tests
                        cislo_dokumentu = MockDokumentId,                                                                                               // Pro D1.3 zůstává MOCK, bude se řešit v D1.4
                    };
                    break;

            }

            var options = new JsonSerializerOptions
            {
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
