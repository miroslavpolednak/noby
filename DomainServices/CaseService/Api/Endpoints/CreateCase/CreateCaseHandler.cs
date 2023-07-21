using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using ExternalServices.Eas.V1;

namespace DomainServices.CaseService.Api.Endpoints.CreateCase;

internal sealed class CreateCaseHandler
    : IRequestHandler<CreateCaseRequest, CreateCaseResponse>
{
    public async Task<CreateCaseResponse> Handle(CreateCaseRequest request, CancellationToken cancellation)
    {
        // overit existenci ownera
        var userInstance = await _userService.GetUser(request.CaseOwnerUserId, cancellation);
        //TODO zkontrolovat existenci vlastnika?

        // get default case state
        int defaultCaseState = (await _codebookService.CaseStates(cancellation)).First(t => t.IsDefault.GetValueOrDefault()).Id;

        // ziskat caseId
        long newCaseId = await _easClient.GetCaseId(CIS.Foms.Enums.IdentitySchemes.Kb, request.Data.ProductTypeId, cancellation);
        _logger.NewCaseIdCreated(newCaseId);

        // vytvorit entitu
        var entity = createDatabaseEntity(request, newCaseId);
        entity.OwnerUserName = userInstance.UserInfo.DisplayName;//dotazene jmeno majitele caseu (poradce)
        entity.State = defaultCaseState;//vychozi status

        try
        {
            _dbContext.Cases.Add(entity);
            await _dbContext.SaveChangesAsync(cancellation);
            _bag.Add(CreateCaseRollback.BagKeyCaseId, entity.CaseId);

            _logger.EntityCreated(nameof(Database.Entities.Case), newCaseId);
        }
        catch (DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException && ((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2627)
        {
            throw ErrorCodeMapper.CreateAlreadyExistsException(ErrorCodeMapper.CaseAlreadyExist, newCaseId);
        }

        // notify SB about state change
        // schvalne bez await, aby to spoklo pripadnou exception
        _mediator.Send(new NotifyStarbuildRequest
        {
            CaseId = newCaseId,
            SkipRiskBusinessCaseId = true
        }, cancellation);
        
        return new CreateCaseResponse()
        {
            CaseId = newCaseId
        };
    }

    private Database.Entities.Case createDatabaseEntity(CreateCaseRequest request, long caseId)
    {
        var entity = new Database.Entities.Case
        {
            CaseId = caseId,

            StateUpdatedInStarbuild = (int)UpdatedInStarbuildStates.Unknown,
            StateUpdateTime = _dbContext.CisDateTime.Now,
            ProductTypeId = request.Data.ProductTypeId,

            Name = request.Customer.Name,
            FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson,
            DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson,
            Cin = request.Customer.Cin,

            TargetAmount = request.Data.TargetAmount,
            IsEmployeeBonusRequested = request.Data.IsEmployeeBonusRequested,
            ContractNumber = request.Data.ContractNumber,

            OwnerUserId = request.CaseOwnerUserId,
        };

        // pokud je zadany customer
        if (request.Customer is not null)
        {
            entity.CustomerIdentityScheme = (CIS.Foms.Enums.IdentitySchemes)Convert.ToInt32(request.Customer?.Identity?.IdentityScheme, CultureInfo.InvariantCulture);
            entity.CustomerIdentityId = request.Customer?.Identity?.IdentityId;
        }

        entity.EmailForOffer = request.OfferContacts?.EmailForOffer;
        entity.PhoneIDCForOffer = request.OfferContacts?.PhoneNumberForOffer?.PhoneIDC;
        entity.PhoneNumberForOffer = request.OfferContacts?.PhoneNumberForOffer?.PhoneNumber;

        return entity;
    }

    private readonly IRollbackBag _bag;
    private readonly IMediator _mediator;
    private readonly CaseServiceDbContext _dbContext;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly IEasClient _easClient;
    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly UserService.Clients.IUserServiceClient _userService;

    public CreateCaseHandler(
        IRollbackBag bag,
        IMediator mediator,
        UserService.Clients.IUserServiceClient userService,
        CodebookService.Clients.ICodebookServiceClient codebookService,
        IEasClient easClient,
        CaseServiceDbContext dbContext,
        ILogger<CreateCaseHandler> logger)
    {
        _bag = bag;
        _mediator = mediator;
         _userService = userService;
        _easClient = easClient;
        _dbContext = dbContext;
        _logger = logger;
        _codebookService = codebookService;
    }
}