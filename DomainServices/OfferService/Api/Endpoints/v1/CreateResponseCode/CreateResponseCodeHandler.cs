using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Endpoints.v1.CreateResponseCode;

internal sealed class CreateResponseCodeHandler(Database.OfferServiceDbContext _dbContext)
        : IRequestHandler<CreateResponseCodeRequest, CreateResponseCodeResponse>
{
    public async Task<CreateResponseCodeResponse> Handle(CreateResponseCodeRequest request, CancellationToken cancellationToken)
    {
        var entity = new Database.Entities.ResponseCode
        {
            ResponseCodeTypeId = request.ResponseCodeTypeId,
            CaseId = request.CaseId,
            Data = request.Data
        };
        _dbContext.ResponseCodes.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        //TODO: zalozit do BDP


        return new CreateResponseCodeResponse
        {
            ResponseCodeId = entity.ResponseCodeId
        };
    }
}
