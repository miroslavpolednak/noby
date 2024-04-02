﻿using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.ValidateCaseId;

internal sealed class ValidateCaseIdHandler
    : IRequestHandler<ValidateCaseIdRequest, ValidateCaseIdResponse>
{
    public async Task<ValidateCaseIdResponse> Handle(ValidateCaseIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _dbContext.Cases
            .Where(t => t.CaseId == request.CaseId)
            .Select(t => new { t.State, t.OwnerUserId })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && instance is null)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        }
        Helpers.ThrowIfCaseIsCancelled(instance?.State);

        return new ValidateCaseIdResponse
        {
            Exists = instance is not null,
            OwnerUserId = instance?.OwnerUserId,
            State = instance?.State
        };
    }

    private readonly CaseServiceDbContext _dbContext;

    public ValidateCaseIdHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}