using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using __Household = DomainServices.HouseholdService.Contracts;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class StartSigningHandler : IRequestHandler<StartSigningRequest, StartSigningResponse>
{
    private const int _crsDocumentType = 14;
   

    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly IDataAggregatorServiceClient _dataAggregatorServiceClient;
    private readonly ICodebookServiceClient _codebookServiceClient;
    private readonly StartSigningMapper _startSigningMapper;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ICaseServiceClient _caseServiceClient;

    public StartSigningHandler(
        DocumentOnSAServiceDbContext dbContext,
        IMediator mediator,
        IHouseholdServiceClient householdClient,
        ISalesArrangementServiceClient arrangementServiceClient,
        IDataAggregatorServiceClient dataAggregatorServiceClient,
        ICodebookServiceClient codebookServiceClient,
        StartSigningMapper startSigningMapper,
        ICurrentUserAccessor currentUser,
        ICaseServiceClient caseServiceClient)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _householdClient = householdClient;
        _arrangementServiceClient = arrangementServiceClient;
        _dataAggregatorServiceClient = dataAggregatorServiceClient;
        _codebookServiceClient = codebookServiceClient;
        _startSigningMapper = startSigningMapper;
        _currentUser = currentUser;
        _caseServiceClient = caseServiceClient;
    }

    public async Task<StartSigningResponse> Handle(StartSigningRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId!.Value, cancellationToken);

        var salesArrangementType = await GetSalesArrangementType(salesArrangement, cancellationToken);

        DocumentOnSa documentOnSaEntity;
        if (salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest)
        {
            if (request.TaskId is not null) // workflow
                documentOnSaEntity = await ProcessWorkflowRequest(request, cancellationToken);
            else if (request.DocumentTypeId == _crsDocumentType) //CRS request
                documentOnSaEntity = await ProcessCrsRequest(request, salesArrangement, cancellationToken);
            else // Product request
                documentOnSaEntity = await ProcessProductRequest(request, salesArrangement, cancellationToken);
        }
        else if (salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ServiceRequest)
            documentOnSaEntity = await ProcessServiceRequest(request, salesArrangement, cancellationToken);
        else
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnsupportedKindOfSigningRequest);

        await _dbContext.DocumentOnSa.AddAsync(documentOnSaEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await UpdateSalesArrangementStateIfNeeded(salesArrangement, cancellationToken);

        return _startSigningMapper.MapToResponse(documentOnSaEntity);
    }

    private async Task<DocumentOnSa> ProcessServiceRequest(StartSigningRequest request, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        StartSigningBlValidator.ValidateServiceRequest(request);
        await ServiceRequestInvalidateExistingSigningProcessesIfExist(request, cancellationToken);
        var formIdResponse = await _mediator.Send(new GenerateFormIdRequest(), cancellationToken); // Not versioned formId (in future we have to implement support for versioning of SalesArrangementId)
        // If Service request doesn't have DocumentTypeId (mean without household), we have to get it via SalesArrangementTypeId 
        request.DocumentTypeId = await GetDocumentTypeIdForServiceRequest(request.DocumentTypeId, salesArrangement, cancellationToken);
        var documentData = await GetDocumentData(request, salesArrangement, cancellationToken);
        return await _startSigningMapper.ServiceRequestMapToEntity(request, formIdResponse.FormId, documentData, salesArrangement, cancellationToken);
    }

    private async Task<DocumentOnSa> ProcessProductRequest(StartSigningRequest request, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        StartSigningBlValidator.ValidateProductRequest(request);
        var houseHold = await GetHouseholdId(request.DocumentTypeId!.Value, request.SalesArrangementId!.Value, cancellationToken);
        // Check, if signing process has been already started. If started we have to invalidate exist signing processes
        await ProductRequestInvalidateExistingSigningProcessesIfExist(request, houseHold, cancellationToken);
        var formIdResponse = await _mediator.Send(new GenerateFormIdRequest { HouseholdId = houseHold.HouseholdId }, cancellationToken);
        var documentData = await GetDocumentData(request, salesArrangement, cancellationToken);
        return await _startSigningMapper.ProductRequestMapToEntity(request, houseHold, formIdResponse.FormId, documentData, cancellationToken);
    }

    private async Task<DocumentOnSa> ProcessCrsRequest(StartSigningRequest request, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        StartSigningBlValidator.ValidateCrsRequest(request);
        await CrsInvalidateExistingSigningProcessesIfExist(request, cancellationToken);
        var formIdResponse = await _mediator.Send(new GenerateFormIdRequest(), cancellationToken); // Not versioned formId
        var documentData = await GetDocumentData(request, salesArrangement, cancellationToken);
        return await _startSigningMapper.CrsMapToEntity(request, formIdResponse.FormId, documentData, cancellationToken);
    }

    private async Task<DocumentOnSa> ProcessWorkflowRequest(StartSigningRequest request, CancellationToken cancellationToken)
    {
        StartSigningBlValidator.ValidateWorkflowRequest(request);
        await WorkflowInvalidateExistingSigningProcessesIfExist(request, cancellationToken);
        var taskDetail = await _caseServiceClient.GetTaskDetail(request.TaskId!.Value, cancellationToken);
        return await _startSigningMapper.WorkflowMapToEntity(request, taskDetail, cancellationToken);
    }

    private async Task<int> GetDocumentTypeIdForServiceRequest(int? documentTypeId, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        // Service request without household
        if (documentTypeId is null)
        {
            var documentTypes = await _codebookServiceClient.DocumentTypes(cancellationToken);
            var documentType = documentTypes.Single(d => d.SalesArrangementTypeId == salesArrangement.SalesArrangementTypeId);
            return documentType.Id;
        }
        else // With household
        {
            return documentTypeId.Value;
        }
    }

    private async Task<GetDocumentDataResponse> GetDocumentData(StartSigningRequest request, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        return await _dataAggregatorServiceClient.GetDocumentData(new()
        {
            DocumentTypeId = request.DocumentTypeId!.Value,
            DocumentTemplateVersionId = request.DocumentTemplateVersionId,
            DocumentTemplateVariantId = request.DocumentTemplateVariantId,
            InputParameters = new()
            {
                SalesArrangementId = request.SalesArrangementId!.Value,
                CaseId = salesArrangement.CaseId,
                UserId = _currentUser.User?.Id,

            }
        }, cancellationToken);
    }

    private async Task UpdateSalesArrangementStateIfNeeded(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        switch (salesArrangement.State)
        {
            case (int)SalesArrangementStates.InSigning://(7 Podepisování)
                break; // Skip state change
            case (int)SalesArrangementStates.InProgress://(1 Rozpracováno)
                await _arrangementServiceClient.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, (int)SalesArrangementStates.InSigning, cancellationToken);
                break;
            default:
                throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.UnableToStartSigningOrSignInvalidSalesArrangementState);
        }
    }

    private async Task ProductRequestInvalidateExistingSigningProcessesIfExist(StartSigningRequest request, __Household.Household houseHold, CancellationToken cancellationToken)
    {
        var existSigningProcesses = await _dbContext.DocumentOnSa.Where(e => e.SalesArrangementId == request.SalesArrangementId
                                                         && e.HouseholdId == houseHold.HouseholdId && e.IsValid)
                                                        .ToListAsync(cancellationToken);

        existSigningProcesses.ForEach(s => s.IsValid = false);
    }

    private async Task CrsInvalidateExistingSigningProcessesIfExist(StartSigningRequest request, CancellationToken cancellationToken)
    {
        var existSigningProcesses = await _dbContext.DocumentOnSa.Where(e => e.SalesArrangementId == request.SalesArrangementId
                                                        && e.DocumentTypeId == request.DocumentTypeId && e.CustomerOnSAId1 == request.CustomerOnSAId1 && e.IsValid)
                                                       .ToListAsync(cancellationToken);

        existSigningProcesses.ForEach(s => s.IsValid = false);
    }

    private async Task WorkflowInvalidateExistingSigningProcessesIfExist(StartSigningRequest request, CancellationToken cancellationToken)
    {
        var existSigningProcesses = await _dbContext.DocumentOnSa.Where(e => e.SalesArrangementId == request.SalesArrangementId
                                                        && e.TaskId == request.TaskId && e.IsValid)
                                                       .ToListAsync(cancellationToken);

        existSigningProcesses.ForEach(s => s.IsValid = false);
    }

    private async Task ServiceRequestInvalidateExistingSigningProcessesIfExist(StartSigningRequest request, CancellationToken cancellationToken)
    {
        // SalesArragmentId is unique for service request
        var existSigningProcesses = await _dbContext.DocumentOnSa.Where(e => e.SalesArrangementId == request.SalesArrangementId && e.IsValid)
                                                     .ToListAsync(cancellationToken);

        existSigningProcesses.ForEach(s => s.IsValid = false);
    }

    private async Task<__Household.Household> GetHouseholdId(int documentTypeId, int salesArrangementId, CancellationToken cancellationToken)
    {
        var householdTypeId = GetHouseholdTypeId(documentTypeId);

        var houseHolds = await _householdClient.GetHouseholdList(salesArrangementId, cancellationToken);
        var houseHold = houseHolds.SingleOrDefault(r => r.HouseholdTypeId == householdTypeId);

        return houseHold is null
            ? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ForSpecifiedDocumentTypeIdCannotFindHousehold, documentTypeId)
            : houseHold;
    }

    private async Task<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem> GetSalesArrangementType(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var salesArrangementTypes = await _codebookServiceClient.SalesArrangementTypes(cancellationToken);
        return salesArrangementTypes.Single(r => r.Id == salesArrangement.SalesArrangementTypeId);
    }

    private static int GetHouseholdTypeId(int documentTypeId) => documentTypeId switch
    {
        4 => 1,
        5 => 2,
        _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.DocumentTypeIdNotSupportedForProductRequest, documentTypeId)
    };
}
