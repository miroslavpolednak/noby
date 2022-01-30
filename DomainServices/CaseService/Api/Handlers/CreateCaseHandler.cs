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
        _logger.LogInformation("Create case");

        // zjistit o jakou kategorii produktu se jedna z daneho typu produktu - SS, Uver SS, Hypoteka
        var productInstanceTypeCategory = await getProductCategory(request.Request.Data.ProductInstanceTypeId);
        
        // kontrola, zda se jedna jen o SS nebo Hypo (uver SS nema nove CaseId - to uz existuje na sporeni)
        if (productInstanceTypeCategory == CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.BuildingSavingsLoan)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"ProductInstanceTypeId {request.Request.Data.ProductInstanceTypeId} is not valid for this operation", 13013);

        // overit existenci ownera
        var userInstance = resolveUserResult(await _userService.GetUser(request.Request.CaseOwnerUserId));
        //TODO zkontrolovat existenci klienta?

        // pro jakou spolecnost
        var mandant = productInstanceTypeCategory == CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.Mortgage ? CIS.Core.IdentitySchemes.Kb : CIS.Core.IdentitySchemes.Mp;

        // get default case state
        int defaultCaseState = await getDefaultState();

        // ziskat caseId
        long newCaseId = resolveCaseIdResult(await _easClient.GetCaseId(mandant, request.Request.Data.ProductInstanceTypeId));
        _logger.LogDebug("newCaseId={newCaseId}", newCaseId);

        // vytvorit entitu
        var entity = Repositories.Entities.CaseInstance.Create(newCaseId, request.Request);
        entity.OwnerUserName = userInstance.FullName;//dotazene jmeno majitele caseu (poradce)
        entity.State = defaultCaseState;//vychozi status

        try
        {
            // ulozit entitu
            await _repository.CreateCase(entity, cancellation);
            _logger.LogDebug("Case ID #{id} saved", newCaseId);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException && ((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2627)
        {
            _logger.LogError("Case ID #{id} already exists", newCaseId);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, $"Case #{newCaseId} already exists", 13015);
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
    private async Task<CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory> getProductCategory(long ProductInstanceTypeId)
    {
        var productTypes = await _codebookService.ProductInstanceTypes();

        var item = productTypes.FirstOrDefault(t => t.Id == ProductInstanceTypeId);
        if (item == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"ProductInstanceTypeId {ProductInstanceTypeId} not found", 13014);

        _logger.LogDebug("ProductInstanceTypeCategory={id}", item.ProductCategory);

        return item.ProductCategory;
    }

    /// <summary>
    /// Zjistit vychozi stav CASE
    /// </summary>
    private async Task<int> getDefaultState()
    {
        int defaultCaseState = (await _codebookService.CaseStates()).First(t => t.IsDefaultNewState).Id;
        _logger.LogDebug("defaultCaseState={defaultCaseState}", defaultCaseState);
        return defaultCaseState;
    }

    private long resolveCaseIdResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r when r.Model > 0 => r.Model,
            SuccessfulServiceCallResult<long> r when r.Model == 0 => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "Unable to get CaseId from SB", 13004),
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };

    private UserService.Contracts.User resolveUserResult(IServiceCallResult result) =>
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