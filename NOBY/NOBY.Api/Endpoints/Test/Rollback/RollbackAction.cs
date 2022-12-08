﻿using CIS.Infrastructure.MediatR.Rollback;

namespace NOBY.Api.Endpoints.Test.Rollback;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.AsImplementedInterfacesService]
internal class RollbackAction
    : IRollbackAction<RollbackRequest>
{
    private readonly IRollbackBag _bag;
    private readonly ILogger<RollbackAction> _logger;

    public RollbackAction(IRollbackBag bag, ILogger<RollbackAction> logger)
    {
        _logger= logger;
        _bag = bag;
    }

    public async Task ExecuteRollback(Exception exception, RollbackRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogInformation($"ID: {_bag["id"]}");
    }
}
