using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.CreateSalesArrangement;

internal sealed class CreateSalesArrangementHandler
    : IRequestHandler<CreateSalesArrangementRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(CreateSalesArrangementRequest request, CancellationToken cancellation)
    {
        // validace na existenci case
        //TODO je nejaka spojitost mezi ProductTypeId a SalesArrangementTypeId, ktera by se dala zkontrolovat?
        await _caseService.GetCaseDetail(request.CaseId, cancellation);

        // vytvorit entitu
        var saEntity = new Database.Entities.SalesArrangement
        {
            SalesArrangementSignatureTypeId = request.SalesArrangementSignatureTypeId,
            CaseId = request.CaseId,
            SalesArrangementTypeId = request.SalesArrangementTypeId,
            StateUpdateTime = _dbContext.CisDateTime.Now,
            ContractNumber = request.ContractNumber,
            ChannelId = 4 //TODO jak ziskat ChannelId? Z instance uzivatele? Az bude pripravena xxvvss asi...
        };

        // get default SA state
        saEntity.State = (await _codebookService.SalesArrangementStates(cancellation)).First(t => t.IsDefault).Id;

        // ulozit do DB
        _dbContext.SalesArrangements.Add(saEntity);
        await _dbContext.SaveChangesAsync(cancellation);

        // params
        if (request.DataCase != CreateSalesArrangementRequest.DataOneofCase.None)
        {
            // validace
            validateDataCase(request.DataCase, (SalesArrangementTypes)request.SalesArrangementTypeId);

            var data = new UpdateSalesArrangementParametersRequest()
            {
                SalesArrangementId = saEntity.SalesArrangementId
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
                case CreateSalesArrangementRequest.DataOneofCase.CustomerChange3602A:
                    data.CustomerChange3602A = request.CustomerChange3602A;
                    break;
                case CreateSalesArrangementRequest.DataOneofCase.CustomerChange3602B:
                    data.CustomerChange3602B = request.CustomerChange3602B;
                    break;
                case CreateSalesArrangementRequest.DataOneofCase.CustomerChange3602C:
                    data.CustomerChange3602C = request.CustomerChange3602C;
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
                SalesArrangementId = saEntity.SalesArrangementId,
                OfferId = request.OfferId.Value
            }), cancellation);
        }

        _logger.EntityCreated(nameof(Database.Entities.SalesArrangement), saEntity.SalesArrangementId);

        return new CreateSalesArrangementResponse { SalesArrangementId = saEntity.SalesArrangementId };
    }

    static bool validateDataCase(CreateSalesArrangementRequest.DataOneofCase dataCase, SalesArrangementTypes salesArrangementTypeId)
        => salesArrangementTypeId switch
        {
            SalesArrangementTypes.Mortgage when dataCase == CreateSalesArrangementRequest.DataOneofCase.Mortgage => true,
            SalesArrangementTypes.Drawing when dataCase == CreateSalesArrangementRequest.DataOneofCase.Drawing => true,
            SalesArrangementTypes.GeneralChange when dataCase == CreateSalesArrangementRequest.DataOneofCase.GeneralChange => true,
            SalesArrangementTypes.HUBN when dataCase == CreateSalesArrangementRequest.DataOneofCase.HUBN => true,
            SalesArrangementTypes.CustomerChange when dataCase == CreateSalesArrangementRequest.DataOneofCase.CustomerChange => true,
            SalesArrangementTypes.CustomerChange3602A when dataCase == CreateSalesArrangementRequest.DataOneofCase.CustomerChange3602A => true,
            SalesArrangementTypes.CustomerChange3602B when dataCase == CreateSalesArrangementRequest.DataOneofCase.CustomerChange3602B => true,
            SalesArrangementTypes.CustomerChange3602C when dataCase == CreateSalesArrangementRequest.DataOneofCase.CustomerChange3602C => true,
            _ => throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.DataObjectIsNotValid, salesArrangementTypeId)
        };

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly OfferService.Clients.IOfferServiceClient _offerService;
    private readonly CaseService.Clients.ICaseServiceClient _caseService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<CreateSalesArrangementHandler> _logger;
    private readonly IMediator _mediator;

    public CreateSalesArrangementHandler(
        IMediator mediator,
        OfferService.Clients.IOfferServiceClient offerService,
        CaseService.Clients.ICaseServiceClient caseService,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Database.SalesArrangementServiceDbContext dbContext,
        ILogger<CreateSalesArrangementHandler> logger)
    {
        _mediator = mediator;
        _offerService = offerService;
        _caseService = caseService;
        _codebookService = codebookService;
        _dbContext = dbContext;
        _logger = logger;
    }
}
