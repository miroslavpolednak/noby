using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using CIS.Infrastructure.Caching;

namespace DomainServices.CaseService.Api.Endpoints.v1.ValidateCaseId;

internal sealed class ValidateCaseIdHandler(CaseServiceDbContext _dbContext, IDistributedCache _distributedCache)
        : IRequestHandler<ValidateCaseIdRequest, ValidateCaseIdResponse>
{
    public async Task<ValidateCaseIdResponse> Handle(ValidateCaseIdRequest request, CancellationToken cancellationToken)
    {
        await _distributedCache.SetObjectAsync("CaseService:CaseStateChanged_1", new SharedDto.CaseStateChangeRequestId
            {
                RequestId = 1,
                CaseId = 1,
                CreatedTime = DateTime.Now
            }, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(1),
            },
           CIS.Core.Configuration.ICisDistributedCacheConfiguration.SerializationTypes.Protobuf,
           cancellationToken);

        var instance = await _dbContext.Cases
            .Where(t => t.CaseId == request.CaseId)
            .Select(t => new { t.State, t.OwnerUserId })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ThrowExceptionIfNotFound && instance is null)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        }

        return new ValidateCaseIdResponse
        {
            Exists = instance is not null,
            OwnerUserId = instance?.OwnerUserId,
            State = instance?.State
        };
    }
}
