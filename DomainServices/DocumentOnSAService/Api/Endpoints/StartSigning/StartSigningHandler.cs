using CIS.Core.Security;
using SharedTypes.Enums;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CaseService.Clients.v1;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.ESignatures.V1;
using FastEnumUtility;
using Microsoft.EntityFrameworkCore;
using __Household = DomainServices.HouseholdService.Contracts;
using DomainServices.DocumentOnSAService.Api.Common;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class StartSigningHandler : IRequestHandler<StartSigningRequest, StartSigningResponse>
{
    private const int _crsDocumentType = 13;

    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ISalesArrangementServiceClient _salesArrangementServiceClient;
    private readonly IDataAggregatorServiceClient _dataAggregatorServiceClient;
    private readonly StartSigningMapper _startSigningMapper;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly ICommonSigningMethods _commonSigning;

    public StartSigningHandler(
        DocumentOnSAServiceDbContext dbContext,
        IMediator mediator,
        IHouseholdServiceClient householdClient,
        ISalesArrangementServiceClient salesArrangementServiceClient,
        IDataAggregatorServiceClient dataAggregatorServiceClient,
        StartSigningMapper startSigningMapper,
        ICurrentUserAccessor currentUser,
        ICaseServiceClient caseServiceClient,
        IESignaturesClient eSignaturesClient,
        ICommonSigningMethods commonSigning)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _householdClient = householdClient;
        _salesArrangementServiceClient = salesArrangementServiceClient;
        _dataAggregatorServiceClient = dataAggregatorServiceClient;
        _startSigningMapper = startSigningMapper;
        _currentUser = currentUser;
        _caseServiceClient = caseServiceClient;
        _eSignaturesClient = eSignaturesClient;
        _commonSigning = commonSigning;
    }

    public async Task<StartSigningResponse> Handle(StartSigningRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var salesArrangement = await _salesArrangementServiceClient.GetSalesArrangement(request.SalesArrangementId!.Value, cancellationToken);

        var salesArrangementType = await _commonSigning.GetSalesArrangementType(salesArrangement, cancellationToken);

        var documentOnSaEntity = request switch
        {
            //Workflow
            _ when request.TaskIdSb is not null => await ProcessWorkflowRequest(request, cancellationToken),
            //CRS request
            _ when request.DocumentTypeId == _crsDocumentType => await ProcessCrsRequest(request, salesArrangement, cancellationToken),
            // Product request
            _ when salesArrangementType.SalesArrangementCategory ==  SalesArrangementCategories.ProductRequest.ToByte() => await ProcessProductRequest(request, salesArrangement, cancellationToken),
            // Service request
            _ when salesArrangementType.SalesArrangementCategory == SalesArrangementCategories.ServiceRequest.ToByte() => await ProcessServiceRequest(request, salesArrangement, cancellationToken),
            _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnsupportedKindOfSigningRequest)
        };

        try
        {
            //Insert new DocumentOnSA and get DocumentOnSaId
            await _dbContext.DocumentOnSa.AddAsync(documentOnSaEntity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // For CRS, Product and Service request  
            if (documentOnSaEntity.TaskId is null && documentOnSaEntity.SignatureTypeId is not null && documentOnSaEntity.SignatureTypeId == SignatureTypes.Electronic.ToByte())
            {
                var prepareDocumentRequest = await _startSigningMapper.MapPrepareDocumentRequest(documentOnSaEntity, salesArrangement, cancellationToken);
                var referenceId = await _eSignaturesClient.PrepareDocument(prepareDocumentRequest, cancellationToken);
                var uploadDocumentRequest = await _startSigningMapper.MapUploadDocumentRequest(referenceId, prepareDocumentRequest.DocumentData.FileName, salesArrangement, documentOnSaEntity, cancellationToken);
                var (externalId, _) = await _eSignaturesClient.UploadDocument(uploadDocumentRequest.ReferenceId, uploadDocumentRequest.Filename, uploadDocumentRequest.CreationDate, uploadDocumentRequest.FileData, cancellationToken);
                
                documentOnSaEntity.ExternalIdESignatures = externalId;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            if (request.TaskId is null)
                await UpdateSalesArrangementStateIfNeeded(salesArrangement, cancellationToken);

            return _startSigningMapper.MapToResponse(documentOnSaEntity);
        }
        catch
        {
            _dbContext.DocumentOnSa.Remove(documentOnSaEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            throw;
        }
    }

    private async Task<DocumentOnSa> ProcessServiceRequest(StartSigningRequest request, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        StartSigningBlValidator.ValidateServiceRequest(request);
        await ServiceRequestInvalidateExistingSigningProcessesIfExist(request, cancellationToken);
        var household = await GetHouseholdForCodeptor(request.SalesArrangementId!.Value, cancellationToken);
        // Not versioned formId for service request without household (in future we have to implement support for versioning of SalesArrangementId)
        var formIdResponse = await _mediator.Send(new GenerateFormIdRequest { HouseholdId = household?.HouseholdId }, cancellationToken);
        var documentData = await GetDocumentData(request, salesArrangement, null, cancellationToken);
        return await _startSigningMapper.ServiceRequestMapToEntity(request, household, formIdResponse.FormId, documentData, salesArrangement, cancellationToken);
    }

    private async Task<DocumentOnSa> ProcessProductRequest(StartSigningRequest request, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        StartSigningBlValidator.ValidateProductRequest(request);
        var houseHold = await GetHousehold(request.DocumentTypeId!.Value, request.SalesArrangementId!.Value, cancellationToken);
        // Check, if signing process has been already started. If started we have to invalidate exist signing processes
        await ProductRequestInvalidateExistingSigningProcessesIfExist(request, houseHold, cancellationToken);
        var formIdResponse = await _mediator.Send(new GenerateFormIdRequest { HouseholdId = houseHold.HouseholdId }, cancellationToken);
        var documentData = await GetDocumentData(request, salesArrangement, null, cancellationToken);
        return await _startSigningMapper.ProductRequestMapToEntity(request, houseHold, formIdResponse.FormId, documentData, cancellationToken);
    }

    private async Task<DocumentOnSa> ProcessCrsRequest(StartSigningRequest request, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        StartSigningBlValidator.ValidateCrsRequest(request);
        await CrsInvalidateExistingSigningProcessesIfExist(request, cancellationToken);
        var formIdResponse = await _mediator.Send(new GenerateFormIdRequest(), cancellationToken); // Not versioned formId
        // Only CRS has to send CustomerOnSAId1
        var documentData = await GetDocumentData(request, salesArrangement, request.CustomerOnSAId1, cancellationToken);
        return await _startSigningMapper.CrsMapToEntity(request, formIdResponse.FormId, documentData, cancellationToken);
    }

    private async Task<DocumentOnSa> ProcessWorkflowRequest(StartSigningRequest request, CancellationToken cancellationToken)
    {
        StartSigningBlValidator.ValidateWorkflowRequest(request);
        await WorkflowInvalidateExistingSigningProcessesIfExist(request, cancellationToken);
        var taskDetail = await _caseServiceClient.GetTaskDetail(request.TaskIdSb!.Value, cancellationToken);
        return await _startSigningMapper.WorkflowMapToEntity(request, taskDetail, cancellationToken);
    }

    private async Task<GetDocumentDataResponse> GetDocumentData(StartSigningRequest request, SalesArrangement salesArrangement, int? customerOnSAId1, CancellationToken cancellationToken)
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
                CustomerOnSaId = customerOnSAId1
            }
        }, cancellationToken);
    }

    private async Task UpdateSalesArrangementStateIfNeeded(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        switch (salesArrangement.State)
        {
            case (int)EnumSalesArrangementStates.InSigning://(7 Podepisování)
                break; // Skip state change
            case (int)EnumSalesArrangementStates.InProgress://(1 Rozpracováno)
                await _salesArrangementServiceClient.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, (int)EnumSalesArrangementStates.InSigning, cancellationToken);
                break;
            default:
                throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.SigningInvalidSalesArrangementState);
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
                                                        && e.TaskIdSb == request.TaskIdSb && e.IsValid)
                                                       .ToListAsync(cancellationToken);

        existSigningProcesses.ForEach(s => s.IsValid = false);
    }

    private async Task ServiceRequestInvalidateExistingSigningProcessesIfExist(StartSigningRequest request, CancellationToken cancellationToken)
    {
        // SalesArragmentId is unique for service request
        var existSigningProcesses = await _dbContext.DocumentOnSa.Where(e => e.SalesArrangementId == request.SalesArrangementId
                                                                        && e.DocumentTypeId == request.DocumentTypeId
                                                                        && e.IsValid)
                                                     .ToListAsync(cancellationToken);

        existSigningProcesses.ForEach(s => s.IsValid = false);
    }

    private async Task<__Household.Household?> GetHouseholdForCodeptor(int salesArrangementId, CancellationToken cancellationToken)
    {
        var houseHolds = await _householdClient.GetHouseholdList(salesArrangementId, cancellationToken);
        return houseHolds.SingleOrDefault(r => r.HouseholdTypeId == HouseholdTypes.Codebtor.ToByte());
    }

    private async Task<__Household.Household> GetHousehold(int documentTypeId, int salesArrangementId, CancellationToken cancellationToken)
    {
        var householdTypeId = GetHouseholdTypeId((DocumentTypes)documentTypeId);

        var houseHolds = await _householdClient.GetHouseholdList(salesArrangementId, cancellationToken);
        var houseHold = houseHolds.SingleOrDefault(r => r.HouseholdTypeId == householdTypeId);

        return houseHold is null
            ? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ForSpecifiedDocumentTypeIdCannotFindHousehold, documentTypeId)
            : houseHold;
    }

    private static int GetHouseholdTypeId(DocumentTypes documentType) => documentType switch
    {
        // 4 
        DocumentTypes.ZADOSTHU => HouseholdTypes.Main.ToByte(),// 1,
        // 5 
        DocumentTypes.ZADOSTHD => HouseholdTypes.Codebtor.ToByte(),// 2
        _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.DocumentTypeIdNotSupportedForProductRequest, documentType.ToByte())
    };
}
