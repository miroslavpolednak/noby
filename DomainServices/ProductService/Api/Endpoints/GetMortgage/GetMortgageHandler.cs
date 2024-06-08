using DomainServices.CodebookService.Clients;

namespace DomainServices.ProductService.Api.Endpoints.GetMortgage;

internal sealed class GetMortgageHandler(
	IMpHomeClient _mpHomeClient,
	ICodebookServiceClient _codebookService)
    : IRequestHandler<GetMortgageRequest, GetMortgageResponse>
{
    public async Task<GetMortgageResponse> Handle(GetMortgageRequest request, CancellationToken cancellationToken)
    {
        var loan = await _mpHomeClient.GetMortgage(request.ProductId, cancellationToken);
        var partner = await _mpHomeClient.GetPartner(loan!.PartnerId!.Value, cancellationToken);
		var (retention, refixation) = await _mpHomeClient.GetRefinancing(request.ProductId, cancellationToken);

        var mappedLoan = loan!.MapToProductServiceContract();
		mappedLoan.Statement.Address = await getStatementAddress(partner!, cancellationToken);
		
		// retence
		if (retention is not null && retention.MonthInstallment > 0)
		{
			mappedLoan.Retention = new MortgageData.Types.RetentionData
			{
				LoanInterestRate = retention.InterestRate.ToDecimal(),
				LoanInterestRateValidFrom = retention.From,
				LoanInterestRateValidTo = retention.To,
				LoanPaymentAmount = retention.MonthInstallment.ToDecimal()
			};
		}
		
		// refixace
		if (refixation is not null && refixation.FixationPeriod > 0)
		{
			mappedLoan.Refixation = new MortgageData.Types.RefixationData
			{
				FixedRatePeriod = refixation.FixationPeriod,
				LoanInterestRate = refixation.InterestRate.ToDecimal(),
				LoanInterestRateValidTo = refixation.InterestRateValidTo,
				LoanPaymentAmount = refixation.RefixFutureMonthInstallment.ToDecimal()
			};
		}

		return new GetMortgageResponse 
        { 
            Mortgage = mappedLoan
        };
    }

    private async Task<SharedTypes.GrpcTypes.GrpcAddress?> getStatementAddress(PartnerResponse partner, CancellationToken cancellationToken)
    {
		var address = partner.Addresses.FirstOrDefault(t => t.Type == AddressType.Mailing);
		var countries = await _codebookService.Countries(cancellationToken);

		if (address is not null) 
		{
			return new SharedTypes.GrpcTypes.GrpcAddress
			{
				Street = address.Street ?? string.Empty,
				StreetNumber = address.BuildingIdentificationNumber ?? string.Empty,
				HouseNumber = address.LandRegistryNumber ?? string.Empty,
				Postcode = address.PostCode ?? string.Empty,
				City = address.City ?? string.Empty,
				CountryId = countries.FirstOrDefault(t => t.ShortName == address.Country)?.Id
			};
		}

		return null;
	}
}