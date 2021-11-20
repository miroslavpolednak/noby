using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.ProductService.Contracts;
using Grpc.Core;

namespace DomainServices.ProductService.Api.Handlers;

internal class UpdateHousingSavingsInstanceHandler
    : IRequestHandler<Dto.UpdateHousingSavingsInstanceMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateHousingSavingsInstanceMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Update product ID #{id}", request.ProductInstanceId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly ILogger<UpdateHousingSavingsInstanceHandler> _logger;
    private readonly MpHome.IMpHomeClient _mpHomeClient;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public UpdateHousingSavingsInstanceHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        MpHome.IMpHomeClient mpHomeClient,
        ILogger<UpdateHousingSavingsInstanceHandler> logger)
    {
        _mpHomeClient = mpHomeClient;
        _logger = logger;
        _codebookService = codebookService;
    }
}
