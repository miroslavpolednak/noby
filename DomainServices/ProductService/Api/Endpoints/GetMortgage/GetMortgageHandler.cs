using SharedTypes.GrpcTypes;

namespace DomainServices.ProductService.Api.Endpoints.GetMortgage;

internal sealed class GetMortgageHandler(IMpHomeClient _mpHomeClient)
    : IRequestHandler<GetMortgageRequest, GetMortgageResponse>
{
    public async Task<GetMortgageResponse> Handle(GetMortgageRequest request, CancellationToken cancellationToken)
    {
        var loan = await _mpHomeClient.GetMortgage(request.ProductId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        var relationships = await _repository.GetRelationships(request.ProductId, cancellationToken);

        var mortgage = loan.ToMortgage(relationships);

        mortgage.PcpId = await _repository.GetPcpIdByCaseId(request.ProductId, cancellationToken);
        mortgage.Statement.Address = await GetStatementAddress(request.ProductId, cancellationToken);
        mortgage.LoanRealEstates.AddRange(await GetLoanRealEstates(request.ProductId, cancellationToken));
        mortgage.LoanPurposes.AddRange(await GetLoanPurposes(request.ProductId, cancellationToken));

        return new GetMortgageResponse { Mortgage = mortgage };
    }
    private async Task<GrpcAddress?> GetStatementAddress(long caseId, CancellationToken cancellationToken)
    {
        var loanStatement = await _repository.GetLoanStatement(caseId, cancellationToken);

        if (loanStatement is null)
            return default;

        return new GrpcAddress
        {
            Street = loanStatement.Street ?? string.Empty,
            StreetNumber = loanStatement.StreetNumber ?? string.Empty,
            HouseNumber = loanStatement.HouseNumber ?? string.Empty,
            Postcode = loanStatement.Postcode ?? string.Empty,
            City = loanStatement.City ?? string.Empty,
            AddressPointId = loanStatement.AddressPointId ?? string.Empty,
            CountryId = loanStatement.CountryId
        };
    }

    private async Task<IEnumerable<Contracts.LoanRealEstate>> GetLoanRealEstates(long caseId, CancellationToken cancellationToken)
    {
        var realEstates = await _repository.GetLoanRealEstates(caseId, cancellationToken);

        if (realEstates.Count == 0)
        {
            return Enumerable.Empty<Contracts.LoanRealEstate>();
        }

        var collateral = await _repository.GetCollateral(caseId, cancellationToken);

        return realEstates.Select(r =>
        {
            var col = collateral.FirstOrDefault(c => c.RealEstateId == r.RealEstateId);

            return new Contracts.LoanRealEstate
            {
                RealEstateTypeId = (int)r.RealEstateTypeId,
                RealEstatePurchaseTypeId = r.RealEstatePurchaseTypeId,
                IsCollateral = col is not null
            };
        });
    }

    private async Task<IEnumerable<Contracts.LoanPurpose>> GetLoanPurposes(long caseId, CancellationToken cancellationToken)
    {
        var purposes = await _repository.GetLoanPurposes(caseId, cancellationToken);

        return purposes.Select(p => p.ToLoanPurpose());
    }
}