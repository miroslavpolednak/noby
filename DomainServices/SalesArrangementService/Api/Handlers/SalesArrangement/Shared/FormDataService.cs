﻿using Grpc.Core;
using CIS.Infrastructure.gRPC.CisTypes;

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


namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement.Shared;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class FormDataService
{

    #region Construction

    private static readonly string StringJoinSeparator = ",";

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly IUserServiceAbstraction _userService;

    private readonly Repositories.NobyRepository _repository;
    private readonly ILogger<FormDataService> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly IMediator _mediator;

    public FormDataService(
        ICodebookServiceAbstraction codebookService,
        ICaseServiceAbstraction caseService,
        IOfferServiceAbstraction offerService,
        ICustomerServiceAbstraction customerService,
        IUserServiceAbstraction userService,
        Repositories.NobyRepository repository,
        ILogger<FormDataService> logger,
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

    #region Data (loading & modifications)

    private static void CheckSA(Contracts.SalesArrangement arrangement)
    {
        // check mandatory fields of SalesArrangement
        var saMandatoryFields = new List<(string Field, bool Valid)>
        {
            ("IncomeCurrencyCode", !String.IsNullOrEmpty(arrangement.Mortgage?.IncomeCurrencyCode)  ),
            ("ResidencyCurrencyCode", !String.IsNullOrEmpty(arrangement.Mortgage?.ResidencyCurrencyCode) ),
            ("SignatureTypeId", (arrangement.Mortgage?.SignatureTypeId).HasValue ),
        };

        var invalidSaMandatoryFields = saMandatoryFields.Where(i => !i.Valid).Select(i => i.Field).ToArray();

        if (invalidSaMandatoryFields.Length > 0)
        {
            throw new CisValidationException(99999, $"Sales arrangement mandatory fields not provided [{String.Join(StringJoinSeparator, invalidSaMandatoryFields)}]."); //TODO: ErrorCode
        }

        // check if Offer exists
        if (!arrangement.OfferId.HasValue)
        {
            throw new CisNotFoundException(16000, $"Sales Arrangement #{arrangement.SalesArrangementId} is not linked to Offer");
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
                ("JobNoticePeriod", (income.Employement?.Job?.JobNoticePeriod).HasValue ),
                ("JobTrialPeriod", (income.Employement?.Job?.JobTrialPeriod).HasValue )
           };

            return mandatoryFields.Where(i => !i.Valid).Select(i => i.Field).ToArray();
        }

        var invalidIncomes = incomesById.Select(i => new { Id = i.Key, InvalidFields = FindInvalidFields(i.Value) }).Where(i => i.InvalidFields.Length > 0).ToArray();
        if (invalidIncomes.Length > 0)
        {
            var details = invalidIncomes.Select(i => $"{i.Id}[{String.Join(StringJoinSeparator, i.InvalidFields)}]");
            throw new CisValidationException(99999, $"Income mandatory fields not provided [{String.Join(StringJoinSeparator, details)}]."); //TODO: ErrorCode
        }
    }

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
            throw new CisValidationException(99999, $"Sales arrangement customers [{String.Join(StringJoinSeparator, customerIdsInvalid)}] don't contain both [KB,MP] identities."); //TODO: ErrorCode
        }
    }

    private static void CheckHouseholds(List<Contracts.Household> households, Dictionary<int, HouseholdTypeItem> householdTypesById, List<Contracts.CustomerOnSA> customersOnSa)
    {
        // check if each household type is represented at most once
        var duplicitHouseholdTypeIds = households.GroupBy(i => i.HouseholdTypeId).Where(g => g.Count() > 1).Select(i => i.Key);
        if (duplicitHouseholdTypeIds.Any())
        {
            throw new CisValidationException(99999, $"Sales arrangement contains duplicit household types [{String.Join(StringJoinSeparator, duplicitHouseholdTypeIds)}]."); //TODO: ErrorCode
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
            throw new CisValidationException(99999, $"Sales arrangement contains households [{String.Join(StringJoinSeparator, invalidHouseholdIds)}] with CustomerOnSAId2 but without CustomerOnSAId1."); //TODO: ErrorCode
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
            throw new CisValidationException(99999, $"Sales arrangement households contain duplicit customers [{String.Join(StringJoinSeparator, duplicitCustomerOnSAIds)}] on sales arrangement."); //TODO: ErrorCode
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
            throw new CisValidationException(99999, $"Customers [{String.Join(StringJoinSeparator, customerIdsInvalid)}] on sales arrangement don't correspond to customers on households."); //TODO: ErrorCode
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
        var incomeIds = customersOnSa.SelectMany(i => i.Incomes.Select(i => i.IncomeId)).ToArray();
        var incomes = new List<Income>();
        for (int i = 0; i < incomeIds.Length; i++)
        {
            var income = await _mediator.Send(new Dto.GetIncomeMediatrRequest(incomeIds[i]), cancellation);
            incomes.Add(income);
        }
        return incomes.ToDictionary(i => i.IncomeId);
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
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors[0].Message, err.Errors[0].Key),
            _ => throw new NotImplementedException()
        };

    private static void ResolveAddFirstSignatureDate(IServiceCallResult result)
    {
        switch (result)
        {
            case ErrorServiceCallResult err:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors[0].Message, err.Errors[0].Key);
        }
    }

    private static int ResolveCheckForm(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<int> r => r.Model,
           ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors[0].Message, err.Errors[0].Key),
           _ => throw new NotImplementedException()
       };

    private async Task UpdateSalesArrangement(Contracts.SalesArrangement entity, string contractNumber, CancellationToken cancellation)
    {
        await _mediator.Send(new Dto.UpdateSalesArrangementMediatrRequest(new UpdateSalesArrangementRequest { SalesArrangementId = entity.SalesArrangementId, ContractNumber = contractNumber, EaCode = entity.EaCode, RiskBusinessCaseId = entity.RiskBusinessCaseId, FirstSignedDate = entity.FirstSignedDate }), cancellation);
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

    public async Task<FormData> LoadAndPrepare(int salesArrangementId, CancellationToken cancellation)
    {
        // load SalesArrangement
        var arrangement = await _mediator.Send(new Dto.GetSalesArrangementMediatrRequest(new GetSalesArrangementRequest { SalesArrangementId = salesArrangementId }), cancellation);

        // check mandatory fields of SalesArrangement
        CheckSA(arrangement);

        // TODO: Některé validace se týkají pouze DROPu 1 !!!

        // load customers on SA and validate them
        var customersOnSA = await GetCustomersOnSA(arrangement.SalesArrangementId, cancellation);
        CheckCustomersOnSA(customersOnSA);   // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!

        // load households and validate them
        var households = (await _mediator.Send(new Dto.GetHouseholdListMediatrRequest(arrangement.SalesArrangementId), cancellation)).Households.ToList();
        var householdTypesById = (await _codebookService.HouseholdTypes(cancellation)).ToDictionary(i => i.Id);
        CheckHouseholds(households, householdTypesById, customersOnSA);

        // load incomes
        var incomesById = await GetIncomesById(customersOnSA, cancellation);
        CheckIncomes(incomesById);

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

        // Add first signature date (pro KB produkty caseId = UverID)
        ResolveAddFirstSignatureDate(await _easClient.AddFirstSignatureDate((int)arrangement.CaseId, (int)arrangement.CaseId, DateTime.Now));

        // ProductType load
        var productType = await GetProductType(arrangement.SalesArrangementTypeId, cancellation);

        // Offer load
        var _offer = ServiceCallResult.ResolveToDefault<GetMortgageOfferDetailResponse>(await _offerService.GetMortgageOfferDetail(arrangement.OfferId!.Value, cancellation))
            ?? throw new CisNotFoundException(99999, $"Offer ID #{arrangement.OfferId} does not exist."); //TODO: ErrorCode

        // User load (by arrangement.Created.UserId)
        var _user = ServiceCallResult.ResolveToDefault<User>(await _userService.GetUser(arrangement.Created.UserId ?? 0, cancellation))
            ?? throw new CisNotFoundException(99999, $"User ID #{arrangement.Created.UserId} does not exist."); //TODO: ErrorCode

        // load customers
        var customersByIdentityCode = await GetCustomersByIdentityCode(customersOnSA, cancellation);

        // Load codebooks
        var academicDegreesBeforeById = (await _codebookService.AcademicDegreesBefore(cancellation)).ToDictionary(i => i.Id);
        var gendersById = (await _codebookService.Genders(cancellation)).ToDictionary(i => i.Id);
        var salesArrangementStatesById = (await _codebookService.SalesArrangementStates(cancellation)).ToDictionary(i => i.Id);
        var employmentTypes = await _codebookService.EmploymentTypes(cancellation);

        var formData = new FormData(arrangement, productType, _offer, _case, _user, households, customersOnSA, incomesById, customersByIdentityCode, academicDegreesBeforeById, gendersById, salesArrangementStatesById, employmentTypes);

        return (formData);
    }

}
