using System.Globalization;
using System.Text.Json;

using CIS.Core.Results;
using Grpc.Core;
using CIS.Infrastructure.gRPC;

using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.CustomerService.Contracts;
using DomainServices.UserService.Contracts;

using DomainServices.CodebookService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.CustomerService.Abstraction;

using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;
using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;

using DomainServices.SalesArrangementService.Api;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class SendToCmpHandler
    : IRequestHandler<Dto.SendToCmpMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ICustomerServiceAbstraction _customerService;

    private readonly Repositories.NobyRepository _repository;
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly IMediator _mediator;

    public SendToCmpHandler(
        ICodebookServiceAbstraction codebookService,
        ICaseServiceAbstraction caseService,
        IOfferServiceAbstraction offerService,
        ICustomerServiceAbstraction customerService,
        Repositories.NobyRepository repository,
        ILogger<SendToCmpHandler> logger,
        Eas.IEasClient easClient,
        IMediator mediator)
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _offerService = offerService;
        _customerService = customerService;
        _repository = repository;
        _logger = logger;
        _easClient = easClient;
        _mediator = mediator;
    }

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.SendToCmpMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(SendToCmpHandler), request.SalesArrangementId);

        // load SalesArrangement
        var _arrangement = await _mediator.Send(new Dto.GetSalesArrangementMediatrRequest(new GetSalesArrangementRequest { SalesArrangementId = request.SalesArrangementId }), cancellation);

        // check if SalesArrangementType is Mortgage
        var productTypeCategory = await GetProductTypeCategory(_arrangement.SalesArrangementTypeId);
        if (productTypeCategory != ProductTypeCategory.Mortgage)
        {
            throw new CisArgumentException(1, $"SalesArrangementTypeId '{_arrangement.SalesArrangementTypeId}' doesn't match ProductTypeCategory '{ProductTypeCategory.Mortgage}'.", nameof(request));
        }

        var householdsByType = await GetHouseholdsByType(request.SalesArrangementId, cancellation);

        //arrangement.ContractNumber = String.Empty;

        // update ContractNumber if not specified
        if (String.IsNullOrEmpty(_arrangement.ContractNumber))
        {
            // pro EAS.Get_ContractNumber se jako ´clientId´ vezme ´CustomerOnSAId1´ z domácnosti ´Debtor´?
            // ... co když taková na SA není, nebo jich je naopak více?
            // ... co když je nalezen právě jedna domácnost, ale nemá vyplněné ´CustomerOnSAId1´?
            if (!householdsByType.ContainsKey(CIS.Foms.Enums.HouseholdTypes.Main)) //opravit číselník (Main)
            {
                throw new CisValidationException(99999, $"Sales arrangement {request.SalesArrangementId} contains no household of type {CIS.Foms.Enums.HouseholdTypes.Main}."); //TODO: ErrorCode
            }

            var debtorHousehold = householdsByType[CIS.Foms.Enums.HouseholdTypes.Main].First();

            if (!debtorHousehold.CustomerOnSAId1.HasValue)
            {
                throw new CisValidationException(99999, $"Household´s CustomerOnSAId1 not defined'{debtorHousehold.HouseholdId}'."); //TODO: ErrorCode
            }

            // ziskat caseId
            //TODO: [_easClient.GetContractNumber] Kanálu požadavku skončila platnost při pokusu o odeslání po 00:00:05. Zvyšte hodnotu časového limitu předanou volání požadavku, nebo zvyšte hodnotu SendTimeout na vazbě. Čas přidělený této operaci byl pravděpodobně částí delšího časového limitu.

            // client ID . . . CustomerOnSa with MP identity Identity.Id

            var contractNumber = resolveGetContractNumber(await _easClient.GetContractNumber(debtorHousehold.CustomerOnSAId1.Value, (int)_arrangement.CaseId));
            //var contractNumber = $"{debtorHousehold.CustomerOnSAId1.Value}_{arrangement.CaseId}";

            await UpdateSalesArrangement(request.SalesArrangementId, contractNumber, cancellation);
            await UpdateCase(_arrangement.CaseId, contractNumber, cancellation);

            _arrangement.ContractNumber = contractNumber;
        }

        var _case = ServiceCallResult.ResolveToDefault<Case>(await _caseService.GetCaseDetail(_arrangement.CaseId, cancellation))
           ?? throw new CisNotFoundException(16002, $"Case ID #{_arrangement.CaseId} does not exist.");

        //User load from:
        //arrangement.Created.UserId

        //var _offer = await GetOfferMortgage(arrangement.OfferId, cancellation);

        var _offer = !_arrangement.OfferId.HasValue ? null :
            ServiceCallResult.ResolveToDefault<GetMortgageDataResponse>(await _offerService.GetMortgageData(_arrangement.OfferId.Value, cancellation))
                ?? throw new CisNotFoundException(16001, $"Offer ID #{_arrangement.OfferId} does not exist.");

        // customerId???
        // get customer from customerOnSa with KB identity
        var customerOnSAId = householdsByType.ContainsKey(CIS.Foms.Enums.HouseholdTypes.Main) ? householdsByType[CIS.Foms.Enums.HouseholdTypes.Main].First().CustomerOnSAId1 : null;
        //var customerOnSAIdentity = customerOnSAId.HasValue ? new CIS.Infrastructure.gRPC.CisTypes.Identity(customerOnSAId, CIS.Foms.Enums.IdentitySchemes.Kb) : null;
        var customerOnSAIdentity = customerOnSAId.HasValue ? new CIS.Infrastructure.gRPC.CisTypes.Identity(customerOnSAId, CIS.Foms.Enums.IdentitySchemes.Unknown) : null;


        // přímo v SA service
        //var _customer = !customerOnSAId.HasValue ? null :
        //    ServiceCallResult.ResolveToDefault<CustomerService.Contracts.CustomerResponse>(await _customerService.GetCustomerDetail(new CustomerService.Contracts.CustomerRequest { Identity = customerOnSAIdentity }, cancellation))
        //        ?? throw new CisNotFoundException(16001, $"Customer identity   #[{customerOnSAIdentity?.IdentityId}, {customerOnSAIdentity?.IdentityScheme}] does not exist.");  //TODO: ErrorCode


        var households = new List<Contracts.Household>(); //TODO: load households

        // create JSON data
        var jsonData = CreateJsonData(_arrangement, _offer, _case, null, null, households);

        // save form to DB
        await SaveForm(_arrangement.ContractNumber, jsonData, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    #region Data (loading & modifications)

    //private async Task<OfferService.Contracts.GetMortgageDataResponse> GetOfferMortgage(int? offerId, CancellationToken cancellation)
    //{
    //    if (!offerId.HasValue)
    //    {
    //        return null;
    //    }

    //    var response = ServiceCallResult.ResolveToDefault<OfferService.Contracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(offerId.Value, cancellation))
    //            ?? throw new CisNotFoundException(16001, $"Offer ID #{offerId} does not exist.");

    //    return response;
    //}

    private async Task<ProductTypeCategory> GetProductTypeCategory(int salesArrangementTypeId)
    {
        var salesArrangementType = (await _codebookService.SalesArrangementTypes()).FirstOrDefault(t => t.Id == salesArrangementTypeId);

        if (salesArrangementType == null)
        {
            throw new CisNotFoundException(99999, nameof(SalesArrangementTypeItem), salesArrangementTypeId); //TODO: ErrorCode
        }

        if (!salesArrangementType.ProductTypeId.HasValue)
        {
            throw new CisArgumentNullException(7, $"Sales arrangement type '{salesArrangementTypeId}' with undefined product type", nameof(salesArrangementTypeId));
        }

        var productType = (await _codebookService.ProductTypes()).FirstOrDefault(t => t.Id == salesArrangementType.ProductTypeId);

        if (productType == null)
        {
            throw new CisNotFoundException(13014, nameof(ProductTypeItem), salesArrangementType.ProductTypeId.Value);
        }

        return productType.ProductCategory;
    }

    private async Task<Dictionary<CIS.Foms.Enums.HouseholdTypes, List<Contracts.Household>>> GetHouseholdsByType(int salesArrangementId, CancellationToken cancellation)
    {
        var households = (await _mediator.Send(new Dto.GetHouseholdListMediatrRequest(salesArrangementId), cancellation)).Households.ToList();
        var householdTypes = await _codebookService.HouseholdTypes(cancellation);

        var householdTypesById = householdTypes.ToDictionary(i => i.Id);

        var householdsByType = new Dictionary<CIS.Foms.Enums.HouseholdTypes, List<Contracts.Household>>();

        households.ForEach(household =>
        {
            var type = householdTypesById[household.HouseholdTypeId];

            if (!householdsByType.ContainsKey(type.Value))
            {
                householdsByType.Add(type.Value, new List<Contracts.Household>());
            }

            householdsByType[type.Value].Add(household);
        });

        return householdsByType;
    }

    private static string resolveGetContractNumber(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<string> r => r.Model,
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };

    private async Task UpdateSalesArrangement(int salesArrangementId, string contractNumber, CancellationToken cancellation)
    {
        await _mediator.Send(new Dto.UpdateSalesArrangementMediatrRequest(new UpdateSalesArrangementRequest { SalesArrangementId = salesArrangementId, ContractNumber = contractNumber }), cancellation);
    }

    private async Task UpdateCase(long caseId, string contractNumber, CancellationToken cancellation)
    {
        var entity = ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.GetCaseDetail(caseId, cancellation))
         ?? throw new CisNotFoundException(16002, $"Case ID #{caseId} does not exist.");

        var data = new CaseService.Contracts.CaseData(entity.Data);
        data.ContractNumber = contractNumber;

        ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.UpdateCaseData(caseId, data, cancellation));
    }

    private async Task SaveForm(string contractNumber, string jsonData, CancellationToken cancellation)
    {
        var count = await _repository.GetFormsCount(cancellation);

        var id = count + 1;

        var docmentId = GenerateDocmentId(id);
        var formId = GenerateFormId(id);

        // save to DB
        var entity = new Repositories.Entities.FormInstanceInterface()
        {
            DOKUMENT_ID = docmentId,
            TYP_FORMULARE = "3601A",
            CISLO_SMLOUVY = contractNumber,
            STATUS = 100,
            DRUH_FROMULARE = 'N',
            FORMID = formId,
            CPM = String.Empty,                 // add from user
            ICP = String.Empty,                 // add from user
            CREATED_AT = DateTime.Now,          // what time zone?
            HESLO_KOD = "600248",
            STORNOVANO = 0,
            TYP_DAT = 1,
            JSON_DATA_CLOB = jsonData
        };

        await _repository.CreateForm(entity, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.FormInstanceInterface), long.Parse(formId, CultureInfo.InvariantCulture));
    }

    #endregion

    #region Form data
    private static string GenerateDocmentId(int id, int length = 30)
    {
        var prefix = "KBHNB";
        var sId = id.ToString(CultureInfo.InvariantCulture).PadLeft(length - prefix.Length, '0');
        return prefix + sId;
    }

    private static string GenerateFormId(int id, int length = 13)
    {
        // Numeric(13) ... nedoplňovat nulama!
        var prefix = "1";
        var suffix = "01";
        var sId = id.ToString(CultureInfo.InvariantCulture).PadLeft(length - suffix.Length - prefix.Length, '0');
        return prefix + sId + suffix;
    }

    private static string CreateJsonData(SalesArrangement arrangement, GetMortgageDataResponse offer, Case? caseData, User? user, CustomerResponse? customer, List<Contracts.Household> households)
    {
        // loan purposes
        // offer.Inputs.LoanPurpose.Add(new LoanPurpose { LoanPurposeId = 1, Sum = 10000000 }); (test)
        var offerLoanPurposes = offer.Inputs.LoanPurpose.Select(i => new {
            uv_ucel = i.LoanPurposeId.ToJsonString(),
            uv_ucel_suma = i.Sum.ToJsonString()
        }).ToArray();

        // households
        var arrangementHouseholds = households.Select(i => new {
            cislo_domacnosti = i.HouseholdTypeId.ToJsonString(),
            pocet_deti_0_10let = i.Data.ChildrenUpToTenYearsCount.ToJsonString(),
            pocet_deti_nad_10let = i.Data.ChildrenOverTenYearsCount.ToJsonString(),
            sporeni = i.Expenses.SavingExpenseAmount.ToJsonString(),
            pojisteni = i.Expenses.InsuranceExpenseAmount.ToJsonString(),
            naklady_na_bydleni = i.Expenses.HousingExpenseAmount.ToJsonString(),
            ostatni_vydaje = i.Expenses.OtherExpenseAmount.ToJsonString(),
            vyporadani_majetku = i.Data.PropertySettlementId.ToJsonString(),
        }).ToArray();

        // root
        var actualDate = DateTime.Now.Date;
        var data = new {
            cislo_smlouvy = arrangement.ContractNumber,
            case_id = arrangement.CaseId,
            // business_case_ID = arrangement.RiskBusinessCaseId,                                                                   // SalesArrangement - na základě volání RBC ??? na SA zatím nemáme (chybí implementace ?)
            kanal_ziskani = arrangement.ChannelId.ToJsonString(),                                                                   // SalesArrangement - vyplněno na základě usera
            datum_vytvoreni_zadosti = actualDate,                                                                                   // SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
            datum_prvniho_podpisu = actualDate,                                                                                     // SalesArrangement - byla domluva posílat pro D1.1 aktuální datum

            uv_produkt = offer.ProductTypeId.ToJsonString(),                                                                        // ??? SalesArrangement nemá být z OfferProductTypeId ???
            uv_druh = offer.Inputs.LoanKindId.ToJsonString(),                                                                       // OfferInstance
            indikativni_LTV = offer.Outputs.LoanToValue.ToJsonString(),                                                             // OfferInstance
            indikativni_LTC = offer.Outputs.LoanToCost.ToJsonString(),                                                              // OfferInstance
            // seznam_mark_akci =                                                                                                   // OfferInstance ??? na offer zatím nemáme     
            // individualni_sleva_us =                                                                                              // OfferInstance - default 0 ??? na offer zatím nemáme
            // garance_us =                                                                                                         // OfferInstance - default 0 ??? na offer zatím nemáme
            sazba_vyhlasovana = offer.Outputs.InterestRateAnnounced.ToJsonString(),                                                 // OfferInstance
            sazba_skladacka = offer.Outputs.LoanInterestRate.ToJsonString(),                                                        // OfferInstance
            sazba_poskytnuta = offer.Outputs.LoanInterestRate.ToJsonString(),                                                       // OfferInstance = sazba_skladacka
            // vyhlasovanaTyp =                                                                                                     // OfferInstance ??? nenalezeno (v popisu na Confluence)             
            vyse_uveru = offer.Outputs.LoanAmount.ToJsonString(),                                                                   // OfferInstance
            anuitni_splatka = offer.Outputs.LoanPaymentAmount.ToJsonString(),                                                       // OfferInstance
            splatnost_uv_mesice = offer.Outputs.LoanDuration.ToJsonString(),                                                        // OfferInstance (kombinace dvou vstupů roky + měsíce na FE)
            fixace_uv_mesice = offer.Inputs.FixedRatePeriod.ToJsonString(),                                                         // OfferInstance - na FE je to v rocích a je to číselník ?
            // predp_termin_cerpani = arrangement.ExpectedDateOfDrawing,                                                            // SalesArrangement ??? na SA zatím nemáme (chybí implementace ?)
            den_splaceni = offer.Outputs.PaymentDayOfTheMonth.ToJsonString(),                                                       // OfferInstance default=15
            // forma_splaceni =                                                                                                     // OfferInstance default = inkaso  ??? na offer zatím nemáme 
            // seznam_poplatku =                                                                                                    // OfferInstance - celý objekt vůbec nebude - TBD - diskuse k simulaci ??? na offer zatím nemáme
            seznam_ucelu = offerLoanPurposes,                                                                                       // OfferInstance - 1..5 ??? má se brát jen prvních 5 účelů ?
            // seznam_objektu =                                                                                                     // SalesArrangement - 0..3 ??? na SA zatím nemáme (chybí implementace ?)
            // seznam_ucastniku =                                                                   // CustomerOnSA, Customer ???

            // zprostredkovano_3_stranou                                                                                            // SalesArrangement - dle typu Usera ???
            // sjednal_CPM = user.CPM,                                                                 // User
            // sjednal_ICP = user.ICP,                                                                 // User
            // mena_prijmu = arrangement.IncomeCurrencyCode                                                                         // SalesArrangement ??? na SA zatím nemáme (chybí implementace ?)
            // mena_bydliste = arrangement.ResidencyCurrencyCode                                                                    // SalesArrangement ??? na SA zatím nemáme (chybí implementace ?)

            zpusob_zasilani_vypisu = offer.Outputs.StatementTypeId.ToJsonString(),                                                  // Offerinstance
            predp_hodnota_nem_zajisteni = offer.Inputs.CollateralAmount.ToJsonString(),                                             // Offerinstance
            // fin_kryti_vlastni_zdroje =                                                                                           // OfferInstance ??? na offer zatím nemáme
            // fin_kryti_cizi_zdroje =                                                                                              // OfferInstance ??? na offer zatím nemáme
            // fin_kryti_celkem =                                                                                                   // OfferInstance ??? na offer zatím nemáme
            // zpusob_podpisu_smluv_dok =                                                                                           // SalesArrangement  ??? na SA zatím nemáme
            seznam_domacnosti = arrangementHouseholds,
        };   

        var json = JsonSerializer.Serialize(data);

        return json;

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
    }

    #endregion
}

