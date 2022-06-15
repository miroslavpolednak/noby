using System.Text.Json;

using CIS.Infrastructure.gRPC.CisTypes;

using DomainServices.SalesArrangementService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.CustomerService.Contracts;


namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement.Shared
{
    public class FormDataJsonBuilder
    {
        private static int[] FormIDs = new int[] { 3601001 };

        #region Construction

        public FormData Data { get; init; }

        public FormDataJsonBuilder(FormData data)
        {
            Data = data;
        }

        #endregion


        public Eas.EasWrapper.CheckFormData Build(int formId)
        {
            string? jsonData;

            switch (formId)
            {
                case 3601001:
                    jsonData = BuildJson_3601001();
                    break;

                default:
                    throw new CisArgumentException(99999, $"FormId #{formId} is not supported.", nameof(formId));  //TODO: ErrorCode
            }

            var formData = new Eas.EasWrapper.CheckFormData()
            {
                formular_id = 3601001,
                cislo_smlouvy = Data.Arrangement.ContractNumber,
                dokument_id = "9876543210", //???
                datum_prijeti = new DateTime(2022, 1, 1), //???
                data = jsonData,
            };

            return formData!;
        }

        private string BuildJson_3601001(bool ignoreNullValues = true)
        {
            var actualDate = DateTime.Now.Date;

            var householdsByCustomerOnSAId = Data.CustomersOnSa.ToDictionary(i => i.CustomerOnSAId, i => Data.Households.Where(h => h.CustomerOnSAId1 == i.CustomerOnSAId || h.CustomerOnSAId2 == i.CustomerOnSAId).ToArray());

            object? MapHousehold(Contracts.Household i)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    cislo_domacnosti = i.HouseholdTypeId.ToJsonString(),
                    pocet_deti_0_10let = i.Data.ChildrenUpToTenYearsCount.ToJsonString(),
                    pocet_deti_nad_10let = i.Data.ChildrenOverTenYearsCount.ToJsonString(),
                    sporeni = i.Expenses.SavingExpenseAmount.ToJsonString(),
                    pojisteni = i.Expenses.InsuranceExpenseAmount.ToJsonString(),
                    naklady_na_bydleni = i.Expenses.HousingExpenseAmount.ToJsonString(),
                    ostatni_vydaje = i.Expenses.OtherExpenseAmount.ToJsonString(),
                    vyporadani_majetku = i.Data.PropertySettlementId.ToJsonString(),
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
                    // ulice_dodatek =                                  // ??? OP!
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

            object? MapCustomerObligation(CustomerObligation i, int rowNumber)
            {
                if (i == null)
                {
                    return null;
                }

                return new
                {
                    cislo_zavazku = rowNumber.ToJsonString(),
                    druh_zavazku = i.ObligationTypeId.ToJsonString(),
                    vyse_splatky = i.InstallmentAmount.ToJsonString(),
                    vyse_nesplacene_jistiny = i.LoanPrincipalAmount.ToJsonString(),
                    vyse_limitu = i.CreditCardLimit.ToJsonString(),
                    mimo_entitu_mandanta = i.IsObligationCreditorExternal.ToJsonString()
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

                return new
                {
                    prvni_pracovni_sml_od = i.Employement?.Job?.FirstWorkContractSince.ToJsonString(),
                    posledni_zamestnani_od = actualDate.ToJsonString(),                         // [MOCK] aktuální datum (relevantní v tomto DROPu, poté bude ´posledni_zamestnani_od´ zrušeno)
                    poradi_prijmu = rowNumber.ToJsonString(),
                    zdroj_prijmu_hlavni = iil.IncomeTypeId.ToJsonString(),
                    typ_pracovniho_pomeru = i.Employement?.Job?.EmploymentTypeId.ToJsonString(),
                    klient_ve_vypovedni_lhute = i.Employement?.Job?.JobNoticePeriod.ToJsonString(),
                    klient_ve_zkusebni_lhute = i.Employement?.Job?.JobTrialPeriod.ToJsonString(),
                    //prijem_ze_zahranici = i.Employement?.IsForeignIncome.ToJsonString(),
                    //domicilace_prijmu_ze_zamestnani = i.Employement?.IsDomicile.ToJsonString(),
                    pracovni_smlouva_aktualni_od = i.Employement?.Job?.CurrentWorkContractSince.ToJsonString(),
                    pracovni_smlouva_aktualni_do = i.Employement?.Job?.CurrentWorkContractTo.ToJsonString(),
                    zamestnavatel_nazov = i.Employement?.Employer?.Name,
                    zamestnavatel_rc_ico = i.Employement?.Employer?.Cin,
                    //zamestnavatel_sidlo_ulice = i.Employement?.Employer?.Address?.Street,
                    //zamestnavatel_sidlo_cislo_popisne_orientacni = GetAddressNumber(i.Employement?.Employer?.Address),  //složit string ve formátu "BuildingIdentificationNumber/LandRegistryNumber"
                    //zamestnavatel_sidlo_mesto = i.Employement?.Employer?.Address?.City,
                    //zamestnavatel_sidlo_psc = i.Employement?.Employer?.Address?.Postcode.ToPostCodeJsonString(),
                    zamestnavatel_sidlo_stat = i.Employement?.Employer?.CountryId?.ToJsonString(),
                    //zamestnavatel_telefonni_cislo = i.Employement?.Employer?.PhoneNumber,
                    //zamestnavatel_okec = i.Employement?.Employer?.ClassificationOfEconomicActivitiesId.ToJsonString(),
                    //zamestnavatel_pracovni_sektor =  i.Employement?.Employer?.WorkSectorId.ToJsonString(),
                    //zamestnavatel_senzitivni_sektor =  i.Employement?.Employer?.SensitiveSector.ToJsonString(),
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
                    PEP = 0.ToJsonString(),                                                         // [MOCK] (default 0) OP!
                    seznam_adres = c.Addresses?.Select(i => MapAddress(i)).ToArray() ?? Array.Empty<object>(),
                    seznam_dokladu = cIdentificationDocuments,                                      // ??? mělo by to být pole, nikoliv jeden objekt ???
                    seznam_kontaktu = c.Contacts?.Select(i => MapContact(i)).ToArray() ?? Array.Empty<object>(),
                    rodinny_stav = c.NaturalPerson?.MaritalStatusStateId.ToJsonString(),
                    druh_druzka = i.HasPartner.ToJsonString(),
                    vzdelani = 3.ToJsonString(),                                                    // [MOCK] (default 3) OP!
                    prijmy = i.Incomes?.ToList().Select((i, index) => MapCustomerIncome(i, index + 1)).ToArray() ?? Array.Empty<object>(),
                    zavazky = i.Obligations?.ToList().Select((i, index) => MapCustomerObligation(i, index + 1)).ToArray() ?? Array.Empty<object>(),
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

                var household = householdsByCustomerOnSAId![i.CustomerOnSAId].First();  // ??? co když je stejné CustomerOnSAId ve vícero households
                return new
                {
                    role = i.CustomerRoleId.ToJsonString(),                     // CustomerOnSA
                    zmocnenec = 0.ToJsonString(),                               // [MOCK] CustomerOnSA (default 0)
                    cislo_domacnosti = household.HouseholdTypeId.ToJsonString(),// CustomerOnSA ??? brát Houshold.CustomerOnSAId (1 nebo 2)
                    klient = MapCustomer(i),
                };
            }


            // root

            var financialResourcesOwn = Data.Offer.BasicParameters.FinancialResourcesOwn.ToDecimal();
            var financialResourcesOther = Data.Offer.BasicParameters.FinancialResourcesOther.ToDecimal();
            decimal? financialResourcesTotal = (financialResourcesOwn.HasValue || financialResourcesOther.HasValue) ? ((financialResourcesOwn ?? 0) + (financialResourcesOther ?? 0)) : null;

            var data = new
            {
                cislo_smlouvy = Data.Arrangement.ContractNumber,
                case_id = Data.Arrangement.CaseId.ToJsonString(),
                business_case_ID = Data.Arrangement.RiskBusinessCaseId,                                                                      // SalesArrangement
                kanal_ziskani = Data.Arrangement.ChannelId.ToJsonString(),                                                                   // SalesArrangement - vyplněno na základě usera
                datum_vytvoreni_zadosti = actualDate.ToJsonString(),                                                                    // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
                datum_prvniho_podpisu = actualDate.ToJsonString(),                                                                      // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
                                                                                                                                        //uv_produkt = Data.Offer.ProductTypeId.ToJsonString(),                                                                      // ??? SalesArrangement nemá být z OfferProductTypeId ???
                uv_produkt = Data.ProductType.Id.ToJsonString(),
                uv_druh = Data.Offer.SimulationInputs.LoanKindId.ToJsonString(),                                                             // OfferInstance
                indikativni_LTV = Data.Offer.SimulationResults.LoanToValue.ToJsonString(),                                                   // OfferInstance
                indikativni_LTC = Data.Arrangement.LoanToCost.ToJsonString(),                                                                // OfferInstance -> SalesArrangement !!! moved from offer to arrangement in D1-2
                seznam_mark_akci = Array.Empty<object>(),                                                                               // [MOCK] OfferInstance (default empty array)
                individualni_sleva_us = 0.ToJsonString(),                                                                               // [MOCK] OfferInstance (default 0)
                sazba_vyhlasovana = Data.Offer.SimulationResults.LoanInterestRateAnnounced.ToJsonString(),                                   // OfferInstance
                sazba_skladacka = Data.Offer.SimulationResults.LoanInterestRate.ToJsonString(),                                              // OfferInstance
                sazba_poskytnuta = Data.Offer.SimulationResults.LoanInterestRate.ToJsonString(),                                             // OfferInstance = sazba_skladacka
                vyhlasovanaTyp = Data.Offer.SimulationResults.LoanInterestRateAnnouncedType.ToJsonString(),                                  // OfferInstance 
                vyse_uveru = Data.Offer.SimulationResults.LoanAmount.ToJsonString(),                                                         // OfferInstance
                anuitni_splatka = Data.Offer.SimulationResults.LoanPaymentAmount.ToJsonString(),                                             // OfferInstance
                splatnost_uv_mesice = Data.Offer.SimulationResults.LoanDuration.ToJsonString(),                                              // OfferInstance (kombinace dvou vstupů roky + měsíce na FE)
                fixace_uv_mesice = Data.Offer.SimulationInputs.FixedRatePeriod.ToJsonString(),                                               // OfferInstance - na FE je to v rocích a je to číselník ?
                individualni_cenotvorba_odchylka = Data.Offer.SimulationInputs.InterestRateDiscount.ToJsonString(),
                predp_termin_cerpani = Data.Arrangement.Mortgage?.ExpectedDateOfDrawing.ToJsonString(),                                      // SalesArrangement 
                den_splaceni = Data.Offer.SimulationInputs.PaymentDay.ToJsonString(),                                                        // OfferInstance
                forma_splaceni = 1.ToJsonString(),                                                                                      // [MOCK] OfferInstance (default 1)  
                seznam_poplatku = Data.Offer.SimulationResults.Fees?.Select(i => MapFee(i)).ToArray() ?? Array.Empty<object>(),              // Data.Offer.SimulationResults.Fees
                seznam_ucelu = Data.Offer.SimulationInputs.LoanPurposes?.Select(i => MapLoanPurpose(i)).ToArray() ?? Array.Empty<object>(),  // OfferInstance - 1..5 ??? má se brát jen prvních 5 účelů ?
                seznam_objektu = Data.Arrangement.Mortgage?.LoanRealEstates.ToList().Select((i, index) => MapLoanRealEstate(i, index + 1)).ToArray() ?? Array.Empty<object>(), // SalesArrangement - 0..3 ???
                seznam_ucastniku = Data.CustomersOnSa?.Select(i => MapCustomerOnSA(i)).ToArray() ?? Array.Empty<object>(),                   // CustomerOnSA, Customer
                zprostredkovano_3_stranou = false.ToJsonString(),                                                                       // [MOCK] SalesArrangement - dle typu Usera (na offer zatím nemáme, dohodnuta mockovaná hodnota FALSE)
                sjednal_CPM = Data.User!.CPM,                                                                                                // User
                sjednal_ICP = Data.User!.ICP,                                                                                                // User
                                                                                                                                        // VIP_makler = 0.ToJsonString(),                                                                                       // [MOCK] User (default 0) !!! removed in D1-2
                mena_prijmu = Data.Arrangement.Mortgage?.IncomeCurrencyCode,                                                                 // SalesArrangement
                mena_bydliste = Data.Arrangement.Mortgage?.ResidencyCurrencyCode,                                                            // SalesArrangement

                // zpusob_zasilani_vypisu = Data.Offer.SimulationResults.StatementTypeId.ToJsonString(),                                     // Offerinstance !!! removed in D1-2 (moved to fees without DV mapping)
                predp_hodnota_nem_zajisteni = Data.Offer.SimulationInputs.CollateralAmount.ToJsonString(),                                   // Offerinstance
                garance_us_platnost_do = Data.Offer.BasicParameters.GuaranteeDateTo.ToJsonString(),                                          // Data.Offer.BasicParameters.GuaranteeDateTo
                fin_kryti_vlastni_zdroje = Data.Offer.BasicParameters.FinancialResourcesOwn.ToJsonString(),                                  // OfferInstance
                fin_kryti_cizi_zdroje = Data.Offer.BasicParameters.FinancialResourcesOther.ToJsonString(),                                   // OfferInstance
                fin_kryti_celkem = financialResourcesTotal.ToJsonString(),                                                              // OfferInstance
                zpusob_podpisu_smluv_dok = Data.Arrangement.Mortgage?.SignatureTypeId.ToJsonString(),                                        // SalesArrangement
                seznam_domacnosti = Data.Households?.Select(i => MapHousehold(i)).ToArray() ?? Array.Empty<object>(),

                // other mandatory fields in JSON:
                parametr_domicilace = 1.ToJsonString(),
                parametr_RZP = 1.ToJsonString(),
                parametr_pojisteni_nem = 1.ToJsonString(),
                parametr_vyse_prijmu_uveru = 1.ToJsonString(),
            };

            var options = new JsonSerializerOptions { DefaultIgnoreCondition = ignoreNullValues ? System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull : System.Text.Json.Serialization.JsonIgnoreCondition.Never };
            var json = JsonSerializer.Serialize(data, options);

            return json;
        }


        #region SampleFormData

        public static Eas.EasWrapper.CheckFormData BuildSampleFormData(int formId = 3601001)
        {
            if (!FormIDs.Contains(formId))
            {
                throw new CisArgumentException(99999, $"FormId #{formId} is not supported.", nameof(formId));  //TODO: ErrorCode
            }

            Eas.EasWrapper.CheckFormData formData = null;

            switch (formId)
            {
                case 3601001:
                    formData = BuildSampleFormData_3601001();
                    break;
            }

            return formData!;
        }

        private static Eas.EasWrapper.CheckFormData BuildSampleFormData_3601001()
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
