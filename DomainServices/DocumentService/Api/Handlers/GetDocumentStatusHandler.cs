using DomainServices.DocumentService.Api.Dto;
using DomainServices.DocumentService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.DocumentService.Api.Handlers;

internal class GetDocumentStatusHandler : IRequestHandler<GetDocumentStatusMediatrRequest, GetDocumentStatusResponse>
{
    #region Construction

    private readonly ILogger<GetDocumentStatusHandler> _logger;
    private readonly ESignatures.IESignaturesClient _eSignatures;

    public GetDocumentStatusHandler(ILogger<GetDocumentStatusHandler> logger, ESignatures.IESignaturesClient eSignatures)
    {
        _logger = logger;
        _eSignatures = eSignatures;
    }

    #endregion

    public async Task<GetDocumentStatusResponse> Handle(GetDocumentStatusMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get document status [DocumentId: {id}, Mandant: {mandant}]", request.DocumentId, request.Mandant);

        //if (request.Mandant != CIS.Core.IdentitySchemes.Kb)
        //{
        //    throw new NotSupportedException($"Mandant '{request.Mandant}' is not supported");
        //}

        var response = (await _eSignatures.GetDocumentStatus(request.DocumentId, request.Mandant)).ToESignaturesResult<ESignatures.ESignaturesWrapper.ResponseStatus>();

        var model = new GetDocumentStatusResponse
        {
            DocumentId = request.DocumentId,
            DocumentStatus = response.Status
        };

        return model;
    }
}