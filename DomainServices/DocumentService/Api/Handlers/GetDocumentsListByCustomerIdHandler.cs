using DomainServices.DocumentService.Api.Dto;
using DomainServices.DocumentService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.DocumentService.Api.Handlers;

internal class GetDocumentsListByCustomerIdHandler : IRequestHandler<GetDocumentsListByCustomerIdMediatrRequest, GetDocumentsListResponse>
{
    #region Construction

    private readonly ILogger<GetDocumentsListByCustomerIdHandler> _logger;

    public GetDocumentsListByCustomerIdHandler(
        ILogger<GetDocumentsListByCustomerIdHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    public async Task<GetDocumentsListResponse> Handle(GetDocumentsListByCustomerIdMediatrRequest request, CancellationToken cancellationToken)
    {
        //TODO: request params?

        _logger.LogInformation("Get documents list by customer id [CustomerId: {id}]", request.CustomerId);

        var model = new GetDocumentsListResponse
        {
        };

        model.Documents.Add(new Document() { DocumentId = "11" } );

        return model;
    }
}