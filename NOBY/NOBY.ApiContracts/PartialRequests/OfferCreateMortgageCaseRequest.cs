namespace NOBY.ApiContracts;

[RollbackDescription("- maže domácnost s customerOnSA<br/>- maže SalesArrangement<br/>- maže Case")]
public partial class OfferCreateMortgageCaseRequest
    : IRequest<OfferCreateMortgageCaseResponse>, CIS.Infrastructure.CisMediatR.Rollback.IRollbackCapable
{
}
