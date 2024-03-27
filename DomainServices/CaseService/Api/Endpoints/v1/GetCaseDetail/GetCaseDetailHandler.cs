using CIS.Core.Security;
using SharedAudit;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using CIS.Core.Configuration;

namespace DomainServices.CaseService.Api.Endpoints.v1.GetCaseDetail;

internal sealed class GetCaseDetailHandler
    : IRequestHandler<GetCaseDetailRequest, Case>
{
    /// <summary>
    /// Vraci detail Case-u
    /// </summary>
    public async Task<Case> Handle(GetCaseDetailRequest request, CancellationToken cancellation)
    {
        // vytahnout Case z DB
        var model = await _dbContext.Cases
            .Where(t => t.CaseId == request.CaseId)
            .AsNoTracking()
            .Select(CaseServiceDatabaseExpressions.CaseDetail())
            .FirstOrDefaultAsync(cancellation)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        Helpers.ThrowIfCaseIsCancelled(model.State);

        // auditni log
        if (_currentUser.IsAuthenticated && _currentUser.User!.Id != model.CaseOwner.UserId
            || !_currentUser.IsAuthenticated && _serviceUser.IsAuthenticated && _serviceUser.User!.Name != _cisEnvironmentConfiguration.InternalServicesLogin)
        {
            _auditLogger.Log(
                AuditEventTypes.Noby009,
                "Přístup na případ, kde přistupující není majitelem případu",
                products: new List<AuditLoggerHeaderItem>
                {
                    new(AuditConstants.ProductNamesCase, request.CaseId)
                }
            );
        }

        return model;
    }

    private readonly ICurrentUserAccessor _currentUser;
    private readonly IAuditLogger _auditLogger;
    private readonly CaseServiceDbContext _dbContext;
    private readonly ICisEnvironmentConfiguration _cisEnvironmentConfiguration;
    private readonly IServiceUserAccessor _serviceUser;

    public GetCaseDetailHandler(
        ICisEnvironmentConfiguration cisEnvironmentConfiguration,
        IServiceUserAccessor serviceUser,
        CaseServiceDbContext dbContext,
        IAuditLogger auditLogger,
        ICurrentUserAccessor currentUser)
    {
        _cisEnvironmentConfiguration = cisEnvironmentConfiguration;
        _serviceUser = serviceUser;
        _currentUser = currentUser;
        _auditLogger = auditLogger;
        _dbContext = dbContext;
    }
}
