namespace NOBY.Api.Endpoints.Cases.GetCurrentPriceException;

internal sealed class GetCurrentPriceExceptionHandler
    : IRequestHandler<GetCurrentPriceExceptionRequest, GetCurrentPriceExceptionResponse>
{
    public async Task<GetCurrentPriceExceptionResponse> Handle(GetCurrentPriceExceptionRequest request, CancellationToken cancellationToken)
    {
        return null;
    }

    public GetCurrentPriceExceptionHandler()
    {

    }
}
