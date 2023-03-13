using CIS.Foms.Types.Enums;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.CreateSalesArrangement;

internal sealed class CreateSalesArrangementHandler
    : IRequestHandler<CreateSalesArrangementRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(CreateSalesArrangementRequest request, CancellationToken cancellation)
    {
        // validace product instance type
        _ = (await _codebookService.SalesArrangementTypes(cancellation)).FirstOrDefault(t => t.Id == request.SalesArrangementTypeId)
            ?? throw new CisNotFoundException(18005, $"SalesArrangementTypeId {request.SalesArrangementTypeId} does not exist.");

        // validace na existenci case
        //TODO je nejaka spojitost mezi ProductTypeId a SalesArrangementTypeId, ktera by se dala zkontrolovat?
        await _caseService.GetCaseDetail(request.CaseId, cancellation);

        // vytvorit entitu
        var saEntity = new Database.Entities.SalesArrangement
        {
            SalesArrangementSignatureTypeId = request.SalesArrangementSignatureTypeId,
            CaseId = request.CaseId,
            SalesArrangementTypeId = request.SalesArrangementTypeId,
            StateUpdateTime = _dateTime.Now,
            ContractNumber = request.ContractNumber,
            ChannelId = 4 //TODO jak ziskat ChannelId? Z instance uzivatele? Az bude pripravena xxvvss asi...
        };

        // get default SA state
        saEntity.State = (await _codebookService.SalesArrangementStates(cancellation)).First(t => t.IsDefault).Id;

        // ulozit do DB
        var salesArrangementId = await _repository.CreateSalesArrangement(saEntity, cancellation);

        // params
        if (request.DataCase != CreateSalesArrangementRequest.DataOneofCase.None)
        {
            // validace
            validateDataCase(request.DataCase, (SalesArrangementTypes)request.SalesArrangementTypeId);

            var data = new UpdateSalesArrangementParametersRequest()
            {
                SalesArrangementId = salesArrangementId
            };
            switch (request.DataCase)
            {
                case CreateSalesArrangementRequest.DataOneofCase.Mortgage:
                    data.Mortgage = request.Mortgage;
                    break;
                case CreateSalesArrangementRequest.DataOneofCase.Drawing:
                    data.Drawing = request.Drawing;
                    break;
                case CreateSalesArrangementRequest.DataOneofCase.GeneralChange:
                    data.GeneralChange = request.GeneralChange;
                    break;
                case CreateSalesArrangementRequest.DataOneofCase.HUBN:
                    data.HUBN = request.HUBN;
                    break;
                case CreateSalesArrangementRequest.DataOneofCase.CustomerChange:
                    data.CustomerChange = request.CustomerChange;
                    break;
            }
            var updateMediatrRequest = new UpdateSalesArrangementParametersRequest(data);

            await _mediator.Send(updateMediatrRequest, cancellation);
        }

        // nalinkovani offer
        if (request.OfferId.HasValue)
        {
            await _mediator.Send(new LinkModelationToSalesArrangementRequest(new()
            {
                SalesArrangementId = salesArrangementId,
                OfferId = request.OfferId.Value
            }), cancellation);
        }

        _logger.EntityCreated(nameof(Database.Entities.SalesArrangement), salesArrangementId);

        return new CreateSalesArrangementResponse { SalesArrangementId = salesArrangementId };
    }

    static bool validateDataCase(CreateSalesArrangementRequest.DataOneofCase dataCase, SalesArrangementTypes salesArrangementTypeId)
        => salesArrangementTypeId switch
        {
            SalesArrangementTypes.Mortgage when dataCase == CreateSalesArrangementRequest.DataOneofCase.Mortgage => true,
            SalesArrangementTypes.Drawing when dataCase == CreateSalesArrangementRequest.DataOneofCase.Drawing => true,
            SalesArrangementTypes.GeneralChange when dataCase == CreateSalesArrangementRequest.DataOneofCase.GeneralChange => true,
            SalesArrangementTypes.HUBN when dataCase == CreateSalesArrangementRequest.DataOneofCase.HUBN => true,
            SalesArrangementTypes.CustomerChange when dataCase == CreateSalesArrangementRequest.DataOneofCase.CustomerChange => true,
            _ => throw new CisValidationException(0, $"CreateSalesArrangementRequest.DataOneofCase is not valid for SalesArrangementTypeId={salesArrangementTypeId}")
        };

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly OfferService.Clients.IOfferServiceClient _offerService;
    private readonly CaseService.Clients.ICaseServiceClient _caseService;
    private readonly Database.SalesArrangementServiceRepository _repository;
    private readonly ILogger<CreateSalesArrangementHandler> _logger;
    private readonly CIS.Core.IDateTime _dateTime;
    private readonly IMediator _mediator;

    public CreateSalesArrangementHandler(
        IMediator mediator,
        CIS.Core.IDateTime dateTime,
        OfferService.Clients.IOfferServiceClient offerService,
        CaseService.Clients.ICaseServiceClient caseService,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Database.SalesArrangementServiceRepository repository,
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
