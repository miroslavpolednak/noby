using CIS.Foms.Types.Enums;
using FastEnumUtility;

namespace ExternalServices.ESignatures.V1;

internal sealed class RealESignaturesClient
    : IESignaturesClient
{
    public async Task<EDocumentStatuses> GetDocumentStatus(string documentId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}/REST/v2/DocumentService/GetDocumentStatus?id={documentId}", cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<Contracts.ResponseStatus>(cancellationToken: cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(GetDocumentStatus), nameof(Contracts.ResponseStatus));

        if ((result.Result?.Code ?? 0) == 0)
        {
            if (FastEnum.TryParse<EDocumentStatuses>(result.Status!, true, out EDocumentStatuses status))
            { 
                return status;
            }
            else
            {
                throw new CisExtServiceValidationException($"Returned status '{result.Status}' is unknown");
            }
        }
        else 
        {
            throw new CisExtServiceValidationException(result.Result!.Code.GetValueOrDefault(), result.Result.Message ?? "");
        }
    }

    public Task DownloadDocumentPreview(string externalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SubmitDispatchForm(bool documentsValid, List<Dto.DispatchFormClientDocument> documents, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<(int? Code, string? Message)> SendDocumentPreview(string externalId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}/REST/v2/DocumentService/SendDocumentPreview?externalId={externalId}", null, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<Contracts.ProcessingResult>(cancellationToken: cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(SendDocumentPreview), nameof(Contracts.ProcessingResult));

        return (result.Code, result.Message);
    }

    public async Task<long> PrepareDocument(Dto.PrepareDocumentRequest request, CancellationToken cancellationToken = default)
    {
        if (request.CurrentUserInfo is null
            || request.CreatorInfo is null
            || request.DocumentData is null
            || request.ClientData is null)
        {
            throw new ArgumentNullException(nameof(request), "One of required objects has not been set");
        }

        var formTypeId = (await _codebookService.DocumentTemplateVersions(cancellationToken))
            .First(t => t.Id == request.DocumentData.DocumentTemplateVersionId)
            .FormTypeId;

        var formType = (await _codebookService.FormTypes(cancellationToken))
            .FirstOrDefault(t => t.Id == formTypeId) 
            ?? throw new CisExtServiceValidationException("DocumentTemplateVersionId does not exist in FormTypes codebook");

        var docType = (await _codebookService.DocumentTypes(cancellationToken))
            .FirstOrDefault(t => t.Id == request.DocumentData.DocumentTypeId)
            ?? throw new CisExtServiceValidationException("DocumentTypeId does not exist in DocumentTypes codebook");

        var svcRequest = new Contracts.PrepareDocumentRequest2
        {
            CurrentUserInfo = new()
            {
                CPM = request.CurrentUserInfo.Cpm,
                ICP = request.CurrentUserInfo.Icp,
                Name = request.CurrentUserInfo.FullName
            },
            CreatorInfo = new()
            {
                CPM = request.CreatorInfo.Cpm,
                ICP = request.CreatorInfo.Icp,
                Name = request.CreatorInfo.FullName
            },
            DocumentData = new()
            {
                TypeCode = docType.ShortName,
                TemplateVersion = formType.Version,
                Name = request.DocumentData.FileName,
                FormId = request.DocumentData.FormId,
                EaCodeMain = docType.EACodeMainId.ToString(),
                ContractNumber = request.DocumentData.ContractNumber
            },
            ClientData = new()
            {
                UniversalId = request.ClientData.Identities!.FirstOrDefault(t => t.Scheme == CIS.Foms.Enums.IdentitySchemes.Kb)?.Id.ToString(),
                ExternalId = request.ClientData.Identities!.FirstOrDefault(t => t.Scheme == CIS.Foms.Enums.IdentitySchemes.Mp)?.Id.ToString() ?? "",
                Name = request.ClientData.FullName!,
                BirthNumber_RegNumber = request.ClientData.BirthNumber,
                PhoneNumber = request.ClientData.Phone,
                EmailAddress = request.ClientData.Email,
                Signatures = new List<Contracts.SignatureData>
                {
                    new()
                    {
                        Code = $"X_SIG_{request.ClientData.CodeIndex}",
                        Info1 = request.ClientData.FullName,
                        Info2 = request.ClientData.Phone,
                        Info3 = request.ClientData.Email
                    }
                }
            }
        };

        if (request.OtherClients is not null)
        {
            svcRequest.OtherClients = request.OtherClients.Select(t => new Contracts.SigneeInfo2
            {
                UniversalId = request.ClientData.Identities!.FirstOrDefault(t => t.Scheme == CIS.Foms.Enums.IdentitySchemes.Kb)?.Id.ToString(),
                EmailAddress = t.Email,
                PhoneNumber = t.Phone,
                Signatures = new List<Contracts.SignatureData>
                {
                    new()
                    {
                        Code = $"X_SIG_{request.ClientData.CodeIndex}",
                        Info1 = t.FullName,
                        Info2 = t.Phone,
                        Info3 = t.Email
                    }
                }
            }).ToList();
        }

        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}/REST/v2/DocumentService/PrepareDocument", svcRequest, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<Contracts.UploadReference>(cancellationToken: cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(PrepareDocument), nameof(Contracts.UploadReference));

        if ((result.Result?.Code ?? 0) != 0)
        {
            throw new CisExtServiceValidationException((result.Result?.Code ?? 0), result.Result?.Message ?? "Unknown error");
        }
        else if (!result.ReferenceId.HasValue)
        {
            throw new CisExtServiceValidationException("ReferenceId not found in response");
        }

        return result.ReferenceId!.Value;
    }

    public async Task<(string ExternalId, string? TargetUrl)> UploadDocument(
        long referenceId, 
        string filename,
        DateTime creationDate,
        byte[] fileData, 
        CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();

        var contentFile = new ByteArrayContent(fileData);
        contentFile.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/pdf");
        content.Add(contentFile, "file", filename);
        
        var response = await _httpClient
            .PostAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}/REST/v2/DocumentService/UploadDocument?referenceId={referenceId}&filename={Uri.EscapeDataString(filename)}&creationDate={creationDate.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}", content, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<Contracts.ResponseUrl2>(cancellationToken: cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(UploadDocument), nameof(Contracts.ResponseUrl2));

        if ((result.Result?.Code ?? 0) != 0)
        {
            throw new CisExtServiceValidationException((result.Result?.Code ?? 0), result.Result?.Message ?? "Unknown error");
        }

        return (result.ExternalId!, result.TargetUrl);
    }

    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly HttpClient _httpClient;

    public RealESignaturesClient(HttpClient httpClient, DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
        _httpClient = httpClient;
    }
}