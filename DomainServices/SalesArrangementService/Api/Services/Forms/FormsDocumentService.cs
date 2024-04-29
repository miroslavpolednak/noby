using System.Globalization;
using System.Runtime.CompilerServices;
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
    private readonly IUserServiceClient _userService;

    public FormsDocumentService(IDocumentOnSAServiceClient documentOnSAService,
                                IDocumentArchiveServiceClient documentArchiveService,
                                IDocumentGeneratorServiceClient documentGeneratorService,
                                ICodebookServiceClient codebookService,
                                IDocumentArchiveRepository documentArchiveRepository,
                                IUserServiceClient userService)
    {
        _documentOnSAService = documentOnSAService;
        _documentArchiveService = documentArchiveService;
        _documentGeneratorService = documentGeneratorService;
        _codebookService = codebookService;
        _documentArchiveRepository = documentArchiveRepository;
        _userService = userService;
    }

    public async IAsyncEnumerable<CreateDocumentOnSAResponse> CreateFinalDocumentsOnSa(int salesArrangementId, [EnumeratorCancellation] CancellationToken cancellationToken, params DynamicFormValues[] dynamicValues)
    {
        foreach (var dynamicValue in dynamicValues)
        {
            var request = new CreateDocumentOnSARequest
            {
                SalesArrangementId = salesArrangementId,
                DocumentTypeId = dynamicValue.DocumentTypeId,
                FormId = await _documentOnSAService.GenerateFormId(new GenerateFormIdRequest { IsFormIdFinal = true }, cancellationToken),
                EArchivId = await _documentArchiveService.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken),
                IsFinal = true
            };

            yield return await _documentOnSAService.CreateDocumentOnSA(request, cancellationToken);
        }
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
                .Select(r => PrepareEntity(r.form, salesArrangement, r.docOnSa.DocumentOnSa.DocumentOnSAId!.Value, easFormResponse.ContractNumber, user, cancellationToken)));

        await _documentArchiveRepository.SaveDataSentenseWithForm(entities, cancellationToken);
    }

    public async Task SaveEasForms(GetEasFormResponse easFormResponse, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(salesArrangement.Created.UserId!.Value, cancellationToken);

        var formsToSave = new List<DocumentInterface>(easFormResponse.Forms.Count);

        foreach (var form in easFormResponse.Forms)
        {
            var documentOnSaResponse = await _documentOnSAService.GetDocumentOnSAByFormId(form.DynamicFormValues.FormId, cancellationToken);

            var entity = await PrepareEntity(form, salesArrangement, documentOnSaResponse.DocumentOnSa.DocumentOnSAId!.Value, easFormResponse.ContractNumber, user, cancellationToken);

            formsToSave.Add(entity);
        }

        await _documentArchiveRepository.SaveDataSentenseWithForm(formsToSave.ToArray(), cancellationToken);
    }

    private async Task<DocumentInterface> PrepareEntity(Form form, SalesArrangement salesArrangement, int documentOnSaId, string contractNumber, User user, CancellationToken cancellationToken)
    {
        var generatedDocument = await GenerateDocument(salesArrangement, documentOnSaId, form.DynamicFormValues, cancellationToken);

        var entity = new DocumentInterface
        {
            DocumentId = form.DynamicFormValues.DocumentId,
            DocumentData = generatedDocument.Data.ToArrayUnsafe(),
            FileName = await GetFileName(documentOnSaId, form.DynamicFormValues.DocumentTypeId, cancellationToken),
            CaseId = salesArrangement.CaseId,
            AuthorUserLogin = GetAuthorUserLogin(user),
            ContractNumber = contractNumber,
            FormId = form.DynamicFormValues.FormId,
            CreatedOn = DateTime.Now.Date,
            EaCodeMainId = form.DefaultValues.EaCodeMainId ?? 0,
            Kdv = 1, // true
            SendDocumentOnly = 0, //false
            DataSentence = new FormInstanceInterface
            {
                DocumentId = form.DynamicFormValues.DocumentId,
                FormType = form.DefaultValues.FormType,
                FormKind = "N",
                Cpm = user.UserInfo.Cpm ?? string.Empty,
                Icp = user.UserInfo.Icp ?? string.Empty,
                Status = 100,
                CreatedAt = DateTime.Now,
                Storno = 0,
                DataType = 1,
                JsonDataClob = form.Json,
                FormIdentifier = form.FormIdentifier
            }
        };

        entity.FileNameSuffix = Path.GetExtension(entity.FileName)[1..];

        return entity;
    }

    private static string GetAuthorUserLogin(User user)
    {
        if (!string.IsNullOrWhiteSpace(user.UserInfo.Icp))
            return user.UserInfo.Icp;

        if (!string.IsNullOrWhiteSpace(user.UserInfo.Cpm))
            return user.UserInfo.Cpm;

        return user.UserId.ToString(CultureInfo.InvariantCulture);
    }

    private async Task<Document> GenerateDocument(SalesArrangement salesArrangement, int documentOnSaId, DynamicFormValues dynamicFormValues, CancellationToken cancellationToken)
    {
        var documentOnSaData = await _documentOnSAService.GetDocumentOnSAData(documentOnSaId, cancellationToken);

        var generateDocumentRequest = new GenerateDocumentRequest
        {
            DocumentTypeId = documentOnSaData.DocumentTypeId!.Value,
            DocumentTemplateVersionId = documentOnSaData.DocumentTemplateVersionId!.Value,
            DocumentTemplateVariantId = documentOnSaData.DocumentTemplateVariantId,
            ForPreview = false,
            OutputType = OutputFileType.Pdfa,
            Parts = { CreateDocPart(documentOnSaData) },
            DocumentFooter = CreateFooter(salesArrangement, dynamicFormValues, documentOnSaId)
        };

        return await _documentGeneratorService.GenerateDocument(generateDocumentRequest, cancellationToken);
    }

    private async Task<string> GetFileName(int documentOnSaId, int documentTypeId, CancellationToken cancellationToken)
    {
        var templates = await _codebookService.DocumentTypes(cancellationToken);
        var fileName = templates.First(t => t.Id == documentTypeId).FileName;

        return $"{fileName}_{documentOnSaId}_{DateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf";
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

    private static DocumentFooter CreateFooter(SalesArrangement salesArrangement, DynamicFormValues dynamicFormValues, int documentOnSaId)
    {
        return new DocumentFooter
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            CaseId = salesArrangement.CaseId,
            DocumentOnSaId = documentOnSaId,
            DocumentId = dynamicFormValues.DocumentId,
            BarcodeText = dynamicFormValues.FormId
        };
    }
}