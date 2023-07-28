﻿using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Telemetry.AuditLog;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.GetCaseDetail;

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
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        Helpers.ThrowIfCaseIsCancelled(model.State);

        // auditni log
        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby006,
            "Přístup na případ, kde přistupující není majitelem případu",
            products: new List<AuditLoggerHeaderItem>
            { 
                new("case", request.CaseId)
            }
        );

        return model;
    }

    private readonly IAuditLogger _auditLogger;
    private readonly CaseServiceDbContext _dbContext;

    public GetCaseDetailHandler(CaseServiceDbContext dbContext, IAuditLogger auditLogger)
    {
        _auditLogger = auditLogger;
        _dbContext = dbContext;
    }
}
