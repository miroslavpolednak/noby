using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer;

internal class SaveDraftHandler
    : IRequestHandler<SaveDraftRequest, SaveDraftResponse>
{
    public async Task<SaveDraftResponse> Handle(SaveDraftRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create case for {offerInstanceId}", request.OfferInstanceId);

        // vytvorit case
        long caseId = await createCase(request);
        _logger.LogDebug("Case #{caseId} created", caseId);

        // vytvorit zadost
        int salesArrangementId = resolveSalesArrangementResult(await _salesArrangementService.CreateSalesArrangement(caseId, _configuration.Savings.SavingsSalesArrangementType, offerInstanceId: request.OfferInstanceId));
        _logger.LogDebug("Sales arrangement #{salesArrangementId} created", salesArrangementId);

        return new SaveDraftResponse
        {
            CaseId = caseId,
            SalesArrangementId = salesArrangementId
        };
    }

    private int resolveSalesArrangementResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<int> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            _ => throw new NotImplementedException()
        };

    private async Task<long> createCase(SaveDraftRequest request)
    {
        var caseModel = new DomainServices.CaseService.Contracts.CreateCaseRequest()
        {
            UserId = _userAccessor.User.Id,
            ProductInstanceType = _configuration.Savings.SavingsProductInstanceType,
            DateOfBirthNaturalPerson = request.DateOfBirth,
            FirstNameNaturalPerson = request.FirstName,
            Name = request.LastName
        };
        if (request.Customer is not null)
            caseModel.Customer = new CIS.Infrastructure.gRPC.CisTypes.Identity(request.Customer);

        _logger.LogInformation("Create case with {model}", caseModel);

        return resolveCaseResult(await _caseService.CreateCase(caseModel));
    }
    
    private long resolveCaseResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            ErrorServiceCallResult e2 => throw new CIS.Core.Exceptions.CisValidationException(e2.Errors),
            _ => throw new NotImplementedException()
        };

    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly Infrastructure.Configuration.AppConfiguration _configuration;
    private readonly ILogger<SaveDraftHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public SaveDraftHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<SaveDraftHandler> logger,
        Infrastructure.Configuration.AppConfiguration configuration,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService, 
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _userAccessor = userAccessor;
        _logger = logger;
        _configuration = configuration;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
