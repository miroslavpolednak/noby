using DomainServices.DocumentService.Api.Dto;
using DomainServices.DocumentService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.DocumentService.Api.Handlers;

internal class GetDocumentHandler : IRequestHandler<GetDocumentMediatrRequest, GetDocumentResponse>
{
    #region Construction

    private readonly ILogger<GetDocumentHandler> _logger;

    public GetDocumentHandler(
        ILogger<GetDocumentHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetDocumentResponse> Handle(GetDocumentMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get document [DocumentId: {id}, Mandant: {mandant}]", request.DocumentId, request.Mandant);

        if (request.Mandant != CIS.Core.Enums.IdentitySchemes.Kb)
        {
            throw new NotSupportedException($"Mandant '{request.Mandant}' is not supported");
        }

        var model = new GetDocumentResponse
        {
            DocumentId = request.DocumentId,
            DocumentData = null,
            //DocumentOrigin =  null,
            MimeType = String.Empty,
            Filename = null
        };

        return model;
    }

   
}