using CIS.Infrastructure.CisMediatR.Rollback;

namespace NOBY.Api.Endpoints.Test.Rollback;

public record RollbackRequest(int? Id) : IRequest<RollbackResponse>, IRollbackCapable { }
public class RollbackResponse { }

public class RollbackHandler
    : IRequestHandler<RollbackRequest, RollbackResponse>
{
    public async Task<RollbackResponse> Handle(RollbackRequest request, CancellationToken cancellationToken)
    {
        _bag.Add("id", request.Id?.ToString());

        throw new CisValidationException(1, "ahoj");

        return new RollbackResponse();
    }

    private readonly IRollbackBag _bag;

    public RollbackHandler(IRollbackBag bag)
    {
        _bag = bag;
    }
}
