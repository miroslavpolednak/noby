using CIS.DomainServices.Security.Abstraction;

namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp.Handlers;

internal class BlobTempSaveCommandHandler : IRequestHandler<Dto.BlobTempSaveRequest, string>
{
    private readonly ILogger<BlobTempSaveCommandHandler> _logger;
    private readonly Core.Configuration.ICisEnvironmentConfiguration _configuration;
    private readonly Contracts.v1.BlobTemp.BlobTempClient _service;
    private readonly ICisUserContextHelpers _userContext;

    public BlobTempSaveCommandHandler(
        ILogger<BlobTempSaveCommandHandler> logger,
        Core.Configuration.ICisEnvironmentConfiguration configuration,
        Contracts.v1.BlobTemp.BlobTempClient service,
        ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _configuration = configuration;
        _service = service;
        _logger = logger;
    }

    public async Task<string> Handle(Dto.BlobTempSaveRequest request, CancellationToken cancellation)
    {
        string? key = request.ApplicationKey ?? this._configuration.DefaultApplicationKey;
        _logger.LogDebug("Saving blob {name} in app {key}", request.Name, key);

        var model = new Contracts.BlobTempSaveRequest
        {
            ApplicationKey = key,
            SessionId = request.SessionId,
            BlobData = new Contracts.BlobFileStructure
            {
                ContentType = request.ContentType,
                Name = request.Name,
                Data = Google.Protobuf.ByteString.CopyFrom(request.Data)
            }
        };
        var result = await _userContext.AddUserContext(async () => await _service.SaveAsync(model));

        _logger.LogDebug("Saved with key {key}", result.BlobKey);
        return result.BlobKey;
    }
}