/*
Pokud na entitě SalesArrangement není číslo smlouvy, pak proběhne provolání StarBuild - getContractNumber (relevantní pro Drop 1.1, pak dojde k přesunu na Document service)
	EAS voláním dojde k získání čísla smlouvy
	???  metoda EAS.Get_ContractNumberAsync požaduje kromě ´caseId´ rovněž ´clientId´, kde to vezmu? následně:
	---------------------------------
	getHouseholdList ( https://wiki.kb.cz/confluence/display/HT/getHouseholdList?src=contextnavpagetreemode )
	HouseholdTypeId = hlavní domácnost
	CustomerOnSAId1 na householdu
	getCustomer (onSA) ( https://wiki.kb.cz/confluence/pages/viewpage.action?pageId=426147951&src=contextnavpagetreemode )
	CustomerIdentifiers. [ ]
	Identity pro IdentityScheme = MPSBID

	HouseHold: customerOnSa1, customerOnSa2 (1 = hlavní, 2 = spoludlužník)
	---------------------------------
	uložení na entitě SalesArrangementu voláním updateSalesArrangement
	uložení na entitě Case voláním updateCaseData
	???  metoda CaseService.UpdateCaseData požaduje kromě ´ContractNumber´ rovněž ´ProductTypeId´ a ´TargetAmount´, tyto položky se také nějak updatují?(getCase a poslat tatáž data)


        //JObject o = JObject.FromObject(new
        //{
        //    channel = new
        //    {
        //        title = "Star Wars",
        //        link = "http://www.starwars.com",
        //        description = "Star Wars blog.",
        //        item =
        //    from p in posts
        //    orderby p.Title
        //    select new
        //    {
        //        title = p.Title,
        //        description = p.Description,
        //        link = p.Link,
        //        category = p.Categories
        //    }
        //    }
        //});


    -------------------------------------------------------------------------------------------------------------
    - získání ClientId pro GetContractNumber:
        - vytáhnout Housholds, které jsou obsaženy v SA
        - z household vzít household typu 'Main' (vždy musí obsahovat household typu Main, pokud ne, je to chyba)
        - z household (Main) vytáhnout CustomerOnSA podle CustomerOnSAId1
        - z CustomerOnSA.Identities vytáhnout identitu se schematem MP (musí obsahovat jednu MP a jednu KB identitu, jinak chyba)
        - z CustomerOnSAIdentity použít 'Id' pro 'ClientId'

    - získání dat o customer:
        - vytáhnout z CustomerService obdobně jako u 'ClientId', avšak použít KB identitu z CustomerOnSA.Identities

    Notes: zajímá nás pouze hlavní domácnost a z ní CustomerOnSAId1
    -------------------------------------------------------------------------------------------------------------

    [Včera 9:32] Herych Jan EX          MPSS
    getnout customera on SA pomoci CustomerOnSAId1

    [Včera 9:32] Herych Jan EX          MPSS
    zafiltrovat v jeho identitach na MP identitu

    [Včera 9:33] Herych Jan EX          MPSS
    a ID teto identity poslat do EAS jako partnerId

    -------------------------------------------------------------------------------------------------------------

 */