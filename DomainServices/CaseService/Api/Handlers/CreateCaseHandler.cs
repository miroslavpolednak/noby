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
        // zjistit o jakou kategorii produktu se jedna z daneho typu produktu - SS, Uver SS, Hypoteka
        var productTypeCategory = await getProductCategory(request.Request.Data.ProductTypeId);
        
        // kontrola, zda se jedna jen o SS nebo Hypo (uver SS nema nove CaseId - to uz existuje na sporeni)
        if (productTypeCategory == CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.BuildingSavingsLoan)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"ProductTypeId {request.Request.Data.ProductTypeId} is not valid for this operation", 13013);

        // overit existenci ownera
        var userInstance = resolveUserResult(await _userService.GetUser(request.Request.CaseOwnerUserId, cancellation));
        //TODO zkontrolovat existenci klienta?

        // pro jakou spolecnost
        var mandant = productTypeCategory == CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.Mortgage ? CIS.Foms.Enums.IdentitySchemes.Kb : CIS.Foms.Enums.IdentitySchemes.Mp;

        // get default case state
        int defaultCaseState = (await _codebookService.CaseStates(cancellation)).First(t => t.IsDefault).Id;

        // ziskat caseId
        long newCaseId = resolveCaseIdResult(await _easClient.GetCaseId(mandant, request.Request.Data.ProductTypeId));
        _logger.NewCaseIdCreated(newCaseId);

        // vytvorit entitu
        var entity = Repositories.Entities.Case.Create(newCaseId, request.Request, _dateTime.Now);
        entity.OwnerUserName = userInstance.FullName;//dotazene jmeno majitele caseu (poradce)
        entity.State = defaultCaseState;//vychozi status

        try
        {
            // ulozit entitu
            await _repository.CreateCase(entity, cancellation);
            _logger.EntityCreated(nameof(Repositories.Entities.Case), newCaseId);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException && ((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2627)
        {
            throw new CisAlreadyExistsException(13015, nameof(Repositories.Entities.Case), newCaseId);
        }

        // fire notification
        await _mediator.Publish(new Notifications.CaseStateChangedNotification
        {
            CaseId = newCaseId,
            CaseStateId = defaultCaseState,
            ClientName = $"{request.Request.Customer?.FirstNameNaturalPerson} {request.Request.Customer?.Name}",
            ProductTypeId = request.Request.Data.ProductTypeId,
            CaseOwnerUserId = request.Request.CaseOwnerUserId
        }, cancellation);

        return new CreateCaseResponse() 
        {
            CaseId = newCaseId 
        };
    }

    /// <summary>
    /// Zjistit typ produktu - Hypo, SS atd.
    /// </summary>
    private async Task<CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory> getProductCategory(long productTypeId)
    {  
        var item = (await _codebookService.ProductTypes())
            .FirstOrDefault(t => t.Id == productTypeId) ?? throw new CisNotFoundException(13014, nameof(productTypeId), productTypeId);
        return item.ProductCategory;
    }
    
    private static long resolveCaseIdResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r when r.Model > 0 => r.Model,
            SuccessfulServiceCallResult<long> r when r.Model == 0 => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, "Unable to get CaseId from SB", 13004),
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, err.Errors[0].Message, err.Errors[0].Key),
            _ => throw new NotImplementedException()
        };

    private static UserService.Contracts.User resolveUserResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<UserService.Contracts.User> r => r.Model,
            ErrorServiceCallResult err => throw new CisNotFoundException(13017, $"User not found: {err.Errors[0].Message}"),
            _ => throw new NotImplementedException()
        };

    private readonly IMediator _mediator;
    private readonly CIS.Core.IDateTime _dateTime;
    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly UserService.Abstraction.IUserServiceAbstraction _userService;

    public CreateCaseHandler(
        IMediator mediator,
        CIS.Core.IDateTime dateTime,
        UserService.Abstraction.IUserServiceAbstraction userService,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Eas.IEasClient easClient,
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _mediator = mediator;
        _dateTime = dateTime;
        _userService = userService;
        _easClient = easClient;
        _repository = repository;
        _logger = logger;
        _codebookService = codebookService;
    }
}