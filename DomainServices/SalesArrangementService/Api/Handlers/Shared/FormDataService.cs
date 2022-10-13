using Grpc.Core;
using CIS.Infrastructure.gRPC.CisTypes;

using DomainServices.CodebookService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using DomainServices.ProductService.Abstraction;
using DomainServices.UserService.Clients;
using DomainServices.HouseholdService.Clients;

using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.CustomerService.Contracts;
using DomainServices.ProductService.Contracts;
using DomainServices.UserService.Contracts;
using DomainServices.HouseholdService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;
using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;


namespace DomainServices.SalesArrangementService.Api.Handlers.Shared;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class FormDataService
{

    #region Construction

    private static readonly string StringJoinSeparator = ",";

    private readonly SulmService.ISulmClient _sulmClient;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly IProductServiceAbstraction _productService;
    private readonly IUserServiceClient _userService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly Repositories.NobyRepository _repository;
    private readonly ILogger<FormDataService> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly IMediator _mediator;

    public FormDataService(
        ICustomerOnSAServiceClient customerOnSAService,
        IHouseholdServiceClient householdService,
        SulmService.ISulmClient sulmClient,
        ICodebookServiceAbstraction codebookService,
        ICaseServiceAbstraction caseService,
        IOfferServiceAbstraction offerService,
        ICustomerServiceAbstraction customerService,
        IProductServiceAbstraction productService,
        IUserServiceClient userService,
        Repositories.NobyRepository repository,
        ILogger<FormDataService> logger,
        Eas.IEasClient easClient,
        IMediator mediator)
    {
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
        _sulmClient = sulmClient;
        _codebookService = codebookService;
        _caseService = caseService;
        _offerService = offerService;
        _customerService = customerService;
        _productService = productService;
        _userService = userService;
        _repository = repository;
        _logger = logger;
        _easClient = easClient;
        _mediator = mediator;
    }

    #endregion

    #region Data (loading & modifications)

    private static void CheckSA(Contracts.SalesArrangement arrangement)
    {
        // check mandatory fields of SalesArrangement
        var saMandatoryFields = new List<(string Field, bool Valid)>
        {
            ("IncomeCurrencyCode", !String.IsNullOrEmpty(arrangement.Mortgage?.IncomeCurrencyCode)  ),
            ("ResidencyCurrencyCode", !String.IsNullOrEmpty(arrangement.Mortgage?.ResidencyCurrencyCode) ),
            ("SignatureTypeId", (arrangement.Mortgage?.ContractSignatureTypeId).HasValue ),
        };

        var invalidSaMandatoryFields = saMandatoryFields.Where(i => !i.Valid).Select(i => i.Field).ToArray();

        if (invalidSaMandatoryFields.Length > 0)
        {
            throw new CisValidationException(16064, $"Sales arrangement mandatory fields not provided [{String.Join(StringJoinSeparator, invalidSaMandatoryFields)}].");
        }

        // check if Offer exists
        if (!arrangement.OfferId.HasValue)
        {
            throw new CisNotFoundException(16065, $"Sales Arrangement #{arrangement.SalesArrangementId} is not linked to Offer");
        }
    }

    private static void CheckIncomes(Dictionary<int, Income> incomesById)
    {
        // check mandatory fields of Incomes
        string[] FindInvalidFields(Income income)
        {
            var mandatoryFields = new List<(string Field, bool Valid)>
            {
                ("EmploymentTypeId", (income.Employement?.Job?.EmploymentTypeId).HasValue  ),
                ("IsInProbationaryPeriod", (income.Employement?.Job?.IsInProbationaryPeriod).HasValue ),
                ("IsInTrialPeriod", (income.Employement?.Job?.IsInTrialPeriod).HasValue )
           };

            return mandatoryFields.Where(i => !i.Valid).Select(i => i.Field).ToArray();
        }

        var invalidIncomes = incomesById.Select(i => new { Id = i.Key, InvalidFields = FindInvalidFields(i.Value) }).Where(i => i.InvalidFields.Length > 0).ToArray();
        if (invalidIncomes.Length > 0)
        {
            var details = invalidIncomes.Select(i => $"{i.Id}[{String.Join(StringJoinSeparator, i.InvalidFields)}]");
            throw new CisValidationException(16066, $"Income mandatory fields not provided [{String.Join(StringJoinSeparator, details)}].");
        }
    }

