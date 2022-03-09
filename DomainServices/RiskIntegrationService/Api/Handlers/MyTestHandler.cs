using DomainServices.RiskIntegrationService.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Handlers;

internal class MyTestHandler
    : IRequestHandler<Dto.MyTestMediatrRequest, MyTestResponse>
{
    public async Task<MyTestResponse> Handle(Dto.MyTestMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(MyTestHandler));

        //TODO logic...

        return new MyTestResponse() { ResponseId = 1 };
    }

    private readonly ILogger<MyTestHandler> _logger;

    public MyTestHandler(ILogger<MyTestHandler> logger)
    {
        _logger = logger;
    }
}
