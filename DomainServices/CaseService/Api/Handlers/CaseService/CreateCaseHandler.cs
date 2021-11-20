using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.CaseService.Contracts;
using Grpc.Core;

namespace DomainServices.CaseService.Api.Handlers.CaseService;

internal class CreateCaseHandler
    : IRequestHandler<Dto.CaseService.CreateCaseMediatrRequest, CreateCaseResponse>
{
    public async Task<CreateCaseResponse> Handle(Dto.CaseService.CreateCaseMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Create case");

        // zjistit o jakou kategorii produktu se jedna z daneho typu produktu - SS, Uver SS, Hypoteka
        var productInstanceTypeCategory = await getProductCategory(request.Request.ProductInstanceType);
        // kontrola, zda se jedna jen o SS nebo Hypo
        if (productInstanceTypeCategory == CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.BuildingSavingsLoan)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "productInstanceTypeCategory is not valid for this operation", 13001);

        // get default case state
        int defaultCaseState = 1; //TODO jak poresit vychozi stav? Nastavit v codebook svc?
        long? newCaseId = default(long);

        switch (productInstanceTypeCategory)
        {
            case CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.BuildingSavings:
            case CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.BuildingSavingsLoan:
                newCaseId = resolveCaseIdResult(await _easClient.GetCaseId(CIS.Core.MandantTypes.MP, request.Request.ProductInstanceType));
                break;

            default:
                throw new NotImplementedException($"Processing for category type {productInstanceTypeCategory} is not implemented");
        }

        // zalozit case v NOBY
        try
        {
            await _repository.CreateCase(new()
            {
                CaseId = newCaseId.Value,
                Name = request.Request.Name,
                FirstNameNaturalPerson = request.Request.FirstNameNaturalPerson,
                CustomerIdentityId = request.Request.Customer?.IdentityId,
                State = defaultCaseState,
                PartyId = request.Request.PartyId,
                ProductInstanceType = request.Request.ProductInstanceType,
                DateOfBirthNaturalPerson = request.Request.DateOfBirthNaturalPerson,
                InsertTime = _dateTime.Now,
                InsertUserId = 1//TODO pridat userid
            });
        }
        catch (System.Data.SqlClient.SqlException ex) when (ex.Number == 2627) // ID uz existuje, jak je to mozne? 
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
            CaseId = newCaseId.Value 
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

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly CIS.Core.IDateTime _dateTime;

    public CreateCaseHandler(
        CIS.Core.IDateTime dateTime,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Eas.IEasClient easClient,
        Repositories.NobyDbRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _dateTime = dateTime;
        _easClient = easClient;
        _repository = repository;
        _logger = logger;
        _codebookService = codebookService;
    }
}