using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.CaseService.Contracts;
using Grpc.Core;

namespace DomainServices.CaseService.Api.Handlers;

internal class CreateCaseHandler
    : IRequestHandler<Dto.CreateCaseMediatrRequest, CreateCaseResponse>
{
    public async Task<CreateCaseResponse> Handle(Dto.CreateCaseMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(CreateCaseHandler));

        // zjistit o jakou kategorii produktu se jedna z daneho typu produktu - SS, Uver SS, Hypoteka
        var ProductTypeCategory = await getProductCategory(request.Request.Data.ProductTypeId);
        
        // kontrola, zda se jedna jen o SS nebo Hypo (uver SS nema nove CaseId - to uz existuje na sporeni)
        if (ProductTypeCategory == CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.BuildingSavingsLoan)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"ProductTypeId {request.Request.Data.ProductTypeId} is not valid for this operation", 13013);

        // overit existenci ownera
        var userInstance = resolveUserResult(await _userService.GetUser(request.Request.CaseOwnerUserId, cancellation));
        //TODO zkontrolovat existenci klienta?

        // pro jakou spolecnost
        var mandant = ProductTypeCategory == CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.Mortgage ? CIS.Core.IdentitySchemes.Kb : CIS.Core.IdentitySchemes.Mp;

        // get default case state
        int defaultCaseState = await getDefaultState();

        // ziskat caseId
        long newCaseId = resolveCaseIdResult(await _easClient.GetCaseId(mandant, request.Request.Data.ProductTypeId));
        _logger.NewCaseIdCreated(newCaseId);

        // vytvorit entitu
        var entity = Repositories.Entities.CaseInstance.Create(newCaseId, request.Request);
        entity.OwnerUserName = userInstance.FullName;//dotazene jmeno majitele caseu (poradce)
        entity.State = defaultCaseState;//vychozi status

        try
        {
            // ulozit entitu
            await _repository.CreateCase(entity, cancellation);
            _logger.EntityCreated(nameof(Repositories.Entities.CaseInstance), newCaseId);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException && ((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2627)
        {
            throw new CisAlreadyExistsException(13015, nameof(Repositories.Entities.CaseInstance), newCaseId);
        }
        catch
        {
            throw;
        }

        return new CreateCaseResponse() 
        {
            CaseId = newCaseId 
        };
    }

    /// <summary>
    /// Zjistit typ produktu - Hypo, SS atd.
    /// </summary>
    private async Task<CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory> getProductCategory(long ProductTypeId)
    {
        var productTypes = await _codebookService.ProductTypes();
        var item = productTypes.FirstOrDefault(t => t.Id == ProductTypeId) ?? throw new CisNotFoundException(13014, nameof(ProductTypeId), ProductTypeId);
        return item.ProductCategory;
    }

    /// <summary>
    /// Zjistit vychozi stav CASE
    /// </summary>
    private async Task<int> getDefaultState()
        => (await _codebookService.CaseStates()).FirstOrDefault(t => t.IsDefaultNewState)?.Id ?? throw new CisNotFoundException(13019, "Unable to determine default Case State");

    private static long resolveCaseIdResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r when r.Model > 0 => r.Model,
            SuccessfulServiceCallResult<long> r when r.Model == 0 => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, "Unable to get CaseId from SB", 13004),
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };

    private static UserService.Contracts.User resolveUserResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<UserService.Contracts.User> r => r.Model,
            ErrorServiceCallResult err => throw new CisNotFoundException(13017, $"User not found: {err.Errors.First().Message}"),
            _ => throw new NotImplementedException()
        };

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly UserService.Abstraction.IUserServiceAbstraction _userService;

    public CreateCaseHandler(
        UserService.Abstraction.IUserServiceAbstraction userService,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Eas.IEasClient easClient,
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _userService = userService;
        _easClient = easClient;
        _repository = repository;
        _logger = logger;
        _codebookService = codebookService;
    }
}