using CIS.Core.Security;
using SharedAudit;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using CIS.Core.Configuration;

namespace DomainServices.CaseService.Api.Endpoints.v1.GetCaseDetail;

internal sealed class GetCaseDetailHandler(
    ICisEnvironmentConfiguration _cisEnvironmentConfiguration,
    IServiceUserAccessor _serviceUser,
    CaseServiceDbContext _dbContext,
    IAuditLogger _auditLogger,
    ICurrentUserAccessor _currentUser)
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
            .Select(DatabaseExpressions.CaseDetail())
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
                products:
                [
                    new(AuditConstants.ProductNamesCase, request.CaseId)
                ]
            );
        }

        return model;
    }
}
