﻿using CIS.Core.Security;
using CIS.Infrastructure.Security;
using SharedAudit;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using DomainServices.DocumentOnSAService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.CancelCase;

internal sealed class CancelCaseHandler
    : IRequestHandler<CancelCaseRequest, CancelCaseResponse>
{
    public async Task<CancelCaseResponse> Handle(CancelCaseRequest request, CancellationToken cancellation)
    {
        // case entity
        var entity = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        // Pokud stav case není 1 (příprava žádosti) vracíme chybu, nelze stornovat
        if (entity.State != (int)CaseStates.InProgress)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UnableToCancelCase, request.CaseId);
        }

        // produktovy SA
        var salesArrangementId = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellation))
            .First()
            .SalesArrangementId;

        // dokumenty v podpisovani
        var documents = (await _documentOnSAService.GetDocumentsOnSAList(salesArrangementId, cancellation))
            .DocumentsOnSA
            .ToList();

        // dojde k provolání stopDocumentOnSASigning pro všechny rozpodepsané dokumenty (getDocumentsToSignList zafiltrovaný na IsSigned = false a DocumentOnSAId in not null)
        await stopSigning(salesArrangementId, cancellation);

        // druh storna podle datumu prvniho podpisu
        CaseStates newCaseState = await firstSignatureDateIsSet(salesArrangementId, documents, cancellation) ? CaseStates.ToBeCancelled : CaseStates.Cancelled;

        if (newCaseState == CaseStates.ToBeCancelled) // pokud mame datum prvniho podpisu
        {
            // odeslat do SB
            await _salesArrangementService.SendToCmp(salesArrangementId, true, cancellation);
        }
        else // pokud nezname datum prvniho podpisu
        {
            // je debtor identifikovany?
            if (await isDebtorIdentified(salesArrangementId, cancellation))
            {
                await _productService.CancelMortgage(request.CaseId, cancellation);

                var saInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellation);

                // zavolat RIP
                if (!string.IsNullOrEmpty(saInstance.RiskBusinessCaseId))
                {
                    var identity = _currentUser.GetUserIdentityFromHeaders();
                    await _riskBusinessCaseService.CommitCase(new RiskIntegrationService.Contracts.RiskBusinessCase.V2.RiskBusinessCaseCommitCaseRequest
                    {
                        SalesArrangementId = salesArrangementId,
                        ProductTypeId = entity.ProductTypeId,
                        RiskBusinessCaseId = saInstance.RiskBusinessCaseId,
                        FinalResult = request.IsUserInvoked ? RiskIntegrationService.Contracts.RiskBusinessCase.V2.RiskBusinessCaseFinalResults.CANCELLED_BY_CLIENT : RiskIntegrationService.Contracts.RiskBusinessCase.V2.RiskBusinessCaseFinalResults.TIMEOUT_BY_EXT_SYS,
                        UserIdentity = identity is null ? null : new RiskIntegrationService.Contracts.Shared.Identity
                        {
                            IdentityScheme = identity.Scheme.ToString(),
                            IdentityId = identity.Identity
                        }
                    }, cancellation);
                }
            }
        }

        // dojde k odeslání elektronicky podepsaných dokumentů do archivu (getDocumentsOnSAList nad produktovou žádostí zafiltrovaný na IsSigned = true a SignatureTypeId = 3)
        await setDocumentArchived(documents, cancellation);

        // nastavit storno v nasi DB
        await _mediator.Send(new UpdateCaseStateRequest
        {
            CaseId = request.CaseId,
            State = (int)newCaseState
        }, cancellation);

        // nastavit stav na SA
        await _salesArrangementService.DeleteSalesArrangement(salesArrangementId, true, cancellation);

        // auditni log
        _auditLogger.Log(
            AuditEventTypes.Noby004,
            "Případ byl stornován",
            products: new List<AuditLoggerHeaderItem>
            {
                new(AuditConstants.ProductNamesCase, request.CaseId)
            },
            bodyBefore: new Dictionary<string, string>
            {
                { "button_label", "Storno žádosti" }
            }
        );

        return new CancelCaseResponse
        {
            CaseState = (int)newCaseState
        };
    }

    private async Task setDocumentArchived(List<DocumentOnSAToSign> documents, CancellationToken cancellationToken)
    {
        var docs = documents.Where(t => t.IsSigned && t.DocumentOnSAId.HasValue && t.SignatureTypeId == (int)SignatureTypes.Electronic);

        foreach (var document in docs)
        {
            await _documentOnSAService.SetDocumentOnSAArchived(document.DocumentOnSAId!.Value, cancellationToken);
        }
    }

    private async Task stopSigning(int salesArrangementId, CancellationToken cancellationToken)
    {
        var documents = (await _documentOnSAService.GetDocumentsToSignList(salesArrangementId, cancellationToken))
            .DocumentsOnSAToSign
            .Where(t => !t.IsSigned && t.DocumentOnSAId.HasValue);

        foreach (var document in documents)
        {
            await _documentOnSAService.StopSigning(new() 
            {
                DocumentOnSAId = document.DocumentOnSAId!.Value,
                SkipValidations = true
            } , cancellationToken);
        }
    }

    /// <summary>
    /// je debtor identifikovany?
    /// </summary>
    private async Task<bool> isDebtorIdentified(int salesArrangementId, CancellationToken cancellationToken)
    {
        var debtor = (await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken))
                .First(t => t.CustomerRoleId == (int)CustomerRoles.Debtor);

        return debtor.CustomerIdentifiers?.Any() ?? false;
    }

    /// <summary>
    /// Nad produktovým SalesArrangementem projít podepsané dokumenty(getDocumentsOnSAList) a najít podepsaný(DocumentOnSA.IsSigned = true), který se vztahuje k hlavnímu Householdu(DocumentOnSA.HouseholdTypeId = 1)
    /// Pokud takový dokument existuje, máme k němu datum prvního podpisu
    /// </summary>
    private async Task<bool> firstSignatureDateIsSet(int salesArrangementId, List<DocumentOnSAToSign> documents, CancellationToken cancellationToken)
    {
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);
        int? householdId = households
            .FirstOrDefault(t => t.HouseholdTypeId == (int)HouseholdTypes.Main)
            ?.HouseholdId;
        
        return householdId.HasValue ? documents.Any(t => t.IsSigned && t.HouseholdId == householdId) : false;
    }

    private readonly IAuditLogger _auditLogger;
    private readonly IMediator _mediator;
    private readonly CaseServiceDbContext _dbContext;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient _riskBusinessCaseService;
    private readonly HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly HouseholdService.Clients.IHouseholdServiceClient _householdService;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly DocumentOnSAService.Clients.IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ProductService.Clients.IProductServiceClient _productService;

    public CancelCaseHandler(
        IAuditLogger auditLogger,
        IMediator mediator,
        ICurrentUserAccessor currentUser,
        DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient riskBusinessCaseService,
        HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        HouseholdService.Clients.IHouseholdServiceClient householdService,
        DocumentOnSAService.Clients.IDocumentOnSAServiceClient documentOnSAService,
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        ProductService.Clients.IProductServiceClient productService,
        CaseServiceDbContext dbContext)
    {
        _auditLogger = auditLogger;
        _currentUser = currentUser;
        _riskBusinessCaseService = riskBusinessCaseService;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
        _mediator = mediator;
        _dbContext = dbContext;
        _documentOnSAService = documentOnSAService;
        _salesArrangementService = salesArrangementService;
        _productService = productService;
    }
}
