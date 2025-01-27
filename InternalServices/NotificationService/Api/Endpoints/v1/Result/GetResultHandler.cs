﻿using CIS.InternalServices.NotificationService.Api.Legacy;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.LegacyContracts.Result;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Result;

internal class GetResultHandler : IRequestHandler<GetResultRequest, GetResultResponse>
{
    private readonly IUserAdapterService _userAdapterService;
    private readonly INotificationRepository _repository;
    
    public GetResultHandler(
        IUserAdapterService userAdapterService,
        INotificationRepository repository)
    {
        _userAdapterService = userAdapterService;
        _repository = repository;
    }
    
    public async Task<GetResultResponse> Handle(GetResultRequest request, CancellationToken cancellationToken)
    {
        _userAdapterService.CheckReadResultAccess();
        
        var result = await _repository.GetResult(request.NotificationId, cancellationToken);
        return new GetResultResponse { Result = result.ToDto() };
    }
}