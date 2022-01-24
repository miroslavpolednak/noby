using DomainServices.DocumentService.Api.Dto;
using DomainServices.DocumentService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.DocumentService.Api.Handlers;

internal class GetDocumentsListByContractNumberHandler : IRequestHandler<GetDocumentsListByContractNumberMediatrRequest, GetDocumentsListResponse>
{
    #region Construction

    private readonly ILogger<GetDocumentsListByContractNumberHandler> _logger;

    public GetDocumentsListByContractNumberHandler(
        ILogger<GetDocumentsListByContractNumberHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetDocumentsListResponse> Handle(GetDocumentsListByContractNumberMediatrRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get documents list by contract number [ContractNumber: {id}, Mandant: {mandant}]", request.ContractNumber, request.Mandant);

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