using CIS.Foms.Types.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using FastEnumUtility;
using System.Globalization;
using System.Net.Http.Headers;

namespace ExternalServices.ESignatures.V1;

internal sealed class RealESignaturesClient
    : IESignaturesClient
{
    public async Task DeleteDocument(string externalId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}{_urlPrefix}DeleteDocument?externalId={externalId}", cancellationToken)
            .ConfigureAwait(false);

        var result = await response.EnsureSuccessStatusAndReadJson<Contracts.ProcessingResult>(StartupExtensions.ServiceName, cancellationToken);

        if ((result.Code ?? 0) != 0)
        {
            throw new CisExtServiceValidationException((result.Code ?? 0), result.Message ?? "Unknown error");
        }
    }

    public async Task<EDocumentStatuses> GetDocumentStatus(string externalId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}{_urlPrefix}GetDocumentStatus?id={externalId}", cancellationToken)
            .ConfigureAwait(false);

        var result = await response.EnsureSuccessStatusAndReadJson<Contracts.ResponseStatus>(StartupExtensions.ServiceName, cancellationToken);

        if ((result.Result?.Code ?? 0) != 0)
        {
            throw new CisExtServiceValidationException((result.Result?.Code ?? 0), result.Result?.Message ?? "Unknown error");
        }
        else if (FastEnum.TryParse<EDocumentStatuses>(result.Status!, true, out EDocumentStatuses status))
        {
            return status;
        }
        else
        {
            throw new CisExtServiceValidationException(50001, $"Returned status '{result.Status}' is unknown");
        }
    }

    public async Task<byte[]> DownloadDocumentPreview(string externalId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}{_urlPrefix}DownloadDocumentPreview?externalId={externalId}", cancellationToken)
            .ConfigureAwait(false);

        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);

        if (response.Content == null)
        {
            throw new CisExtServiceValidationException(50005, "Response does not contain binary data");
        }

        using (MemoryStream ms = new MemoryStream())
        {
            await response.Content.CopyToAsync(ms, cancellationToken);
            return ms.ToArray();
        }
    }

    public async Task SubmitDispatchForm(bool documentsValid, List<Dto.DispatchFormClientDocument> documents, CancellationToken cancellationToken = default)
    {
        var svcRequest = new Contracts.SubmitDispatchFormRequest
        {
            DocumentsValid = documentsValid,
            Documents = documents.Select(t => new Contracts.DispatchFormDocument
            {
                AttachmentsComplete = t.AttachmentsComplete,
                ExternalId = t.ExternalId,
                IsCancelled = t.IsCancelled,
                NotCompletedReason = t.NotCompletedReason
            }).ToList()
        };

        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}{_urlPrefix}SubmitDispatchForm", svcRequest, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.EnsureSuccessStatusAndReadJson<Contracts.ProcessingResult>(StartupExtensions.ServiceName, cancellationToken);

        if ((result.Code ?? 0) != 0)
        {
            throw new CisExtServiceValidationException((result.Code ?? 0), result.Message ?? "Unknown error");
        }
    }

    public async Task<(int? Code, string? Message)> SendDocumentPreview(string externalId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}{_urlPrefix}SendDocumentPreview?externalId={externalId}", null, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.EnsureSuccessStatusAndReadJson<Contracts.ProcessingResult>(StartupExtensions.ServiceName, cancellationToken: cancellationToken);

        if ((result.Code ?? 0) != 0)
        {
            throw new CisExtServiceValidationException((result.Code ?? 0), result.Message ?? "Unknown error");
        }

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
            ?? throw new CisExtServiceValidationException(50002, "DocumentTemplateVersionId does not exist in FormTypes codebook");

        var docType = (await _codebookService.DocumentTypes(cancellationToken))
            .FirstOrDefault(t => t.Id == request.DocumentData.DocumentTypeId)
            ?? throw new CisExtServiceValidationException(50003, "DocumentTypeId does not exist in DocumentTypes codebook");

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
                ContractNumber = request.DocumentData.ContractNumber
            },
            ClientData = new()
            {
                UniversalId = request.ClientData.Identities!.FirstOrDefault(t => t.Scheme == CIS.Foms.Enums.IdentitySchemes.Kb)?.Id.ToString(CultureInfo.InvariantCulture),
                ExternalId = request.ClientData.Identities!.FirstOrDefault(t => t.Scheme == CIS.Foms.Enums.IdentitySchemes.Mp)?.Id.ToString(CultureInfo.InvariantCulture) ?? "",
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
                UniversalId = request.ClientData.Identities!.FirstOrDefault(t => t.Scheme == CIS.Foms.Enums.IdentitySchemes.Kb)?.Id.ToString(CultureInfo.InvariantCulture),
                EmailAddress = t.Email,
                PhoneNumber = t.Phone,
                Signatures = new List<Contracts.SignatureData>
                {
                    new()
                    {
                        Code = $"X_SIG_{t.CodeIndex}",
                        Info1 = t.FullName,
                        Info2 = t.Phone,
                        Info3 = t.Email
                    }
                }
            }).ToList();
        }

        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}{_urlPrefix}PrepareDocument", svcRequest, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<Contracts.UploadReference>(cancellationToken: cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(0, StartupExtensions.ServiceName, nameof(PrepareDocument), nameof(Contracts.UploadReference));

        if ((result.Result?.Code ?? 0) != 0)
        {
            throw new CisExtServiceValidationException((result.Result?.Code ?? 0), result.Result?.Message ?? "Unknown error");
        }
        else if (!result.ReferenceId.HasValue)
        {
            throw new CisExtServiceValidationException(50004, "ReferenceId not found in response");
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
        using var content = new ByteArrayContent(fileData);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        var response = await _httpClient
            .PostAsync(_httpClient.BaseAddress + $"/{StartupExtensions.TenantCode}{_urlPrefix}UploadDocument?referenceId={referenceId}&filename={Uri.EscapeDataString(filename)}&creationDate={creationDate.ToString("s", CultureInfo.InvariantCulture)}", content, cancellationToken)
            .ConfigureAwait(false);
        
        var result = await response.EnsureSuccessStatusAndReadJson<Contracts.ResponseUrl2>(StartupExtensions.ServiceName, cancellationToken);

        if ((result.Result?.Code ?? 0) != 0)
        {
            throw new CisExtServiceValidationException((result.Result?.Code ?? 0), result.Result?.Message ?? "Unknown error");
        }

        return (result.ExternalId!, result.TargetUrl);
    }

    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly HttpClient _httpClient;
    private const string _urlPrefix = "/REST/v2/DocumentService/";

    public RealESignaturesClient(HttpClient httpClient, DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
        _httpClient = httpClient;
    }
}