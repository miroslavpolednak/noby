using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Savings.Offer;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class BaseCaseHandlerAggregate
{
    public DomainServices.OfferService.Abstraction.IOfferServiceAbstraction OfferService { get; init; }
    public DomainServices.CaseService.Abstraction.ICaseServiceAbstraction CaseService { get; init; }
    public DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction SalesArrangementService { get; init; }
    public Infrastructure.Configuration.AppConfiguration Configuration { get; init; }
    public CIS.Core.Security.ICurrentUserAccessor UserAccessor { get; init; }

    public BaseCaseHandlerAggregate(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        Infrastructure.Configuration.AppConfiguration configuration,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        OfferService = offerService;
        UserAccessor = userAccessor;
        Configuration = configuration;
        CaseService = caseService;
        SalesArrangementService = salesArrangementService;
    }
}

internal abstract class BaseCaseHandler
{
    protected async Task<int> createSalesArrangement(long caseId, int offerInstanceId)
    {
        return resolveSalesArrangementResult(await _aggregate.SalesArrangementService.CreateSalesArrangement(caseId, _aggregate.Configuration.BuildingSavings.SavingsSalesArrangementType, offerInstanceId: offerInstanceId));
    }

    private int resolveSalesArrangementResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<int> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            _ => throw new NotImplementedException()
        };

    protected async Task<long> createCase(int offerInstanceId, string? firstName, string? lastName, DateTime? dateOfBirth, CIS.Core.Types.CustomerIdentity? customer)
    {
        // dotahnout informace o offerInstance
        var offerInstance = resolveOfferResult(await _aggregate.OfferService.GetBuildingSavingsData(offerInstanceId));

        var caseModel = new DomainServices.CaseService.Contracts.CreateCaseRequest()
        {
            UserId = _aggregate.UserAccessor.User.Id,
            ProductInstanceType = _aggregate.Configuration.BuildingSavings.SavingsProductInstanceType,
            DateOfBirthNaturalPerson = dateOfBirth,
            FirstNameNaturalPerson = firstName,
            Name = lastName,
            TargetAmount = offerInstance.InputData.TargetAmount
        };
        if (customer is not null)
            caseModel.Customer = new CIS.Infrastructure.gRPC.CisTypes.Identity(customer);

        return resolveCaseResult(await _aggregate.CaseService.CreateCase(caseModel));
    }

    private DomainServices.OfferService.Contracts.GetBuildingSavingsDataResponse resolveOfferResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.OfferService.Contracts.GetBuildingSavingsDataResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };

    private long resolveCaseResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            ErrorServiceCallResult e2 => throw new CIS.Core.Exceptions.CisValidationException(e2.Errors),
            _ => throw new NotImplementedException()
        };

    protected readonly BaseCaseHandlerAggregate _aggregate;

    public BaseCaseHandler(BaseCaseHandlerAggregate aggregate)
    {
        _aggregate = aggregate;
    }
}
