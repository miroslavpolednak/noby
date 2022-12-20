using CIS.Infrastructure.gRPC;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using Grpc.Core;

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
        int defaultCaseState = (await _codebookService.CaseStates(cancellation)).First(t => t.IsDefault).Id;

        // ziskat caseId
        long newCaseId = resolveCaseIdResult(await _easClient.GetCaseId(CIS.Foms.Enums.IdentitySchemes.Kb, request.Data.ProductTypeId));
        _logger.NewCaseIdCreated(newCaseId);

        // vytvorit entitu
        var entity = createDatabaseEntity(request, newCaseId);
        entity.OwnerUserName = userInstance.FullName;//dotazene jmeno majitele caseu (poradce)
        entity.State = defaultCaseState;//vychozi status
        
        try
        {
            _dbContext.Cases.Add(entity);
            await _dbContext.SaveChangesAsync(cancellation);

            _logger.EntityCreated(nameof(Database.Entities.Case), newCaseId);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException && ((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2627)
        {
            throw new CisAlreadyExistsException(13015, nameof(Database.Entities.Case), newCaseId);
        }

        // fire notification
        await _mediator.Publish(new Notifications.CaseStateChangedNotification
        {
            CaseId = newCaseId,
            CaseStateId = defaultCaseState,
            ClientName = $"{request.Customer?.FirstNameNaturalPerson} {request.Customer?.Name}",
            ProductTypeId = request.Data.ProductTypeId,
            CaseOwnerUserId = request.CaseOwnerUserId
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

            StateUpdateTime = _dateTime.Now,
            ProductTypeId = request.Data.ProductTypeId,

            Name = request.Customer.Name,
            FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson,
            DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson,
            Cin = request.Customer.Cin,

            TargetAmount = request.Data.TargetAmount,
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
        entity.PhoneNumberForOffer = request.OfferContacts?.PhoneNumberForOffer;

        return entity;
    }

    private static long resolveCaseIdResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r when r.Model > 0 => r.Model,
            SuccessfulServiceCallResult<long> r when r.Model == 0 => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, "Unable to get CaseId from SB", 13004),
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors[0].Message, err.Errors[0].Key),
            _ => throw new NotImplementedException()
        };

    private readonly IMediator _mediator;
    private readonly CIS.Core.IDateTime _dateTime;
    private readonly CaseServiceDbContext _dbContext;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly UserService.Clients.IUserServiceClient _userService;

    public CreateCaseHandler(
        IMediator mediator,
        CIS.Core.IDateTime dateTime,
        UserService.Clients.IUserServiceClient userService,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Eas.IEasClient easClient,
        CaseServiceDbContext dbContext,
        ILogger<CreateCaseHandler> logger)
    {
        _mediator = mediator;
        _dateTime = dateTime;
        _userService = userService;
        _easClient = easClient;
        _dbContext = dbContext;
        _logger = logger;
        _codebookService = codebookService;
    }
}