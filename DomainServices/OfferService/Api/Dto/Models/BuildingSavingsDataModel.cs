using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto.Models;

public class BuildingSavingsDataModel
{
    public BuildingSavingsData? Savings { get; set; }
    public LoanData? Loan { get; set; }
}
