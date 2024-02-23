using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Api.Endpoints.CreateCase;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.CreateExistingCase;

internal sealed class CreateExistingCaseHandler
    : IRequestHandler<CreateExistingCaseRequest, CreateCaseResponse>
{
    public async Task<CreateCaseResponse> Handle(CreateExistingCaseRequest request, CancellationToken cancellation)
    {
        // overit existenci ownera
        var userInstance = await _userService.GetUser(request.CaseOwnerUserId, cancellation);

        // vytvorit entitu
        var entity = createDatabaseEntity(request);
        entity.OwnerUserName = userInstance.UserInfo.DisplayName;//dotazene jmeno majitele caseu (poradce)
        
        try
        {
            _dbContext.Cases.Add(entity);
            await _dbContext.SaveChangesAsync(cancellation);
            
            _logger.EntityCreated(nameof(Database.Entities.Case), request.CaseId);
        }
        catch (DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException && ((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2627)
        {
            throw ErrorCodeMapper.CreateAlreadyExistsException(ErrorCodeMapper.CaseAlreadyExist, request.CaseId);
        }

        return new CreateCaseResponse()
        {
            CaseId = request.CaseId
        };
    }

    private Database.Entities.Case createDatabaseEntity(CreateExistingCaseRequest request)
    {
        return new Database.Entities.Case
        {
            CaseId = request.CaseId,

            State = request.State,
            StateUpdatedInStarbuild = (int)UpdatedInStarbuildStates.Ok,
            StateUpdateTime = _timeProvider.GetLocalNow().DateTime,
            ProductTypeId = request.Data.ProductTypeId,

            Name = request.Customer.Name,
            FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson,
            DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson,
            Cin = request.Customer.Cin,
            CustomerPriceSensitivity = request.Customer.CustomerPriceSensitivity,
            CustomerChurnRisk = request.Customer.CustomerChurnRisk,

            TargetAmount = request.Data.TargetAmount,
            IsEmployeeBonusRequested = request.Data.IsEmployeeBonusRequested,
            ContractNumber = request.Data.ContractNumber,

            OwnerUserId = request.CaseOwnerUserId,

            CustomerIdentityScheme = (SharedTypes.Enums.IdentitySchemes)Convert.ToInt32(request.Customer?.Identity?.IdentityScheme, CultureInfo.InvariantCulture),
            CustomerIdentityId = request.Customer?.Identity?.IdentityId
        };
    }

    private readonly TimeProvider _timeProvider;
    private readonly CaseServiceDbContext _dbContext;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly UserService.Clients.IUserServiceClient _userService;

    public CreateExistingCaseHandler(
        UserService.Clients.IUserServiceClient userService,
        CaseServiceDbContext dbContext,
        ILogger<CreateCaseHandler> logger,
        TimeProvider timeProvider)
    {
        _userService = userService;
        _dbContext = dbContext;
        _logger = logger;
        _timeProvider = timeProvider;
    }
}
