﻿using CIS.Infrastructure.Caching.Grpc;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Api.Endpoints.v1.CreateCase;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.CreateExistingCase;

internal sealed class CreateExistingCaseHandler(
    IGrpcServerResponseCache _responseCache,
    UserService.Clients.v1.IUserServiceClient _userService,
    CaseServiceDbContext _dbContext,
    ILogger<CreateCaseHandler> _logger,
    TimeProvider _timeProvider)
        : IRequestHandler<CreateExistingCaseRequest, CreateCaseResponse>
{
    public async Task<CreateCaseResponse> Handle(CreateExistingCaseRequest request, CancellationToken cancellation)
    {
        // overit existenci ownera
        var userInstance = await _userService.GetUser(request.CaseOwnerUserId, cancellation);

        // vytvorit entitu
        var entity = createDatabaseEntity(request);
        entity.OwnerUserName = userInstance.UserInfo.DisplayName;//dotazene jmeno majitele caseu (poradce)

        try
        {
            _dbContext.Cases.Add(entity);
            await _dbContext.SaveChangesAsync(cancellation);

            _logger.EntityCreated(nameof(Database.Entities.Case), request.CaseId);
        }
        catch (DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException && ((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2627)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateAlreadyExistsException(ErrorCodeMapper.CaseAlreadyExist, request.CaseId);
        }

        await _responseCache.InvalidateEntry(nameof(ValidateCaseId), request.CaseId);

        return new CreateCaseResponse()
        {
            CaseId = request.CaseId
        };
    }

    private Database.Entities.Case createDatabaseEntity(CreateExistingCaseRequest request)
    {
        return new Database.Entities.Case
        {
            CaseId = request.CaseId,

            State = request.State,
            StateUpdatedInStarbuild = (int)UpdatedInStarbuildStates.Ok,
            StateUpdateTime = _timeProvider.GetLocalNow().DateTime,
            ProductTypeId = request.Data.ProductTypeId,

            Name = request.Customer.Name,
            FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson,
            DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson,
            Cin = request.Customer.Cin,
            CustomerPriceSensitivity = request.Customer.CustomerPriceSensitivity,
            CustomerChurnRisk = request.Customer.CustomerChurnRisk,

            TargetAmount = request.Data.TargetAmount,
            IsEmployeeBonusRequested = request.Data.IsEmployeeBonusRequested,
            ContractNumber = request.Data.ContractNumber,

            OwnerUserId = request.CaseOwnerUserId,

            CustomerIdentityScheme = (IdentitySchemes)Convert.ToInt32(request.Customer?.Identity?.IdentityScheme, CultureInfo.InvariantCulture),
            CustomerIdentityId = request.Customer?.Identity?.IdentityId
        };
    }
}
