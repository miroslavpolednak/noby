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
        var partner = await _mpHomeClient.GetCustomer(loan!.PartnerId!.Value, cancellationToken);

        var mappedLoan = loan!.MapToProductServiceContract();
		mappedLoan.Statement.Address = await getStatementAddress(partner!, cancellationToken);

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