using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class GetResultHandler : IRequestHandler<GetResultRequest, GetResultResponse>
{
    private readonly UserAdapterService _userAdapterService;
    private readonly NotificationRepository _repository;
    
    public GetResultHandler(
        UserAdapterService userAdapterService,
        NotificationRepository repository)
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