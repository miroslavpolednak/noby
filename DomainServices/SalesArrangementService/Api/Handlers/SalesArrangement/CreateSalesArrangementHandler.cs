﻿using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class CreateSalesArrangementHandler
    : IRequestHandler<Dto.CreateSalesArrangementMediatrRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(Dto.CreateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        // validace product instance type
        _ = (await _codebookService.SalesArrangementTypes(cancellation)).FirstOrDefault(t => t.Id == request.Request.SalesArrangementTypeId)
            ?? throw new CisNotFoundException(18005, $"SalesArrangementTypeId {request.Request.SalesArrangementTypeId} does not exist.");

        // validace na existenci case
        //TODO je nejaka spojitost mezi ProductTypeId a SalesArrangementTypeId, ktera by se dala zkontrolovat?
        _ = ServiceCallResult.ResolveToDefault<CaseService.Contracts.Case>(await _caseService.GetCaseDetail(request.Request.CaseId, cancellation))
            ?? throw new CisNotFoundException(18002, $"Case ID #{request.Request.CaseId} does not exist.");

        // vytvorit entitu
        var saEntity = new Repositories.Entities.SalesArrangement
        {
            SalesArrangementSignatureTypeId = request.Request.SalesArrangementSignatureTypeId,
            CaseId = request.Request.CaseId,
            SalesArrangementTypeId = request.Request.SalesArrangementTypeId,
            StateUpdateTime = _dateTime.Now,
            ContractNumber = request.Request.ContractNumber,
            ChannelId = 4 //TODO jak ziskat ChannelId? Z instance uzivatele? Az bude pripravena xxvvss...
        };

        // get default SA state
        saEntity.State = (await _codebookService.SalesArrangementStates(cancellation)).First(t => t.IsDefault).Id;

        // ulozit do DB
        var salesArrangementId = await _repository.CreateSalesArrangement(saEntity, cancellation);

        // nalinkovani offer
        if (request.Request.OfferId.HasValue)
        {
            await _mediator.Send(new Dto.LinkModelationToSalesArrangementMediatrRequest(new()
            {
                SalesArrangementId = salesArrangementId,
                OfferId = request.Request.OfferId.Value
            }), cancellation);
        }

        // params
        if (request.Request.DataCase != CreateSalesArrangementRequest.DataOneofCase.None)
        {
            var data = new UpdateSalesArrangementParametersRequest()
            {
                SalesArrangementId = salesArrangementId
            };
            if (request.Request.DataCase == CreateSalesArrangementRequest.DataOneofCase.Mortgage)
                data.Mortgage = request.Request.Mortgage;
            if (request.Request.DataCase == CreateSalesArrangementRequest.DataOneofCase.Drawing)
                data.Drawing = request.Request.Drawing;

            await _mediator.Send(new Dto.UpdateSalesArrangementParametersMediatrRequest(data), cancellation);
        }
        
        _logger.EntityCreated(nameof(Repositories.Entities.SalesArrangement), salesArrangementId);

        return new CreateSalesArrangementResponse { SalesArrangementId = salesArrangementId };
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<CreateSalesArrangementHandler> _logger;
    private readonly CIS.Core.IDateTime _dateTime;
    private readonly IMediator _mediator;
    
    public CreateSalesArrangementHandler(
        IMediator mediator,
        CIS.Core.IDateTime dateTime,
        OfferService.Abstraction.IOfferServiceAbstraction offerService,
        CaseService.Abstraction.ICaseServiceAbstraction caseService,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<CreateSalesArrangementHandler> logger)
    {
        _mediator = mediator;
        _dateTime = dateTime;
        _offerService = offerService;
        _caseService = caseService;
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}
