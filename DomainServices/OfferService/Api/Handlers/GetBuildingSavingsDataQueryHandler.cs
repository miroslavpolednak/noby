using DomainServices.OfferService.Contracts;
using System.Text.Json;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetBuildingSavingsDataQueryHandler 
    : IRequestHandler<Dto.GetBuildingSavingsDataMediatrRequest, GetBuildingSavingsDataResponse>
{
    public async Task<GetBuildingSavingsDataResponse> Handle(Dto.GetBuildingSavingsDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get offer instance ID #{id}", request.OfferInstanceId);

        var entity = await _repository.Get(request.OfferInstanceId);
        var data = JsonSerializer.Deserialize<Dto.Models.BuildingSavingsDataModel>(entity.Outputs ?? "");

        var model = new GetBuildingSavingsDataResponse
        {
            OfferInstanceId = entity.OfferInstanceId,
            Created = new(entity),
            InputData = JsonSerializer.Deserialize<BuildingSavingsInput>(entity.Inputs ?? ""),
            BuildingSavings = data?.Savings,
            Loan = data?.Loan
        };
        return model;
    }

    private readonly Repositories.SimulateBuildingSavingsRepository _repository;
    private readonly ILogger<SimulateBuildingSavingsCommandHandler> _logger;

    public GetBuildingSavingsDataQueryHandler(
        Repositories.SimulateBuildingSavingsRepository repository,
        ILogger<SimulateBuildingSavingsCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
