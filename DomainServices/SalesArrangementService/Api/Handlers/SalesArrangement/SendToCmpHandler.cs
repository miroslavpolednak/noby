using CIS.Core.Results;
using Grpc.Core;
using CIS.Infrastructure.gRPC;

using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CaseService.Abstraction;

using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;
using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class SendToCmpHandler
    : IRequestHandler<Dto.SendToCmpMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly IMediator _mediator;
    
    public SendToCmpHandler(
        ICodebookServiceAbstraction codebookService,
         ICaseServiceAbstraction caseService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<SendToCmpHandler> logger,
        Eas.IEasClient easClient,
        IMediator mediator)
    {
        _codebookService = codebookService;
        _caseService = caseService;
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
        var productTypeCategory = await GetProductTypeCategory(arrangement.SalesArrangementTypeId);
        if (productTypeCategory != ProductTypeCategory.Mortgage)
        {
            throw new CisArgumentException(1, $"SalesArrangementTypeId '{arrangement.SalesArrangementTypeId}' doesn't match ProductTypeCategory '{ProductTypeCategory.Mortgage}'.", nameof(request));
        }

        // update ContractNumber if not specified
        if (String.IsNullOrEmpty(arrangement.ContractNumber))
        {
            var householdsByType = await GetHouseholdsByType(request.SalesArrangementId, cancellation);

            // pro EAS.Get_ContractNumber se jako ´clientId´ vezme ´CustomerOnSAId1´ z domácnosti ´Debtor´?
            // ... co když taková na SA není, nebo jich je naopak více?
            // ... co když je nalezen právě jedna domácnost, ale nemá vyplněné ´CustomerOnSAId1´?
            if (!householdsByType.ContainsKey(CIS.Foms.Enums.HouseholdTypes.Debtor))
            {
                throw new CisValidationException(99999, $"Sales arrangement {request.SalesArrangementId} contains no household of type {CIS.Foms.Enums.HouseholdTypes.Debtor}."); //TODO: ErrorCode
            }

            var debtorHousehold = householdsByType[CIS.Foms.Enums.HouseholdTypes.Debtor].First();

            if (!debtorHousehold.CustomerOnSAId1.HasValue)
            {
                throw new CisValidationException(99999, $"Household´s CustomerOnSAId1 not defined'{debtorHousehold.HouseholdId}'."); //TODO: ErrorCode
            }

            // ziskat caseId
            //TODO: [_easClient.GetContractNumber] Kanálu požadavku skončila platnost při pokusu o odeslání po 00:00:05. Zvyšte hodnotu časového limitu předanou volání požadavku, nebo zvyšte hodnotu SendTimeout na vazbě. Čas přidělený této operaci byl pravděpodobně částí delšího časového limitu.
            //var contractNumber = resolveGetContractNumber(await _easClient.GetContractNumber(debtorHousehold.CustomerOnSAId1.Value, (int)arrangement.CaseId));
            var contractNumber = $"{debtorHousehold.CustomerOnSAId1.Value}_{arrangement.CaseId}";

            await UpdateSalesArrangement(request.SalesArrangementId, contractNumber, cancellation);
            await UpdateCase(arrangement.CaseId, contractNumber, cancellation);
        }

        //await UpdateCase(arrangement.CaseId, $"{99}_{arrangement.CaseId}", cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }


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
        await _mediator.Send(new Dto.UpdateSalesArrangementMediatrRequest( new UpdateSalesArrangementRequest { SalesArrangementId = salesArrangementId, ContractNumber = contractNumber }), cancellation);
    }

    private async Task UpdateCase(long caseId, string contractNumber, CancellationToken cancellation)
    {
        var entity = ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.GetCaseDetail(caseId, cancellation))
         ?? throw new CisNotFoundException(16002, $"Case ID #{caseId} does not exist.");

        var data = new CaseService.Contracts.CaseData(entity.Data);
        data.ContractNumber = contractNumber;

        ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.UpdateCaseData(caseId, data, cancellation));
    }

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

 */