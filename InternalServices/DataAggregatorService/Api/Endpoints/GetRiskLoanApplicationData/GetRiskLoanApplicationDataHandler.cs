namespace CIS.InternalServices.DataAggregatorService.Api.Endpoints.GetRiskLoanApplicationData;

internal class GetRiskLoanApplicationDataHandler : IRequestHandler<GetRiskLoanApplicationDataRequest, GetRiskLoanApplicationDataResponse>
{
    public GetRiskLoanApplicationDataHandler()
    {
    }

    public Task<GetRiskLoanApplicationDataResponse> Handle(GetRiskLoanApplicationDataRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}