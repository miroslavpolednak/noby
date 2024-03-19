using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.UserService.Clients;

namespace DomainServices.SalesArrangementService.Api.Endpoints.CreateSalesArrangement;

internal sealed class CreateSalesArrangementHandler
    : IRequestHandler<CreateSalesArrangementRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(CreateSalesArrangementRequest request, CancellationToken cancellation)
    {
        // validace na existenci case
        //TODO je nejaka spojitost mezi ProductTypeId a SalesArrangementTypeId, ktera by se dala zkontrolovat?
        await _caseService.ValidateCaseId(request.CaseId, true, cancellation);

        var user = await _userService.GetCurrentUser(cancellation);

        // vytvorit entitu
        var saEntity = new Database.Entities.SalesArrangement
        {
            CaseId = request.CaseId,
            SalesArrangementTypeId = request.SalesArrangementTypeId,
            StateUpdateTime = _timeProvider.GetLocalNow().DateTime,
            ContractNumber = request.ContractNumber,
            ChannelId = user.UserInfo.ChannelId,
            PcpId = request.PcpId,
            TaskProcessId = request.TaskProcessId
        };

        // get default SA state
        saEntity.State = request.State ?? (await _codebookService.SalesArrangementStates(cancellation)).First(t => t.IsDefault).Id;

        // ulozit do DB
        _dbContext.SalesArrangements.Add(saEntity);
        await _dbContext.SaveChangesAsync(cancellation);
        _bag.Add(CreateSalesArrangementRollback.BagKeySalesArrangementId, saEntity.SalesArrangementId);

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
                case CreateSalesArrangementRequest.DataOneofCase.Retention:
                    data.Retention = request.Retention;
                    break;
                case CreateSalesArrangementRequest.DataOneofCase.Refixation:
                    data.Refixation = request.Refixation;
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
            SalesArrangementTypes.Retention when dataCase == CreateSalesArrangementRequest.DataOneofCase.Retention => true,
            SalesArrangementTypes.Refixation when dataCase == CreateSalesArrangementRequest.DataOneofCase.Refixation => true,
            _ => throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.DataObjectIsNotValid, salesArrangementTypeId)
        };

    private readonly TimeProvider _timeProvider;
    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly OfferService.Clients.IOfferServiceClient _offerService;
    private readonly CaseService.Clients.v1.ICaseServiceClient _caseService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<CreateSalesArrangementHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IUserServiceClient _userService;
    private readonly IRollbackBag _bag;

    public CreateSalesArrangementHandler(
        IRollbackBag bag,
        IUserServiceClient userService,
        IMediator mediator,
        OfferService.Clients.IOfferServiceClient offerService,
        CaseService.Clients.v1.ICaseServiceClient caseService,
        CodebookService.Clients.ICodebookServiceClient codebookService,
        Database.SalesArrangementServiceDbContext dbContext,
        ILogger<CreateSalesArrangementHandler> logger,
        TimeProvider timeProvider)
    {
        _bag = bag;
        _userService = userService;
        _mediator = mediator;
        _offerService = offerService;
        _caseService = caseService;
        _codebookService = codebookService;
        _dbContext = dbContext;
        _logger = logger;
        _timeProvider = timeProvider;
    }
}
