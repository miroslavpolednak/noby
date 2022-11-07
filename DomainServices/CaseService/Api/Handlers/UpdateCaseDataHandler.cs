﻿namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseDataHandler
    : IRequestHandler<Dto.UpdateCaseDataMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseDataMediatrRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        await _repository.EnsureExistingCase(request.Request.CaseId, cancellation);

        // zkontrolovat ProdInstType
        if (!(await _codebookService.ProductTypes(cancellation)).Any(t => t.Id == request.Request.Data.ProductTypeId))
            throw new CisNotFoundException(13014, nameof(request.Request.Data.ProductTypeId), request.Request.Data.ProductTypeId);

        // ulozit do DB
        await _repository.UpdateCaseData(request.Request.CaseId, request.Request.Data, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
    
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly Repositories.CaseServiceRepository _repository;
    
    public UpdateCaseDataHandler(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Repositories.CaseServiceRepository repository)
    {
        _codebookService = codebookService;
        _repository = repository;
    }
}
