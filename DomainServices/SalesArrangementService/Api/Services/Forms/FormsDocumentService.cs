using System.Globalization;
using CIS.Core;
using CIS.Core.Attributes;
using CIS.Core.Security;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService;
using DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService.Entities;
using DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Contracts;
using Newtonsoft.Json;
using static DomainServices.UserService.Contracts.v1.UserService;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal sealed class FormsDocumentService
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorService;
    private readonly ICodebookServiceClients _codebookService;
    private readonly IDocumentArchiveRepository _documentArchiveRepository;
    private readonly IDateTime _dateTime;
    private readonly IUserServiceClient _userService;

    public FormsDocumentService(IDocumentOnSAServiceClient documentOnSAService,
                                IDocumentArchiveServiceClient documentArchiveService,
                                IDocumentGeneratorServiceClient documentGeneratorService,
                                ICodebookServiceClients codebookService,
                                IDocumentArchiveRepository documentArchiveRepository,
                                IDateTime dateTime,
                                IUserServiceClient userService)
    {
        _documentOnSAService = documentOnSAService;
        _documentArchiveService = documentArchiveService;
        _documentGeneratorService = documentGeneratorService;
        _codebookService = codebookService;
        _documentArchiveRepository = documentArchiveRepository;
        _dateTime = dateTime;
        _userService = userService;
    }

    /// <summary>
    /// Prepare DocumentInterface (Form) with FormInstanceInterface (data sentense) entities.
    /// </summary>
    public async Task SaveEasForms(GetEasFormResponse easFormResponse, SalesArrangement salesArrangement, IReadOnlyCollection<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        var formsWithDataSentenses = createdFinalVersionOfDocOnSa.OrderBy(r => r.DocumentOnSa.EArchivId)
                 .Zip(easFormResponse.Forms.OrderBy(r => r.DynamicFormValues.DocumentId),
                 (docOnSa, form) => new { docOnSa, form });

        // load user
        var user = await _userService.GetUser(salesArrangement.Created.UserId!.Value, cancellationToken);

        var entities = await Task.WhenAll(
            formsWithDataSentenses
                .Select(r => PrepareEntity(r.docOnSa, r.form, salesArrangement, easFormResponse.ContractNumber, user, cancellationToken)));

        await _documentArchiveRepository.SaveDataSentenseWithForm(entities, cancellationToken);
    }

    private async Task<DocumentInterface> PrepareEntity(CreateDocumentOnSAResponse docOnSa, Form form, SalesArrangement salesArrangement, string contractNumber, User user, CancellationToken cancellationToken)
    {
        var entity = new DocumentInterface();
        entity.DocumentId = docOnSa.DocumentOnSa.EArchivId;
        var generatedDocument = await GenerateDocument(salesArrangement, docOnSa, cancellationToken);
        entity.DocumentData = generatedDocument.Data.ToArrayUnsafe();
        entity.FileName = await GetFileName(docOnSa.DocumentOnSa, cancellationToken);
        entity.FileNameSuffix = Path.GetExtension(entity.FileName)[1..];
        entity.CaseId = salesArrangement.CaseId;
        entity.AuthorUserLogin = GetAuthorUserLogin(user);
        entity.ContractNumber = contractNumber;
        entity.FormId = docOnSa.DocumentOnSa.FormId;
        entity.CreatedOn = _dateTime.Now.Date;
        entity.EaCodeMainId = form.DefaultValues.EaCodeMainId ?? 0;
        entity.Kdv = 1; // true
        entity.SendDocumentOnly = 0; //false
        entity.DataSentence = new FormInstanceInterface
        {
            DocumentId = form.DynamicFormValues.DocumentId,
            FormType = form.DefaultValues.FormType,
            FormKind = "N",
            Cpm = user.CPM ?? string.Empty,
            Icp = user.ICP ?? string.Empty,
            Status = 100,
            CreatedAt = _dateTime.Now,
            Storno = 0,
            DataType = 1,
            JsonDataClob = form.Json
        };

        return entity;
    }

    private static string GetAuthorUserLogin(User user)
    {
        if (!string.IsNullOrWhiteSpace(user.ICP))
            return user.ICP;
        else if (!string.IsNullOrWhiteSpace(user.CPM))
            return user.CPM;
        else
            return user.Id.ToString(CultureInfo.InvariantCulture);
    }

    public async Task<CreateDocumentOnSAResponse> CreateFinalDocumentOnSa(int salesArrangementId, DynamicFormValues dynamicValue, CancellationToken cancellationToken)
    {
        var request = new CreateDocumentOnSARequest
        {
            SalesArrangementId = salesArrangementId,
            DocumentTypeId = dynamicValue.DocumentTypeId,
            FormId = await _documentOnSAService.GenerateFormId(new GenerateFormIdRequest { IsFormIdFinal = true }, cancellationToken),
            EArchivId = await _documentArchiveService.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken),
            IsFinal = true
        };

        return await _documentOnSAService.CreateDocumentOnSA(request, cancellationToken);
    }

    public async Task<Document> GenerateDocument(SalesArrangement salesArrangement, CreateDocumentOnSAResponse documentOnSaResponse, CancellationToken cancellationToken)
    {
        var documentOnSaData = await _documentOnSAService.GetDocumentOnSAData(documentOnSaResponse.DocumentOnSa.DocumentOnSAId!.Value, cancellationToken);

        var generateDocumentRequest = CreateDocumentRequest(documentOnSaResponse.DocumentOnSa, documentOnSaData, salesArrangement);

        return await _documentGeneratorService.GenerateDocument(generateDocumentRequest, cancellationToken);
    }

    private static IEnumerable<GenerateDocumentPartData> CreateData(List<DocumentDataDto>? documentDataDtos)
    {
        ArgumentNullException.ThrowIfNull(documentDataDtos);

        foreach (var documentDataDto in documentDataDtos)
        {
            var documentPartData = new GenerateDocumentPartData
            {
                Key = documentDataDto.FieldName,
                StringFormat = documentDataDto.StringFormat,
                TextAlign = (TextAlign)(documentDataDto.TextAlign ?? 0)
            };

            switch (documentDataDto.ValueCase)
            {
                case 0: break;
                case 3:
                    documentPartData.Text = documentDataDto.Text ?? string.Empty;
                    break;
                case 4:
                    documentPartData.Date = new DateTime(documentDataDto.Date!.Year, documentDataDto.Date!.Month, documentDataDto.Date!.Day);
                    break;
                case 5:
                    documentPartData.Number = documentDataDto.Number;
                    break;
                case 6:
                    documentPartData.DecimalNumber = new GrpcDecimal(documentDataDto.DecimalNumber!.Units, documentDataDto.DecimalNumber!.Nanos);
                    break;
                case 7:
                    documentPartData.LogicalValue = documentDataDto.LogicalValue;
                    break;
                case 8:
                    throw new NotSupportedException("GenericTable is not supported");
                default:
                    throw new NotSupportedException("Notsupported oneof object");
            }

            yield return documentPartData;
        }
    }

    private async Task<string> GetFileName(DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var templates = await _codebookService.DocumentTypes(cancellationToken);
        var fileName = templates.First(t => t.Id == (int)documentOnSa.DocumentTypeId!).FileName;
        return $"{fileName}_{documentOnSa.DocumentOnSAId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf";
    }

    private static GenerateDocumentRequest CreateDocumentRequest(DocumentOnSAToSign documentOnSa, GetDocumentOnSADataResponse documentOnSaData, SalesArrangement salesArrangement)
    {
        return new GenerateDocumentRequest
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSaData.DocumentTemplateVersionId!.Value,
            ForPreview = false,
            OutputType = OutputFileType.Pdfa,
            Parts = { CreateDocPart(documentOnSaData) },
            DocumentFooter = CreateFooter(salesArrangement, documentOnSa)
        };
    }

    private static GenerateDocumentPart CreateDocPart(GetDocumentOnSADataResponse documentOnSaData)
    {
        var docPart = new GenerateDocumentPart
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSaData.DocumentTemplateVersionId!.Value
        };

        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSaData.Data);
        docPart.Data.AddRange(CreateData(documentDataDtos));
        return docPart;
    }

    private static DocumentFooter CreateFooter(SalesArrangement salesArrangement, DocumentOnSAToSign documentOnSA)
    {
        return new DocumentFooter
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            CaseId = salesArrangement.CaseId,
            BarcodeText = documentOnSA.FormId
        };
    }
}