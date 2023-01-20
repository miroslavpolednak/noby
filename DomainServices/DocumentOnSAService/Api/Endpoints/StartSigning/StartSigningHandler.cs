using DomainServices.CaseService.Clients;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using ExternalServices.Eas.R21;
using Microsoft.EntityFrameworkCore;
using __Entity = DomainServices.DocumentOnSAService.Api.Database.Entities;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class StartSigningHandler : IRequestHandler<StartSigningRequest, StartSigningResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly IEasClient _easClient;

    public StartSigningHandler(
        DocumentOnSAServiceDbContext dbContext,
        IMediator mediator,
        IHouseholdServiceClient householdClient,
        ISalesArrangementServiceClient arrangementServiceClient,
        ICustomerOnSAServiceClient customerOnSAServiceClient,
        ICaseServiceClient caseServiceClient,
        IEasClient easClient)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _householdClient = householdClient;
        _arrangementServiceClient = arrangementServiceClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _caseServiceClient = caseServiceClient;
        _easClient = easClient;
    }

    public async Task<StartSigningResponse> Handle(StartSigningRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var houseHold = await GetHouseholdId(request.DocumentTypeId!.Value, request.SalesArrangementId!.Value, cancellationToken);

        // Check if SalesArrangement has ContractNumber, if not we have to update SalesArrangement and Case 
        await UpdateContractNumberIfNeeded(request.SalesArrangementId!.Value, houseHold, cancellationToken);

        // Check, if signing process has been already started. If started we have to invalidate exist signing processes
        await InvalidateExistingSigningProcessesIfExist(request, houseHold);

        var formIdResponse = await _mediator.Send(new GenerateFormIdRequest { HouseholdId = houseHold.HouseholdId });

        var documentOnSaEntity = MapToEntity(request, houseHold.HouseholdId, formIdResponse.FormId);

        await _dbContext.DocumentOnSa.AddAsync(documentOnSaEntity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(documentOnSaEntity);
    }

    private async Task InvalidateExistingSigningProcessesIfExist(StartSigningRequest request, Household houseHold)
    {
        var existSigningProcesses = await _dbContext.DocumentOnSa
                                                        .Where(e => e.SalesArrangementId == request.SalesArrangementId
                                                         && e.HouseholdId == houseHold.HouseholdId && e.IsValid)
                                                        .ToListAsync();

        if (existSigningProcesses.Any())
        {
            // Invalidate exist signing processes on DocumentOnSa
            existSigningProcesses.ForEach(s => s.IsValid = false);
        }
    }

    private async Task UpdateContractNumberIfNeeded(int salesArrangementId, Household household, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(salesArrangementId, cancellationToken);

        if (salesArrangement is null)
        {
            throw new CisNotFoundException(19000, $"SalesArrangement{salesArrangementId} does not exist.");
        }

        if (string.IsNullOrWhiteSpace(salesArrangement.ContractNumber))
        {
            if (household.CustomerOnSAId1 is null)
            {
                throw new CisNotFoundException(19007, $"CustomerOnSAId1 not found on household {household.HouseholdId}");
            }

            var customer = await _customerOnSAServiceClient.GetCustomer(household.CustomerOnSAId1!.Value, cancellationToken);

            // According to household.CustomerOnSAId1 we have to get customer.CustomerIdentifiers for MPSS 
            var identifier = customer.CustomerIdentifiers.SingleOrDefault(r => r.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp);

            if (identifier is null)
            {
                throw new CisNotFoundException(19006, $"Identity for specified CustomerOnSAId1 {household.CustomerOnSAId1} not found");
            }

            // identifier.IdentityId is specific id for MPSS => clientId
            var contractNumber = await _easClient.GetContractNumber(identifier.IdentityId, (int)salesArrangement.CaseId);

            if (string.IsNullOrWhiteSpace(contractNumber))
            {
                throw new CisNotFoundException(19008, $"ContractNumber for specified IdentityId {identifier.IdentityId} and CaseId {salesArrangement.CaseId} not found");
            }

            await _arrangementServiceClient.UpdateSalesArrangement(salesArrangementId, contractNumber, salesArrangement.RiskBusinessCaseId, salesArrangement.FirstSignedDate, cancellationToken);

            var caseDetail = await _caseServiceClient.GetCaseDetail(salesArrangement.CaseId, cancellationToken);

            await _caseServiceClient.UpdateCaseData(salesArrangement.CaseId, new CaseService.Contracts.CaseData
            {
                ContractNumber = contractNumber,
                ProductTypeId = caseDetail.Data.ProductTypeId,
                TargetAmount = caseDetail.Data.TargetAmount
            });
        }
    }

    private StartSigningResponse MapToResponse(DocumentOnSa documentOnSaEntity)
    {
        return new StartSigningResponse
        {
            DocumentOnSa = new DocumentOnSA
            {
                DocumentOnSAId = documentOnSaEntity.DocumentOnSAId,
                DocumentTypeId = documentOnSaEntity.DocumentTypeId,
                FormId = documentOnSaEntity.FormId,
                HouseholdId = documentOnSaEntity.HouseholdId,
                IsValid = documentOnSaEntity.IsValid,
                IsSigned = documentOnSaEntity.IsSigned,
                IsDocumentArchived = documentOnSaEntity.IsDocumentArchived,
                SignatureMethodId = documentOnSaEntity.SignatureMethodId,
            }
        };
    }

    private __Entity.DocumentOnSa MapToEntity(StartSigningRequest request, int houseHoldId, string formId)
    {
        var entity = new __Entity.DocumentOnSa();
        entity.DocumentTypeId = request.DocumentTypeId!.Value;
        entity.DocumentTemplateVersionId = 0; //ToDo from getDocumentData
        entity.FormId = formId;
        entity.SalesArrangementId = request.SalesArrangementId!.Value;
        entity.HouseholdId = houseHoldId;
        entity.SignatureMethodId = request.SignatureMethodId!.Value;
        entity.Data = "json here"; //ToDo from  getDocumentData
        entity.IsValid = true;
        entity.IsSigned = false;
        entity.IsDocumentArchived = false;
        return entity;
    }

    private async Task<Household> GetHouseholdId(int documentTypeId, int salesArrangementId, CancellationToken cancellationToken)
    {
        var householdTypeId = GetHouseholdTypeId(documentTypeId);

        var houseHolds = await _householdClient.GetHouseholdList(salesArrangementId, cancellationToken);
        var houseHold = houseHolds.SingleOrDefault(r => r.HouseholdTypeId == householdTypeId);

        if (houseHold is null)
        {
            throw new CisNotFoundException(19004, "Household {HouseholdId} does not exist");
        }

        return houseHold;
    }

    private int GetHouseholdTypeId(int documentTypeId) => documentTypeId switch
    {
        4 => 1,
        5 => 2,
        _ => throw new ArgumentException($"DocumentOnTypeId {documentTypeId} does not exist.", nameof(documentTypeId))
    };
}
