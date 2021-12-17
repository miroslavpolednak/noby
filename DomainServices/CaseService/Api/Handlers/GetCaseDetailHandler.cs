using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Handlers;

internal class GetCaseDetailHandler
    : IRequestHandler<Dto.GetCaseDetailMediatrRequest, GetCaseDetailResponse>
{
    public async Task<GetCaseDetailResponse> Handle(Dto.GetCaseDetailMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get detail for #{id}", request.CaseId);

        var entity = await _repository.GetCaseDetail(request.CaseId);

        var model =  new GetCaseDetailResponse
        {
            State = entity.State,
            ActionRequired = entity.IsActionRequired,
            CaseId = entity.CaseId,
            ContractNumber = entity.ContractNumber ?? "",
            ProductInstanceType = entity.ProductInstanceType,
            UserId = entity.UserId,
            DateOfBirthNaturalPerson = entity.DateOfBirthNaturalPerson,
            FirstNameNaturalPerson = entity.FirstNameNaturalPerson,
            Name = entity.Name
        };
        if (entity.CustomerIdentityId.HasValue)
            model.Customer = new CIS.Core.Types.CustomerIdentity(entity.CustomerIdentityId.Value, entity.CustomerIdentityScheme.GetValueOrDefault());

        return model;
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public GetCaseDetailHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
