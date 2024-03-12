using CIS.Core.Security;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

internal abstract class BaseValidator<TBuilder>
    : ICreateSalesArrangementParametersValidator
    where TBuilder : class, ICreateSalesArrangementParametersBuilder
{
    protected DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest Request => _aggregate.Request;

    protected readonly BuilderValidatorAggregate _aggregate;

    public BaseValidator(BuilderValidatorAggregate aggregate)
    {
        _aggregate = aggregate;
    }

    protected void ValidateUserPermissions(UserPermissions requiredPermission)
    {
        if (!_aggregate.HttpContextAccessor.HttpContext!.RequestServices.GetRequiredService<ICurrentUserAccessor>().HasPermission(UserPermissions.APPLICATION_BasicAccess))
        {
            throw new CisAuthorizationException($"User does not have {requiredPermission} permission");
        }
    }

    protected TService GetRequiredService<TService>()
        where TService : class
        => _aggregate.HttpContextAccessor.HttpContext!.RequestServices.GetRequiredService<TService>();

    public virtual Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((ICreateSalesArrangementParametersBuilder)Activator.CreateInstance(typeof(TBuilder), _aggregate)!);
    }
}
