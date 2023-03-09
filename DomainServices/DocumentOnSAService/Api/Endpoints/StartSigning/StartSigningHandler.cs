using CIS.Core.Security;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using __Entity = DomainServices.DocumentOnSAService.Api.Database.Entities;
using __Household = DomainServices.HouseholdService.Contracts;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class StartSigningHandler : IRequestHandler<StartSigningRequest, StartSigningResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly IDataAggregatorServiceClient _dataAggregatorServiceClient;
    private readonly ICodebookServiceClients _codebookServiceClients;
    private readonly ICurrentUserAccessor _currentUser;

    public StartSigningHandler(
        DocumentOnSAServiceDbContext dbContext,
        IMediator mediator,
        IHouseholdServiceClient householdClient,
        ISalesArrangementServiceClient arrangementServiceClient,
        IDataAggregatorServiceClient dataAggregatorServiceClient,
        ICodebookServiceClients codebookServiceClients,
        ICurrentUserAccessor currentUser)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _householdClient = householdClient;
        _arrangementServiceClient = arrangementServiceClient;
        _dataAggregatorServiceClient = dataAggregatorServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _currentUser = currentUser;
    }

    public async Task<StartSigningResponse> Handle(StartSigningRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));
        
        await ValidateRequest(request, cancellationToken);

        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId!.Value, cancellationToken);

        if (salesArrangement is null)
        {
            throw new CisNotFoundException(19000, $"SalesArrangement{request.SalesArrangementId} does not exist.");
        }

        var houseHold = await GetHouseholdId(request.DocumentTypeId!.Value, request.SalesArrangementId!.Value, cancellationToken);

        // Check, if signing process has been already started. If started we have to invalidate exist signing processes
        await InvalidateExistingSigningProcessesIfExist(request, houseHold);

        var formIdResponse = await _mediator.Send(new GenerateFormIdRequest { HouseholdId = houseHold.HouseholdId });

        var documentData = await _dataAggregatorServiceClient.GetDocumentData(new()
        {
            DocumentTypeId = request.DocumentTypeId!.Value,
            InputParameters = new()
            {
                SalesArrangementId = request.SalesArrangementId!.Value,
                CaseId = salesArrangement.CaseId,
                UserId = _currentUser.User?.Id
            }
        }, cancellationToken);

        var documentOnSaEntity = MapToEntity(request, houseHold, formIdResponse.FormId, documentData);

        await _dbContext.DocumentOnSa.AddAsync(documentOnSaEntity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(documentOnSaEntity);
    }

    private async Task ValidateRequest(StartSigningRequest request, CancellationToken cancellationToken)
    {
        var signingMethods = await _codebookServiceClients.SigningMethodsForNaturalPerson(cancellationToken);
        if (!signingMethods.Any(r => r.Code.ToUpper() == request.SignatureMethodCode.ToUpper()))
        {
            throw new CisNotFoundException(19002, $"SignatureMethod {request.SignatureMethodCode} does not exist.");
        }
    }

    private async Task InvalidateExistingSigningProcessesIfExist(StartSigningRequest request, __Household.Household houseHold)
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

    private StartSigningResponse MapToResponse(DocumentOnSa documentOnSaEntity)
    {
        return new StartSigningResponse
        {
            DocumentOnSa = new DocumentOnSA
            {
                DocumentOnSAId = documentOnSaEntity.DocumentOnSAId,
                DocumentTypeId = documentOnSaEntity.DocumentTypeId,
                FormId = documentOnSaEntity.FormId ?? string.Empty,
                HouseholdId = documentOnSaEntity.HouseholdId,
                IsValid = documentOnSaEntity.IsValid,
                IsSigned = documentOnSaEntity.IsSigned,
                IsDocumentArchived = documentOnSaEntity.IsDocumentArchived,
                SignatureMethodCode = documentOnSaEntity.SignatureMethodCode ?? string.Empty,
            }
        };
    }

    private __Entity.DocumentOnSa MapToEntity(StartSigningRequest request, __Household.Household houseHold, string formId, GetDocumentDataResponse getDocumentDataResponse)
    {
        var entity = new __Entity.DocumentOnSa();
        entity.DocumentTypeId = request.DocumentTypeId!.Value;
        entity.DocumentTemplateVersionId = getDocumentDataResponse.DocumentTemplateVersionId;
        entity.FormId = formId;
        entity.SalesArrangementId = request.SalesArrangementId!.Value;
        entity.HouseholdId = houseHold.HouseholdId;
        entity.SignatureMethodCode = request.SignatureMethodCode;
        entity.Data = JsonSerializer.Serialize(getDocumentDataResponse.DocumentData);
        entity.IsValid = true;
        entity.IsSigned = false;
        entity.IsDocumentArchived = false;
        return entity;
    }

    private async Task<__Household.Household> GetHouseholdId(int documentTypeId, int salesArrangementId, CancellationToken cancellationToken)
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
        _ => throw new CisValidationException(19001, $"DocumentOnTypeId {documentTypeId} does not exist.")
    };
}
