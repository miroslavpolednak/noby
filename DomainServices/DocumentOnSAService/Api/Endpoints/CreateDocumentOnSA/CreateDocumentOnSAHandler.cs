using CIS.Core.Security;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using Google.Protobuf.WellKnownTypes;
using System.Text.Json;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.CreateDocumentOnSA;

public class CreateDocumentOnSAHandler : IRequestHandler<CreateDocumentOnSARequest, CreateDocumentOnSAResponse>
{
    private readonly IDataAggregatorServiceClient _dataAggregatorServiceClient;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly DocumentOnSAServiceDbContext _dbContext;

    public CreateDocumentOnSAHandler(
        IDataAggregatorServiceClient dataAggregatorServiceClient,
        ISalesArrangementServiceClient arrangementServiceClient,
        ICurrentUserAccessor currentUser,
        DocumentOnSAServiceDbContext dbContext)
    {
        _dataAggregatorServiceClient = dataAggregatorServiceClient;
        _arrangementServiceClient = arrangementServiceClient;
        _currentUser = currentUser;
        _dbContext = dbContext;
    }

    public async Task<CreateDocumentOnSAResponse> Handle(CreateDocumentOnSARequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId!.Value, cancellationToken);

        if (salesArrangement is null)
        {
            throw new CisNotFoundException(19000, $"SalesArrangement{request.SalesArrangementId} does not exist.");
        }

        var documentData = await _dataAggregatorServiceClient.GetDocumentData(new()
        {
            DocumentTypeId = request.DocumentTypeId!.Value,
            DocumentTemplateVersionId = request.DocumentTemplateVersionId,
            DocumentTemplateVariantId = request.DocumentTemplateVariantId,
            InputParameters = new()
            {
                SalesArrangementId = request.SalesArrangementId!.Value,
                CaseId = salesArrangement.CaseId,
                UserId = _currentUser.User?.Id,
                IsDocumentFinal = request.IsFinal
            }
        }, cancellationToken);

        var documentOnSaEntity = MapToEntity(request, documentData);
        await _dbContext.DocumentOnSa.AddAsync(documentOnSaEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MapToResponse(documentOnSaEntity);
    }

    private CreateDocumentOnSAResponse MapToResponse(DocumentOnSa documentOnSaEntity)
    {
        return new CreateDocumentOnSAResponse
        {
            DocumentOnSa = new DocumentOnSAToSign
            {
                DocumentOnSAId = documentOnSaEntity.DocumentOnSAId,
                DocumentTypeId = documentOnSaEntity.DocumentTypeId,
                DocumentTemplateVersionId = documentOnSaEntity.DocumentTemplateVersionId,
                DocumentTemplateVariantId = documentOnSaEntity.DocumentTemplateVariantId,
                FormId = documentOnSaEntity.FormId ?? string.Empty,
                EArchivId = documentOnSaEntity.EArchivId ?? string.Empty,
                DmsxId = documentOnSaEntity.DmsxId ?? string.Empty,
                SalesArrangementId = documentOnSaEntity.SalesArrangementId,
                HouseholdId = documentOnSaEntity.HouseholdId,
                IsValid = documentOnSaEntity.IsValid,
                IsSigned = documentOnSaEntity.IsSigned,
                IsDocumentArchived = documentOnSaEntity.IsDocumentArchived,
                SignatureMethodCode = documentOnSaEntity.SignatureMethodCode ?? string.Empty,
                SignatureDateTime = documentOnSaEntity.SignatureDateTime is not null ? Timestamp.FromDateTime(DateTime.SpecifyKind(documentOnSaEntity.SignatureDateTime.Value, DateTimeKind.Utc)) : null,
                SignatureConfirmedBy = documentOnSaEntity.SignatureConfirmedBy
            }
        };
    }

    private DocumentOnSa MapToEntity(CreateDocumentOnSARequest request, GetDocumentDataResponse dataResponse)
    {
        return new DocumentOnSa
        {
            DocumentTypeId = request.DocumentTypeId!.Value,
            DocumentTemplateVersionId = dataResponse.DocumentTemplateVersionId,
            DocumentTemplateVariantId = dataResponse.DocumentTemplateVariantId,
            FormId = request.FormId,
            EArchivId = request.EArchivId,
            SalesArrangementId = request.SalesArrangementId!.Value,
            Data = JsonSerializer.Serialize(dataResponse.DocumentData),
            IsValid = true,
            IsSigned = false,
            IsDocumentArchived = false,
            IsFinal = request.IsFinal,
        };
    }
}
