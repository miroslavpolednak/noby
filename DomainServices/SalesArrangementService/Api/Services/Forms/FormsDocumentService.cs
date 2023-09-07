using System.Globalization;
using CIS.Core;
using CIS.Core.Attributes;
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
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Contracts;
using Newtonsoft.Json;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal sealed class FormsDocumentService
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly IDocumentArchiveRepository _documentArchiveRepository;
    private readonly IDateTime _dateTime;
    private readonly IUserServiceClient _userService;

    public FormsDocumentService(IDocumentOnSAServiceClient documentOnSAService,
                                IDocumentArchiveServiceClient documentArchiveService,
                                IDocumentGeneratorServiceClient documentGeneratorService,
                                ICodebookServiceClient codebookService,
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
            Cpm = user.UserInfo.Cpm ?? string.Empty,
            Icp = user.UserInfo.Icp ?? string.Empty,
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
        if (!string.IsNullOrWhiteSpace(user.UserInfo.Icp))
            return user.UserInfo.Icp;
        else if (!string.IsNullOrWhiteSpace(user.UserInfo.Cpm))
            return user.UserInfo.Cpm;
        else
            return user.UserId.ToString(CultureInfo.InvariantCulture);
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
            DocumentTemplateVariantId = documentOnSaData.DocumentTemplateVariantId,
            ForPreview = false,
            OutputType = OutputFileType.Pdfa,
            Parts = { CreateDocPart(documentOnSaData) },
            DocumentFooter = CreateFooter(salesArrangement, documentOnSa)
        };
    }

    private static GenerateDocumentPart CreateDocPart(GetDocumentOnSADataResponse documentOnSaData)
    {
        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSaData.Data)!;

        var docPart = new GenerateDocumentPart
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSaData.DocumentTemplateVersionId!.Value,
            DocumentTemplateVariantId = documentOnSaData.DocumentTemplateVariantId,
            Data = { documentDataDtos.CreateDocumentData() }
        };
        return docPart;
    }

    private static DocumentFooter CreateFooter(SalesArrangement salesArrangement, DocumentOnSAToSign documentOnSA)
    {
        return new DocumentFooter
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            CaseId = salesArrangement.CaseId,
            DocumentId = documentOnSA.EArchivId,
            BarcodeText = documentOnSA.FormId
        };
    }
}