    private static void CheckCustomersOnSA(List<CustomerOnSA> customersOnSa)
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
            throw new CisValidationException(16067, $"Sales arrangement customers [{String.Join(StringJoinSeparator, customerIdsInvalid)}] don't contain both [KB,MP] identities.");
        }
    }

    private static void CheckHouseholds(List<Household> households, Dictionary<int, HouseholdTypeItem> householdTypesById, List<CustomerOnSA> customersOnSa)
    {
        // check if each household type is represented at most once
        var duplicitHouseholdTypeIds = households.GroupBy(i => i.HouseholdTypeId).Where(g => g.Count() > 1).Select(i => i.Key);
        if (duplicitHouseholdTypeIds.Any())
        {
            throw new CisValidationException(16068, $"Sales arrangement contains duplicit household types [{String.Join(StringJoinSeparator, duplicitHouseholdTypeIds)}].");
        }

        // check if MAIN household is available
        var mainHouseholdCount = households.Count(i => householdTypesById[i.HouseholdTypeId].EnumValue == CIS.Foms.Enums.HouseholdTypes.Main);
        if (mainHouseholdCount != 1)
        {
            throw new CisValidationException(16069, $"Sales arrangement must contain just one '{CIS.Foms.Enums.HouseholdTypes.Main}' household.");
        }

        // check if any household contains CustomerOnSAId2 without CustomerOnSAId1
        var invalidHouseholdIds = households.Where(i => !i.CustomerOnSAId1.HasValue && i.CustomerOnSAId2.HasValue).Select(i => i.HouseholdId);
        if (invalidHouseholdIds.Any())
        {
            throw new CisValidationException(16070, $"Sales arrangement contains households [{String.Join(StringJoinSeparator, invalidHouseholdIds)}] with CustomerOnSAId2 but without CustomerOnSAId1.");
        }

        // check if CustomerOnSAId1 is available on Main households
        var mainHousehold = households.Single(i => householdTypesById[i.HouseholdTypeId].EnumValue == CIS.Foms.Enums.HouseholdTypes.Main);
        if (!mainHousehold.CustomerOnSAId1.HasValue)
        {
            throw new CisValidationException(16071, $"Main household´s CustomerOnSAId1 not defined [{mainHousehold.HouseholdId}].");
        }

        // check if the same CustomerOnSA belongs to only one household
        var duplicitCustomerOnSAIds = households.Where(i => i.CustomerOnSAId1.HasValue).Select(i => i.CustomerOnSAId1!.Value)
           .Concat(households.Where(i => i.CustomerOnSAId2.HasValue).Select(i => i.CustomerOnSAId2!.Value))
           .GroupBy(i => i).Where(i => i.Count() > 1).Select(i => i.Key);
        if (duplicitCustomerOnSAIds.Any())
        {
            throw new CisValidationException(16072, $"Sales arrangement households contain duplicit customers [{String.Join(StringJoinSeparator, duplicitCustomerOnSAIds)}] on sales arrangement.");
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
            throw new CisValidationException(16073, $"Customers [{String.Join(StringJoinSeparator, customerIdsInvalid)}] on sales arrangement don't correspond to customers on households.");
        }
    }

    private static Identity GetMainMpIdentity(List<Household> households, Dictionary<int, HouseholdTypeItem> householdTypesById, List<CustomerOnSA> customersOnSa)
    {
        var mainHousehold = households.Single(i => householdTypesById[i.HouseholdTypeId].EnumValue == CIS.Foms.Enums.HouseholdTypes.Main);
        var mainCustomerOnSa1 = customersOnSa.Single(i => i.CustomerOnSAId == mainHousehold.CustomerOnSAId1!.Value);
        return mainCustomerOnSa1.CustomerIdentifiers.Where(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp).First();
    }

    private async Task<ProductTypeItem> GetProductType(int salesArrangementTypeId, CancellationToken cancellation)
    {
        var salesArrangementType = (await _codebookService.SalesArrangementTypes(cancellation)).FirstOrDefault(t => t.Id == salesArrangementTypeId);

        if (salesArrangementType == null)
        {
            throw new CisNotFoundException(18005, nameof(SalesArrangementTypeItem), salesArrangementTypeId);
        }

        if (!salesArrangementType.ProductTypeId.HasValue)
        {
            throw new CisValidationException(16074, $"Sales arrangement type '{salesArrangementTypeId}' with undefined product type");
        }

        var productType = (await _codebookService.ProductTypes(cancellation)).FirstOrDefault(t => t.Id == salesArrangementType.ProductTypeId);

        if (productType == null)
        {
            throw new CisNotFoundException(16075, nameof(ProductTypeItem), salesArrangementType.ProductTypeId.Value);
        }

        return productType;
    }

    private async Task<List<CustomerOnSA>> GetCustomersOnSA(int salesArrangementId, CancellationToken cancellation)
    {
        var customersOnSa = ServiceCallResult.ResolveAndThrowIfError<List<CustomerOnSA>>(await _customerOnSAService.GetCustomerList(salesArrangementId, cancellation));

        var customerOnSAIds = customersOnSa.Select(i => i.CustomerOnSAId).ToArray();
        var customers = new List<CustomerOnSA>();
        for (int i = 0; i < customerOnSAIds.Length; i++)
        {
            var customer = ServiceCallResult.ResolveAndThrowIfError<CustomerOnSA>(await _customerOnSAService.GetCustomer(customerOnSAIds[i], cancellation));
            customers.Add(customer);
        }
        return customers;
    }

    private async Task<Dictionary<int, Income>> GetIncomesById(List<CustomerOnSA> customersOnSa, CancellationToken cancellation)
    {
        var incomeIds = customersOnSa.SelectMany(i => i.Incomes.Select(i => i.IncomeId)).ToArray();
        var incomes = new List<Income>();
        for (int i = 0; i < incomeIds.Length; i++)
        {
            var income = ServiceCallResult.ResolveAndThrowIfError<Income>(await _customerOnSAService.GetIncome(incomeIds[i], cancellation));
            incomes.Add(income);
        }
        return incomes.ToDictionary(i => i.IncomeId);
    }

    private async Task<Dictionary<string, CustomerDetailResponse>> GetCustomersByIdentityCode(List<CustomerOnSA> customersOnSa, CancellationToken cancellation)
    {
        // vrací pouze pro KB identity
        var customerIdentities = customersOnSa.SelectMany(i => i.CustomerIdentifiers.Where(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb)).GroupBy(i => i.ToCode()).Select(i => i.First()).ToList();
        var results = new List<CustomerDetailResponse>();
        for (int i = 0; i < customerIdentities.Count; i++)
        {
            var customer = ServiceCallResult.ResolveToDefault<CustomerDetailResponse>(await _customerService.GetCustomerDetail(customerIdentities[i], cancellation));
            results.Add(customer!);
        }
        return results.ToDictionary(i => i.Identity.ToCode());
    }

    private static string ResolveGetContractNumber(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<string> r => r.Model,
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors[0].Message, err.Errors[0].Key),
            _ => throw new NotImplementedException()
        };

    private static Eas.CommonResponse ResolveAddFirstSignatureDate(IServiceCallResult result) =>
      result switch
      {
          SuccessfulServiceCallResult<Eas.CommonResponse> r when r.Model.CommonValue != 0
          => throw new CisValidationException(16076, $"Invalid result of AddFirstSignatureDate [{r.Model.CommonValue}: {r.Model.CommonText}]."),
          SuccessfulServiceCallResult<Eas.CommonResponse> r => r.Model,
          ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors[0].Message, err.Errors[0].Key),
          _ => throw new NotImplementedException()
      };

    private async Task UpdateSalesArrangement(Contracts.SalesArrangement entity, string contractNumber, CancellationToken cancellation)
    {        
        await _mediator.Send(new Dto.UpdateSalesArrangementMediatrRequest(new UpdateSalesArrangementRequest { SalesArrangementId = entity.SalesArrangementId, ContractNumber = contractNumber, RiskBusinessCaseId = entity.RiskBusinessCaseId, FirstSignedDate = entity.FirstSignedDate, SalesArrangementSignatureTypeId = entity.SalesArrangementSignatureTypeId }), cancellation);
        entity.ContractNumber = contractNumber;
    }

    private async Task UpdateCase(Case entity, string contractNumber, CancellationToken cancellation)
    {
        var data = new CaseData(entity.Data);
        data.ContractNumber = contractNumber;
        ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.UpdateCaseData(entity.CaseId, data, cancellation));
        entity.Data.ContractNumber = contractNumber;
    }

    #endregion

    public async Task<FormData> LoadAndPrepare(int salesArrangementId, CancellationToken cancellation, bool addFirstSignatureDate = false)
    {
        // load SalesArrangement
        var arrangement = await _mediator.Send(new Dto.GetSalesArrangementMediatrRequest(new SalesArrangementIdRequest { SalesArrangementId = salesArrangementId }), cancellation);

        // check mandatory fields of SalesArrangement
        CheckSA(arrangement);

        // TODO: Některé validace se týkají pouze DROPu 1 !!!

        // load customers on SA and validate them
        var customersOnSA = await GetCustomersOnSA(arrangement.SalesArrangementId, cancellation);
        CheckCustomersOnSA(customersOnSA);   // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!

        // load households and validate them
        var households = ServiceCallResult.ResolveAndThrowIfError<List<Household>>(await _householdService.GetHouseholdList(salesArrangementId, cancellation));
        var householdTypesById = (await _codebookService.HouseholdTypes(cancellation)).ToDictionary(i => i.Id);
        CheckHouseholds(households, householdTypesById, customersOnSA);

        // load incomes
        var incomesById = await GetIncomesById(customersOnSA, cancellation);
        CheckIncomes(incomesById);

        // load case
        var _case = ServiceCallResult.ResolveToDefault<Case>(await _caseService.GetCaseDetail(arrangement.CaseId, cancellation))
            ?? throw new CisNotFoundException(18002, $"Case ID #{arrangement.CaseId} does not exist.");

        // update ContractNumber if not specified
        // arrangement.ContractNumber = String.Empty;
        if (String.IsNullOrEmpty(arrangement.ContractNumber))
        {
            var identityMP = GetMainMpIdentity(households, householdTypesById, customersOnSA);
            var contractNumber = ResolveGetContractNumber(await _easClient.GetContractNumber(identityMP.IdentityId, (int)arrangement.CaseId));
            await UpdateSalesArrangement(arrangement, contractNumber, cancellation);
            await UpdateCase(_case, contractNumber, cancellation);
        }

        if (addFirstSignatureDate)
        {
            // Add first signature date (pro KB produkty caseId = UverID)
            ResolveAddFirstSignatureDate(await _easClient.AddFirstSignatureDate((int)arrangement.CaseId, (int)arrangement.CaseId, DateTime.Now.Date));
        }

        // HFICH-2426
        foreach (var customer in customersOnSA)
        {
            var kbIdentity = customer.CustomerIdentifiers.FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
            if (kbIdentity is not null)
            {
                await _sulmClient.StopUse(kbIdentity.IdentityId, "MPAP", cancellation);
                await _sulmClient.StartUse(kbIdentity.IdentityId, "MPAP", cancellation);
            }
        }

        // ProductType load
        var productType = await GetProductType(arrangement.SalesArrangementTypeId, cancellation);

        // Offer load
        var _offer = ServiceCallResult.ResolveToDefault<GetMortgageOfferDetailResponse>(await _offerService.GetMortgageOfferDetail(arrangement.OfferId!.Value, cancellation))
                     ?? throw new CisNotFoundException(18001, $"Offer ID #{arrangement.OfferId} does not exist.");

        // User load (by arrangement.Created.UserId)
        var _user = ServiceCallResult.ResolveToDefault<User>(await _userService.GetUser(arrangement.Created.UserId ?? 0, cancellation))
            ?? throw new CisNotFoundException(16077, $"User ID #{arrangement.Created.UserId} does not exist.");

        // Product mortgage
        var _productMortgage = ServiceCallResult.ResolveAndThrowIfError<GetMortgageResponse>(await _productService.GetMortgage(arrangement.CaseId, cancellation)) ?? throw new CisNotFoundException(18002, $"Product ID #{arrangement.CaseId} does not exist.");
        
        // load customers
        var customersByIdentityCode = await GetCustomersByIdentityCode(customersOnSA, cancellation);

        // load drawing applicant customer
        var applicant = arrangement.Drawing?.Applicant;
        var drawingApplicantCustomer = (applicant is null) ? null : ServiceCallResult.ResolveToDefault<CustomerDetailResponse>(await _customerService.GetCustomerDetail(new Identity(applicant.IdentityId, (CIS.Foms.Enums.IdentitySchemes)applicant.IdentityScheme), cancellation));

        // Load codebooks
        var academicDegreesBefore = await _codebookService.AcademicDegreesBefore(cancellation);
        var genders = await _codebookService.Genders(cancellation);
        var salesArrangementStates = await _codebookService.SalesArrangementStates(cancellation);
        var employmentTypes = await _codebookService.EmploymentTypes(cancellation);
        var drawingDurations = await _codebookService.DrawingDurations(cancellation);
        var drawingType = await _codebookService.DrawingTypes(cancellation);

        var countries = await _codebookService.Countries(cancellation);
        var obligationTypes = await _codebookService.ObligationTypes(cancellation);

        var formData = new FormData(
            arrangement,
            productType,
            _offer,
            _case,
            _productMortgage,
            _user,
            households,
            customersOnSA,
            incomesById,
            customersByIdentityCode,
            drawingApplicantCustomer,
            academicDegreesBefore,
            genders,
            salesArrangementStates,
            employmentTypes,
            drawingDurations,
            drawingType,
            countries,
            obligationTypes
            );

        return (formData);
    }

}
