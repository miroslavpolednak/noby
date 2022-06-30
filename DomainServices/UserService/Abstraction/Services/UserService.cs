﻿using CIS.Core.Results;
using Microsoft.Extensions.Logging;

namespace DomainServices.UserService.Abstraction.Services;

internal class UserService : IUserServiceAbstraction
{
    public async Task<IServiceCallResult> GetUserByLogin(string login, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction GetUserByLogin {login}", login);

        try
        {
            var result = await _service.GetUserByLoginAsync(
                new Contracts.GetUserByLoginRequest
                {
                    Login = login
                }, cancellationToken: cancellationToken);
            return new SuccessfulServiceCallResult<Contracts.User>(result);
        }
        catch (CIS.Core.Exceptions.CisNotFoundException ex)
        {
            return new ErrorServiceCallResult(ex.ExceptionCode, ex.Message);
        }
    }

    public async Task<IServiceCallResult> GetUser(int userId, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            var result = await _service.GetUserAsync(
                new Contracts.GetUserRequest
                {
                    UserId = userId
                }, cancellationToken: cancellationToken);
            return new SuccessfulServiceCallResult<Contracts.User>(result);
        }
        catch (CIS.Core.Exceptions.CisNotFoundException ex)
        {
            return new ErrorServiceCallResult(ex.ExceptionCode, ex.Message);
        }
    }

    private readonly ILogger<UserService> _logger;
    private readonly Contracts.v1.UserService.UserServiceClient _service;

    public UserService(
        ILogger<UserService> logger,
        Contracts.v1.UserService.UserServiceClient service)
    {
        _service = service;
        _logger = logger;
    }
}
