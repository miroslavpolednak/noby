using System.Globalization;
using System.Text.Json;

using Grpc.Core;
using CIS.Infrastructure.gRPC.CisTypes;

using DomainServices.CodebookService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.CustomerService.Contracts;
using DomainServices.UserService.Contracts;

using DomainServices.CodebookService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using DomainServices.UserService.Abstraction;

using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;
using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;


namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class SendToCmpHandler
    : IRequestHandler<Dto.SendToCmpMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    private static readonly string StringJoinSeparator = ",";

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly IUserServiceAbstraction _userService;

    private readonly Repositories.NobyRepository _repository;
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly IMediator _mediator;

    public SendToCmpHandler(
        ICodebookServiceAbstraction codebookService,
        ICaseServiceAbstraction caseService,
        IOfferServiceAbstraction offerService,
        ICustomerServiceAbstraction customerService,
        IUserServiceAbstraction userService,
        Repositories.NobyRepository repository,
        ILogger<SendToCmpHandler> logger,
        Eas.IEasClient easClient,
        IMediator mediator)
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _offerService = offerService;
        _customerService = customerService;
        _userService = userService;
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
        var arrangement = await _mediator.Send(new Dto.GetSalesArrangementMediatrRequest(new GetSalesArrangementRequest { SalesArrangementId = request.SalesArrangementId }), cancellation);

        // check if SalesArrangementType is Mortgage
        var productType = await GetProductType(arrangement.SalesArrangementTypeId, cancellation);
        if (productType.ProductCategory != ProductTypeCategory.Mortgage)
        {
            throw new CisArgumentException(1, $"SalesArrangementTypeId '{arrangement.SalesArrangementTypeId}' doesn't match ProductTypeCategory '{ProductTypeCategory.Mortgage}'.", nameof(request));
        }

        // check if Offer exists
        if (!arrangement.OfferId.HasValue)
            throw new CisNotFoundException(16000, $"Sales Arrangement #{request.SalesArrangementId} is not linked to Offer");

        // TODO: Některé validace se týkají pouze DROPu 1 !!!

        // load customers on SA and validate them
        var customersOnSA = await GetCustomersOnSA(arrangement.SalesArrangementId, cancellation);
        CheckCustomersOnSA(customersOnSA);   // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!

        // load households and validate them
        var households = (await _mediator.Send(new Dto.GetHouseholdListMediatrRequest(arrangement.SalesArrangementId), cancellation)).Households.ToList();
        var householdTypesById = (await _codebookService.HouseholdTypes(cancellation)).ToDictionary(i => i.Id);
        CheckHouseholds(households, householdTypesById, customersOnSA);

        // load case
        var _case = ServiceCallResult.ResolveToDefault<Case>(await _caseService.GetCaseDetail(arrangement.CaseId, cancellation))
            ?? throw new CisNotFoundException(16002, $"Case ID #{arrangement.CaseId} does not exist.");

        // update ContractNumber if not specified
        // arrangement.ContractNumber = String.Empty;
        if (String.IsNullOrEmpty(arrangement.ContractNumber))
        {
            var identityMP = GetMainMpIdentity(households, householdTypesById, customersOnSA);
            var contractNumber = ResolveGetContractNumber(await _easClient.GetContractNumber(identityMP.IdentityId, (int)arrangement.CaseId));
            await UpdateSalesArrangement(arrangement, contractNumber, cancellation);
            await UpdateCase(_case, contractNumber, cancellation);
        }

        // Offer load
        var _offer = ServiceCallResult.ResolveToDefault<GetMortgageDataResponse>(await _offerService.GetMortgageData(arrangement.OfferId!.Value, cancellation))
            ?? throw new CisNotFoundException(99999, $"Offer ID #{arrangement.OfferId} does not exist."); //TODO: ErrorCode

        // User load (by arrangement.Created.UserId)
        var _user = ServiceCallResult.ResolveToDefault<User>(await _userService.GetUser(arrangement.Created.UserId, cancellation))
            ?? throw new CisNotFoundException(99999, $"User ID #{arrangement.Created.UserId} does not exist."); //TODO: ErrorCode
        
        // load customers
        var customersByIdentityCode = await GetCustomersByIdentityCode(customersOnSA, cancellation);

        // load incomes
        var incomesById = await GetIncomesById(customersOnSA, cancellation);

        // Load codebooks
        var academicDegreesBeforeById = (await _codebookService.AcademicDegreesBefore(cancellation)).ToDictionary(i => i.Id);
        var gendersById = (await _codebookService.Genders(cancellation)).ToDictionary(i => i.Id);

        // create JSON data
        var jsonData = CreateJsonData(arrangement, productType, _offer, _case, _user, households, customersOnSA, incomesById, customersByIdentityCode, academicDegreesBeforeById, gendersById);

        // save form to DB
        await SaveForm(arrangement.ContractNumber, jsonData, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    #region Data (loading & modifications)

    private static void CheckCustomersOnSA(List<Contracts.CustomerOnSA> customersOnSa)
    {
        // check if each customer contains Mp identity and also Kb identity
        var customerIds = customersOnSa.Select(x => x.CustomerOnSAId);

        var customerIdentities = customersOnSa.SelectMany(c => c.CustomerIdentifiers.Select(i => new { CustomerOnSAId = c.CustomerOnSAId, Identity = i }));

        var customerIdsWithIdentityMp = customerIdentities.Where(i => i.Identity.IdentityScheme == Identity.Types.IdentitySchemes.Mp).Select(i => i.CustomerOnSAId);
        var customerIdsWithIdentityKb = customerIdentities.Where(i => i.Identity.IdentityScheme == Identity.Types.IdentitySchemes.Kb).Select(i => i.CustomerOnSAId);

        var customerIdsWithoutIdentityMp = customerIds.Where(id => !customerIdsWithIdentityMp.Contains(id));
        var customerIdsWithoutIdentityKb = customerIds.Where(id => !customerIdsWithIdentityKb.Contains(id));

        var customerIdsInvalid = customerIdsWithoutIdentityMp.Concat(customerIdsWithoutIdentityKb).ToList();

        if (customerIdsInvalid.Any())
        {
            throw new CisValidationException(99999, $"Sales arrangement customers [{ String.Join(StringJoinSeparator, customerIdsInvalid) }] don't contain both [KB,MP] identities."); //TODO: ErrorCode
        }
    }

    private static void CheckHouseholds(List<Contracts.Household> households, Dictionary<int, HouseholdTypeItem> householdTypesById, List<Contracts.CustomerOnSA> customersOnSa)
    {
        // check if each household type is represented at most once
        var duplicitHouseholdTypeIds = households.GroupBy(i => i.HouseholdTypeId).Where(g => g.Count() > 1).Select(i => i.Key);
        if (duplicitHouseholdTypeIds.Any())
        {
            throw new CisValidationException(99999, $"Sales arrangement contains duplicit household types [{ String.Join(StringJoinSeparator, duplicitHouseholdTypeIds) }]."); //TODO: ErrorCode
        }

        // check if MAIN household is available
        var mainHouseholdCount = households.Count(i => householdTypesById[i.HouseholdTypeId].Value == CIS.Foms.Enums.HouseholdTypes.Main);
        if (mainHouseholdCount != 1)
        {
            throw new CisValidationException(99999, $"Sales arrangement must contain just one '{CIS.Foms.Enums.HouseholdTypes.Main}' household."); //TODO: ErrorCode
        }

        // check if any household contains CustomerOnSAId2 without CustomerOnSAId1
        var invalidHouseholdIds = households.Where(i => !i.CustomerOnSAId1.HasValue && i.CustomerOnSAId2.HasValue).Select(i => i.HouseholdId);
        if (invalidHouseholdIds.Any())
        {
            throw new CisValidationException(99999, $"Sales arrangement contains households [{ String.Join(StringJoinSeparator, invalidHouseholdIds) }] with CustomerOnSAId2 but without CustomerOnSAId1."); //TODO: ErrorCode
        }

        // check if CustomerOnSAId1 is available on Main households
        var mainHousehold = households.Single(i => householdTypesById[i.HouseholdTypeId].Value == CIS.Foms.Enums.HouseholdTypes.Main);
        if (!mainHousehold.CustomerOnSAId1.HasValue)
        {
            throw new CisValidationException(99999, $"Main household´s CustomerOnSAId1 not defined [{mainHousehold.HouseholdId}]."); //TODO: ErrorCode
        }

        // check if the same CustomerOnSA belongs to only one household
        var duplicitCustomerOnSAIds = households.Where(i => i.CustomerOnSAId1.HasValue).Select(i => i.CustomerOnSAId1!.Value)
           .Concat(households.Where(i => i.CustomerOnSAId2.HasValue).Select(i => i.CustomerOnSAId2!.Value))
           .GroupBy(i => i).Where(i => i.Count() > 1).Select(i => i.Key);
        if (duplicitCustomerOnSAIds.Any())
        {
            throw new CisValidationException(99999, $"Sales arrangement households contain duplicit customers [{ String.Join(StringJoinSeparator, duplicitCustomerOnSAIds) }] on sales arrangement."); //TODO: ErrorCode
        }

        // check if customers on SA correspond to customers on households
        var arrangementCustomerIds = customersOnSa.Select(i => i.CustomerOnSAId);
        var householdCustomerIds = households.Where(i => i.CustomerOnSAId1.HasValue).Select(i => i.CustomerOnSAId1!.Value)
            .Concat(households.Where(i => i.CustomerOnSAId2.HasValue).Select(i => i.CustomerOnSAId2!.Value));

        var missingCustomerIdsOnHouseholds = arrangementCustomerIds.Where(id => !householdCustomerIds.Contains(id));
        var missingCustomerIdsOnArrangement = householdCustomerIds.Where(id => !arrangementCustomerIds.Contains(id));

        var customerIdsInvalid = missingCustomerIdsOnHouseholds.Concat(missingCustomerIdsOnArrangement).ToList();

        if (customerIdsInvalid.Any())
        {
            throw new CisValidationException(99999, $"Customers [{ String.Join(StringJoinSeparator, customerIdsInvalid) }] on sales arrangement don't correspond to customers on households."); //TODO: ErrorCode
        }
    }

    private static Identity GetMainMpIdentity(List<Contracts.Household> households, Dictionary<int, HouseholdTypeItem> householdTypesById, List<Contracts.CustomerOnSA> customersOnSa)
    {
        var mainHousehold = households.Single(i => householdTypesById[i.HouseholdTypeId].Value == CIS.Foms.Enums.HouseholdTypes.Main);
        var mainCustomerOnSa1 = customersOnSa.Single(i => i.CustomerOnSAId == mainHousehold.CustomerOnSAId1!.Value);
        return mainCustomerOnSa1.CustomerIdentifiers.Where(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp).First();
    }

    private async Task<ProductTypeItem> GetProductType(int salesArrangementTypeId, CancellationToken cancellation)
    {
        var salesArrangementType = (await _codebookService.SalesArrangementTypes(cancellation)).FirstOrDefault(t => t.Id == salesArrangementTypeId);

        if (salesArrangementType == null)
        {
            throw new CisNotFoundException(99999, nameof(SalesArrangementTypeItem), salesArrangementTypeId); //TODO: ErrorCode
        }

        if (!salesArrangementType.ProductTypeId.HasValue)
        {
            throw new CisArgumentNullException(7, $"Sales arrangement type '{salesArrangementTypeId}' with undefined product type", nameof(salesArrangementTypeId));
        }

        var productType = (await _codebookService.ProductTypes(cancellation)).FirstOrDefault(t => t.Id == salesArrangementType.ProductTypeId);

        if (productType == null)
        {
            throw new CisNotFoundException(13014, nameof(ProductTypeItem), salesArrangementType.ProductTypeId.Value);
        }

        return productType;
    }

    private async Task<List<Contracts.CustomerOnSA>> GetCustomersOnSA(int salesArrangementId, CancellationToken cancellation)
    {
        var customersOnSa = (await _mediator.Send(new Dto.GetCustomerListMediatrRequest(salesArrangementId), cancellation)).Customers.ToList();
        var customerOnSAIds = customersOnSa.Select(i => i.CustomerOnSAId).ToArray();
        var customers = new List<Contracts.CustomerOnSA>();
        for (int i = 0; i < customerOnSAIds.Length; i++)
        {
            var customer = await _mediator.Send(new Dto.GetCustomerMediatrRequest(customerOnSAIds[i]), cancellation);
            customers.Add(customer);
        }
        return customers;
    }

    private async Task<Dictionary<int, Income>> GetIncomesById(List<Contracts.CustomerOnSA> customersOnSa, CancellationToken cancellation)
    {
        var incomeIds = customersOnSa.SelectMany(i => i.Incomes.Select(i=>i.IncomeId)).ToArray();
        var incomes = new List<Income>();
        for (int i = 0; i < incomeIds.Length; i++)
        {
            var income = await _mediator.Send(new Dto.GetIncomeMediatrRequest(incomeIds[i]), cancellation);
            incomes.Add(income);
        }
        return incomes.ToDictionary(i=>i.IncomeId);
    }

    private async Task<Dictionary<string, CustomerResponse>> GetCustomersByIdentityCode(List<Contracts.CustomerOnSA> customersOnSa, CancellationToken cancellation)
    {
        // vrací pouze pro KB identity
        var customerIdentities = customersOnSa.SelectMany(i => i.CustomerIdentifiers.Where(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb)).GroupBy(i => i.ToCode()).Select(i => i.First()).ToList();
        var results = new List<CustomerResponse>();
        for (int i = 0; i < customerIdentities.Count; i++)
        {
            var customer = ServiceCallResult.ResolveToDefault<CustomerResponse>(await _customerService.GetCustomerDetail(new CustomerRequest { Identity = customerIdentities[i] }, cancellation));
            results.Add(customer!);
        }
        return results.ToDictionary(i => i.Identities.First().ToCode());
    }

    private static string ResolveGetContractNumber(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<string> r => r.Model,
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };

    private async Task UpdateSalesArrangement(Contracts.SalesArrangement entity, string contractNumber, CancellationToken cancellation)
    {
        await _mediator.Send(new Dto.UpdateSalesArrangementMediatrRequest(new UpdateSalesArrangementRequest { SalesArrangementId = entity.SalesArrangementId, ContractNumber = contractNumber }), cancellation);
        entity.ContractNumber = contractNumber;
    }

    private async Task UpdateCase(Case entity, string contractNumber, CancellationToken cancellation)
    {
        var data = new CaseData(entity.Data);
        data.ContractNumber = contractNumber;
        ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.UpdateCaseData(entity.CaseId, data, cancellation));
        entity.Data.ContractNumber = contractNumber;
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

    private static string CreateJsonData(
        Contracts.SalesArrangement arrangement,
        ProductTypeItem productType,
        GetMortgageDataResponse offer,
        Case caseData,
        User? user,
        List<Contracts.Household> households,
        List<Contracts.CustomerOnSA> customersOnSa,
        Dictionary<int, Income> incomesById,
        Dictionary<string, CustomerResponse> customersByIdentityCode,
        Dictionary<int, GenericCodebookItem> academicDegreesBeforeById,
        Dictionary<int, CodebookService.Contracts.Endpoints.Genders.GenderItem> gendersById,
        bool ignoreNullValues = true
        )
    {
        var actualDate = DateTime.Now.Date;

        var householdsByCustomerOnSAId = customersOnSa.ToDictionary(i => i.CustomerOnSAId, i => households.Where(h => h.CustomerOnSAId1 == i.CustomerOnSAId || h.CustomerOnSAId2 == i.CustomerOnSAId).ToArray());

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

        object? MapAddress(Address i)
        {
            if (i == null)
            {
                return null;
            }

            return new {
                typ_adresy = i.AddressTypeId.ToJsonString(),
                ulice = i.Street,
                cislo_popisne = i.BuildingIdentificationNumber,
                cislo_orientacni = i.LandRegistryNumber,
                // ulice_dodatek =                                  // ??? OP!
                psc = i.Postcode,
                misto = i.City,
                stat = i.CountryId.ToJsonString(),
            };
        }

        object? MapIdentificationDocument(IdentificationDocument i)
        {
            if (i == null) { 
                return null; 
            }

            return new {
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
                vyse_splatky = i.LoanPaymentAmount.ToJsonString(),
                vyse_nesplacene_jistiny = i.RemainingLoanPrincipal.ToJsonString(),
                vyse_limitu = i.CreditCardLimit.ToJsonString(),
                mimo_entitu_mandanta = ((int)i.ObligationCreditor).ToJsonString(),
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
                var parts = new int?[] { address.BuildingIdentificationNumber, address.LandRegistryNumber };

                var number = String.Join("/", parts.Where(i => i.HasValue));

                return String.IsNullOrEmpty(number) ? null : number;
            }

            var i = incomesById[iil.IncomeId];

            return new
            {
                prvni_pracovni_sml_od = i.Employement?.Job?.FirstWorkContractSince.ToJsonString(),
                posledni_zamestnani_od = actualDate.ToJsonString(),                         // [MOCK] aktuální datum (relevantní v tomto DROPu, poté bude ´posledni_zamestnani_od´ zrušeno)
                poradi_prijmu = rowNumber.ToJsonString(),                                                          
                zdroj_prijmu_hlavni = iil.IncomeTypeId.ToJsonString(),
                typ_pracovniho_pomeru = i.Employement?.Job?.EmploymentTypeId.ToJsonString(),
                klient_ve_vypovedni_lhute = i.Employement?.Job?.JobNoticePeriod.ToJsonString(),
                klient_ve_zkusebni_lhute = i.Employement?.Job?.JobTrialPeriod.ToJsonString(),
                prijem_ze_zahranici = i.Employement?.IsAbroadIncome.ToJsonString(),
                domicilace_prijmu_ze_zamestnani = i.Employement?.IsDomicile.ToJsonString(),
                pracovni_smlouva_aktualni_od = i.Employement?.Job?.CurrentWorkContractSince.ToJsonString(),
                pracovni_smlouva_aktualni_do = i.Employement?.Job?.CurrentWorkContractTo.ToJsonString(),
                zamestnavatel_nazov = i.Employement?.Employer?.Name,
                zamestnavatel_rc_ico = i.Employement?.Employer?.BirthNumber,
                zamestnavatel_sidlo_ulice = i.Employement?.Employer?.Address?.Street,
                zamestnavatel_sidlo_cislo_popisne_orientacni = GetAddressNumber(i.Employement?.Employer?.Address),  //složit string ve formátu "BuildingIdentificationNumber/LandRegistryNumber"
                zamestnavatel_sidlo_mesto = i.Employement?.Employer?.Address?.City,
                zamestnavatel_sidlo_psc = i.Employement?.Employer?.Address?.Postcode,
                zamestnavatel_sidlo_stat = i.Employement?.Employer?.Address?.CountryId.ToJsonString(),
                zamestnavatel_telefonni_cislo = i.Employement?.Employer?.PhoneNumber,
                zamestnavatel_okec = i.Employement?.Employer?.ClassficationOfEconomicActivities,
                zamestnavatel_pracovni_sektor =  i.Employement?.Employer?.WorkSectorId.ToJsonString(),
                zamestnavatel_senzitivni_sektor =  i.Employement?.Employer?.SensitiveSector.ToJsonString(),
                povolani = i.Employement?.Job?.JobType.ToJsonString(),
                zamestnan_jako = i.Employement?.Job?.JobDescription,
                prijem_vyse = iil.Sum.ToJsonString(),
                prijem_mena = iil.CurrencyCode,
                zrazky_ze_mzdy_rozhodnuti = i.Employement?.WageDeduction?.DeductionDecision.ToJsonString(),
                zrazky_ze_mzdy_splatky = i.Employement?.WageDeduction?.DeductionPayments.ToJsonString(),
                zrazky_ze_mzdy_ostatni = i.Employement?.WageDeduction?.DeductionOther.ToJsonString(),
                prijem_potvrzeni_vystavila_ext_firma = i.Employement?.IncomeConfirmation?.ConfirmationByCompany.ToJsonString(),
                prijem_potvrzeni_misto_vystaveni =  i.Employement?.IncomeConfirmation?.ConfirmationPlace,
                prijem_potvrzeni_datum =  i.Employement?.IncomeConfirmation?.ConfirmationDate.ToJsonString(),
                prijem_potvrzení_osoba =  i.Employement?.IncomeConfirmation?.ConfirmationPerson,
                prijem_potvrzeni_kontakt =  i.Employement?.IncomeConfirmation?.ConfirmationContact,
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
            var c = customersByIdentityCode[identityKb.ToCode()];

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

                titul_pred = cDegreeBeforeId.HasValue ? academicDegreesBeforeById[cDegreeBeforeId.Value].Name : null,    // (použít Name, nikoliv jen Id) 
                // titul_za = c.NaturalPerson?.DegreeAfterId,                                    // ??? (použít Name, nikoliv jen Id), ve vzorovém JSONu ani není - neposílat

                prijmeni_nazev = c.NaturalPerson?.LastName,
                prijmeni_rodne = c.NaturalPerson?.BirthName,
                jmeno = c.NaturalPerson?.FirstName,
                datum_narozeni = c.NaturalPerson?.DateOfBirth.ToJsonString(),
                misto_narozeni_obec = c.NaturalPerson?.PlaceOfBirth,                             
                misto_narozeni_stat = c.NaturalPerson?.BirthCountryId.ToJsonString(),
                pohlavi = cGenderId.HasValue ? gendersById[cGenderId.Value].StarBuildJsonCode : null,
                statni_prislusnost = cCitizenshipCountriesId.ToJsonString(),                    // vzít první
                zamestnanec = 0.ToJsonString(),                                                 // [MOCK] OfferInstance (default 0)
                rezident = 0.ToJsonString(),                                                    // [MOCK] OfferInstance (default 0)
                // PEP =                                                                        // OP! Neposílat, není definováno ve starbuild.
                seznam_adres = c.Addresses?.Select(i => MapAddress(i)).ToArray() ?? Array.Empty<object>(),
                seznam_dokladu = cIdentificationDocuments,                                      // ??? mělo by to být pole, nikoliv jeden objekt ???
                seznam_kontaktu = c.Contacts?.Select(i => MapContact(i)).ToArray() ?? Array.Empty<object>(),
                rodinny_stav = c.NaturalPerson?.MaritalStatusStateId.ToJsonString(),
                druh_druzka = i.HasPartner.ToJsonString(),
                // vzdelani =                                                                   // ??? OP!
                prijmy = i.Incomes?.ToList().Select((i, index) => MapCustomerIncome(i, index + 1)).ToArray() ?? Array.Empty<object>(),
                zavazky = i.Obligations?.ToList().Select((i, index) => MapCustomerObligation(i, index + 1)).ToArray() ?? Array.Empty<object>(),
                // prijem_sbiran =                                                              // ??? out of scope
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
                //zmocnenec =                                               // CustomerOnSA ???
                cislo_domacnosti = household.HouseholdId.ToJsonString(),    // CustomerOnSA ??? brát Houshold.CustomerOnSAId (1 nebo 2)
                klient = MapCustomer(i),
            };
        }

        // root
        
        var financialResourcesOwn = offer.Inputs.FinancialResourcesOwn.ToDecimal();
        var financialResourcesOther = offer.Inputs.FinancialResourcesOther.ToDecimal();
        decimal? financialResourcesTotal = (financialResourcesOwn.HasValue || financialResourcesOther.HasValue) ? ((financialResourcesOwn ?? 0) + (financialResourcesOther ?? 0)) : null;

        var data = new
        {
            cislo_smlouvy = arrangement.ContractNumber,
            case_id = arrangement.CaseId.ToJsonString(),
            business_case_ID = arrangement.RiskBusinessCaseId,                                                                      // SalesArrangement
            kanal_ziskani = arrangement.ChannelId.ToJsonString(),                                                                   // SalesArrangement - vyplněno na základě usera
            datum_vytvoreni_zadosti = actualDate.ToJsonString(),                                                                    // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
            datum_prvniho_podpisu = actualDate.ToJsonString(),                                                                      // [MOCK] SalesArrangement - byla domluva posílat pro D1.1 aktuální datum
            //uv_produkt = offer.ProductTypeId.ToJsonString(),                                                                      // ??? SalesArrangement nemá být z OfferProductTypeId ???
            uv_produkt = productType.Id.ToJsonString(),
            uv_druh = offer.Inputs.LoanKindId.ToJsonString(),                                                                       // OfferInstance
            indikativni_LTV = offer.Outputs.LoanToValue.ToJsonString(),                                                             // OfferInstance
            indikativni_LTC = offer.Outputs.LoanToCost.ToJsonString(),                                                              // OfferInstance
            seznam_mark_akci = Array.Empty<object>(),                                                                               // [MOCK] OfferInstance (default empty array)
            individualni_sleva_us = 0.ToJsonString(),                                                                               // [MOCK] OfferInstance (default 0)
            garance_us = 0.ToJsonString(),                                                                                          // [MOCK] OfferInstance (default 0)
            sazba_vyhlasovana = offer.Outputs.InterestRateAnnounced.ToJsonString(),                                                 // OfferInstance
            sazba_skladacka = offer.Outputs.LoanInterestRate.ToJsonString(),                                                        // OfferInstance
            sazba_poskytnuta = offer.Outputs.LoanInterestRate.ToJsonString(),                                                       // OfferInstance = sazba_skladacka
            vyhlasovanaTyp = 1.ToJsonString(),                                                                                      // [MOCK] OfferInstance (default 1)             
            vyse_uveru = offer.Outputs.LoanAmount.ToJsonString(),                                                                   // OfferInstance
            anuitni_splatka = offer.Outputs.LoanPaymentAmount.ToJsonString(),                                                       // OfferInstance
            splatnost_uv_mesice = offer.Outputs.LoanDuration.ToJsonString(),                                                        // OfferInstance (kombinace dvou vstupů roky + měsíce na FE)
            fixace_uv_mesice = offer.Inputs.FixedRatePeriod.ToJsonString(),                                                         // OfferInstance - na FE je to v rocích a je to číselník ?
            predp_termin_cerpani = arrangement.Mortgage?.ExpectedDateOfDrawing.ToJsonString(),                                      // SalesArrangement
            den_splaceni = offer.Outputs.PaymentDayOfTheMonth.ToJsonString(),                                                       // OfferInstance default=15
            forma_splaceni = 1.ToJsonString(),                                                                                      // [MOCK] OfferInstance (default 1)  
            seznam_poplatku = Array.Empty<object>(),                                                                                // [MOCK] OfferInstance - celý objekt vůbec nebude - TBD - diskuse k simulaci 
                                                                                                                                    //          (na offer zatím nemáme, dohodnuta mockovaná hodnota prázdné pole)
            seznam_ucelu = offer.Inputs.LoanPurpose?.Select(i => MapLoanPurpose(i)).ToArray() ?? Array.Empty<object>(),             // OfferInstance - 1..5 ??? má se brát jen prvních 5 účelů ?
            seznam_objektu = arrangement.Mortgage?.LoanRealEstates.ToList().Select((i, index) => MapLoanRealEstate(i, index + 1)).ToArray() ?? Array.Empty<object>(), // SalesArrangement - 0..3 ???
            seznam_ucastniku = customersOnSa?.Select(i => MapCustomerOnSA(i)).ToArray() ?? Array.Empty<object>(),                   // CustomerOnSA, Customer
            zprostredkovano_3_stranou = false.ToJsonString(),                                                                       // [MOCK] SalesArrangement - dle typu Usera (na offer zatím nemáme, dohodnuta mockovaná hodnota FALSE)
            sjednal_CPM = user!.CPM,                                                                                                // User
            sjednal_ICP = user!.ICP,                                                                                                // User
            mena_prijmu = arrangement.Mortgage?.IncomeCurrencyCode,                                                                 // SalesArrangement
            mena_bydliste = arrangement.Mortgage?.ResidencyCurrencyCode,                                                            // SalesArrangement

            zpusob_zasilani_vypisu = offer.Outputs.StatementTypeId.ToJsonString(),                                                  // Offerinstance
            predp_hodnota_nem_zajisteni = offer.Inputs.CollateralAmount.ToJsonString(),                                             // Offerinstance
            fin_kryti_vlastni_zdroje = offer.Inputs.FinancialResourcesOwn.ToJsonString(),                                           // OfferInstance
            fin_kryti_cizi_zdroje = offer.Inputs.FinancialResourcesOther.ToJsonString(),                                            // OfferInstance
            fin_kryti_celkem = financialResourcesTotal.ToJsonString(),                                                              // OfferInstance
            zpusob_podpisu_smluv_dok = arrangement.Mortgage?.SignatureTypeId.ToJsonString(),                                        // SalesArrangement
            seznam_domacnosti = households?.Select(i => MapHousehold(i)).ToArray() ?? Array.Empty<object>(),
        };

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = ignoreNullValues ? System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull : System.Text.Json.Serialization.JsonIgnoreCondition.Never };
        var json = JsonSerializer.Serialize(data, options);

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
 * https://wiki.kb.cz/confluence/display/HT/Offer
 * 
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

    Honza Herych:
    getnout customera on SA pomoci CustomerOnSAId1
    zafiltrovat v jeho identitach na MP identitu
    a ID teto identity poslat do EAS jako partnerId

    -------------------------------------------------------------------------------------------------------------
 */