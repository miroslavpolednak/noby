using DomainServices.DocumentService.Api.Dto;
using DomainServices.DocumentService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.DocumentService.Api.Handlers;

internal class GetDocumentsListByRelationIdHandler : IRequestHandler<GetDocumentsListByRelationIdMediatrRequest, GetDocumentsListResponse>
{
    #region Construction

    private readonly ILogger<GetDocumentsListByRelationIdHandler> _logger;

    public GetDocumentsListByRelationIdHandler(
        ILogger<GetDocumentsListByRelationIdHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetDocumentsListResponse> Handle(GetDocumentsListByRelationIdMediatrRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get documents list by relation id [RelationId: {id}, Mandant: {mandant}]", request.RelationId, request.Mandant);

        if (request.Mandant != CIS.Core.IdentitySchemes.Kb)
        {
            throw new NotSupportedException($"Mandant '{request.Mandant}' is not supported");
        }

        var model = new GetDocumentsListResponse
        {
        };

        model.Documents.Add(new Document() { DocumentId = "11" } );

        return model;
    }
}