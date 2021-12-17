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
        var productInstanceTypeCategory = await getProductCategory(request.Request.ProductInstanceType);
        
        // kontrola, zda se jedna jen o SS nebo Hypo (uver SS nema nove CaseId - to uz existuje na sporeni)
        //TODO je to tak i pro 4001?
        if (productInstanceTypeCategory == CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.BuildingSavingsLoan)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "productInstanceTypeCategory is not valid for this operation", 13001);

        // get default case state
        int defaultCaseState = (await _codebookService.CaseStates()).First(t => t.IsDefaultNewState).Id;

        // ziskat caseId
        //TODO proc se tady dava schema?
        long newCaseId = resolveCaseIdResult(await _easClient.GetCaseId(CIS.Core.IdentitySchemes.MP, request.Request.ProductInstanceType));

        // zalozit case v NOBY
        try
        {
            var entity = new Repositories.Entities.CaseInstance
            {
                CaseId = newCaseId,
                Name = request.Request.Name,
                FirstNameNaturalPerson = request.Request.FirstNameNaturalPerson,
                State = defaultCaseState,
                UserId = request.Request.UserId,
                ProductInstanceType = request.Request.ProductInstanceType,
                DateOfBirthNaturalPerson = request.Request.DateOfBirthNaturalPerson
            };
            // pokud je zadany customer
            if (request.Request.Customer is not null)
            {
                entity.CustomerIdentityScheme = (CIS.Core.IdentitySchemes)Convert.ToInt32(request.Request.Customer?.IdentityScheme);
                entity.CustomerIdentityId = request.Request.Customer?.IdentityId;
            }

            await _repository.CreateCase(entity);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException && ((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2627)
        {
            _logger.LogError("Case ID #{id} already exists", newCaseId);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, $"Case ID #{newCaseId} already exists", 13001);
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

    private long resolveCaseIdResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r when r.Model > 0 => r.Model,
            SuccessfulServiceCallResult<long> r when r.Model == 0 => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "Unable to create MktItem instance in Starbuild.", 12002),
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };

    private async Task<CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory> getProductCategory(long productInstanceType)
    {
        var productTypes = await _codebookService.ProductInstanceTypes();
        var item = productTypes.FirstOrDefault(t => t.Id == productInstanceType);
        if (item == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, "ProductInstanceType not found", 1);
        return item.ProductCategory;
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CreateCaseHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Eas.IEasClient easClient,
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _easClient = easClient;
        _repository = repository;
        _logger = logger;
        _codebookService = codebookService;
    }
}