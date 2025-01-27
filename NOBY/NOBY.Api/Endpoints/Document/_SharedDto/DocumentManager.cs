﻿using System.Globalization;
using System.Runtime.InteropServices;
using CIS.Core.Attributes;
using CIS.Core.Security;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.Document.SharedDto;

[TransientService, SelfService]
public class DocumentManager
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICurrentUserAccessor _userAccessor;

    public DocumentManager(ICodebookServiceClient codebookService, ICurrentUserAccessor userAccessor)
    {
        _codebookService = codebookService;
        _userAccessor = userAccessor;
    }

    public int UserId => _userAccessor.User!.Id;

    internal InputParameters GetOfferInput(int offerId) =>
        new()
        {
            OfferId = offerId,
            UserId = UserId
        };

    internal InputParameters GetSalesArrangementInput(int salesArrangementId, int? customerOnSaId = default) =>
        new()
        {
            SalesArrangementId = salesArrangementId,
            UserId = UserId,
            CustomerOnSaId = customerOnSaId
        };
    
    internal static byte[] GetByteArray(ReadOnlyMemory<byte> memory)
    {
        if (!MemoryMarshal.TryGetArray(memory, out var arraySegment))
            throw new InvalidOperationException("Failed to get memory of document buffer");

        return arraySegment.Array!;
    }

    internal async Task<string> GetFileName(GetDocumentBaseRequest baseRequest, CancellationToken cancellationToken)
    {
        var nameParts = new[]
        {
            await GetTemplateFileName(),
            baseRequest.InputParameters.CaseId?.ToString(CultureInfo.InvariantCulture),
            DateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)
        };

        return string.Join("_", nameParts.Where(str => !string.IsNullOrWhiteSpace(str))) + ".pdf";

        async Task<string> GetTemplateFileName()
        {
            var templates = await _codebookService.DocumentTypes(cancellationToken);

            return templates.First(t => t.Id == (int)baseRequest.DocumentType).FileName;
        }
    }
}