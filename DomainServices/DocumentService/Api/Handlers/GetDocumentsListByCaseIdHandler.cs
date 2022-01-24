using DomainServices.DocumentService.Api.Dto;
using DomainServices.DocumentService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.DocumentService.Api.Handlers;

internal class GetDocumentsListByCaseIdHandler : IRequestHandler<GetDocumentsListByCaseIdMediatrRequest, GetDocumentsListByCaseIdResponse>
{
    #region Construction

    private readonly ILogger<GetDocumentsListByCaseIdHandler> _logger;

    public GetDocumentsListByCaseIdHandler(
        ILogger<GetDocumentsListByCaseIdHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetDocumentsListByCaseIdResponse> Handle(GetDocumentsListByCaseIdMediatrRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get documents list by case id [CaseId: {id}]", request.CaseId);

        var model = new GetDocumentsListByCaseIdResponse
        {
        };

        model.Documents.Add(new Document() { DocumentId = "11" } );

        return model;
    }
}