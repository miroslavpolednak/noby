using DomainServices.ProductService.Contracts;
using DomainServices.CodebookService.Clients;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetMortgage;

internal sealed class GetMortgageHandler
    : IRequestHandler<GetMortgageRequest, GetMortgageResponse>
{
    private readonly Database.ProductServiceDbContext _dbContext;
    private readonly Database.LoanRepository _repository;

    #region Construction

    public GetMortgageHandler(
        Database.ProductServiceDbContext dbContext,
        Database.LoanRepository repository)
    {
        _repository = repository;
        _dbContext = dbContext;
    }
    #endregion

    public async Task<GetMortgageResponse> Handle(GetMortgageRequest request, CancellationToken cancellation)
    {
        var loan = await _repository.GetLoan(request.ProductId, cancellation);

        if (loan == null)
        {
            throw new CisNotFoundException(12001, nameof(Database.Entities.Loan), request.ProductId);
        }

        var relationships = await _repository.GetRelationships(request.ProductId, cancellation);

        var mortgage = loan.ToMortgage(relationships);

        // pcpid
        mortgage.PcpId = await _dbContext.LoanReservations.Where(t => t.UverId == request.ProductId).Select(t => t.PcpInstId).FirstOrDefaultAsync(cancellation);

        // nemovitosti
        var realEstates = _dbContext
            .Loans2RealEstates
            .Include(t => t.RealEstate)
            .AsNoTracking()
            .Where(t => t.UverId == loan.Id)
            .Select(t => new LoanRealEstate
            {
                RealEstatePurchaseTypeId = t.UcelKod,
                RealEstateTypeId = t.NemovitostId
            })
            .ToList();

        var statements = await _dbContext.Loans2Statements
            .AsNoTracking()
            .Where(t => t.Id == loan.Id)
            .FirstOrDefaultAsync(cancellation);
        if (statements is not null)
        {
            mortgage.Statement.Address = new()
            {
                Street = statements.Ulice ?? "",
                StreetNumber = statements.CisloDomu4 ?? "",
                HouseNumber = statements.CisloDomu2 ?? "",
                Postcode = statements.Psc ?? "",
                City = statements.Mesto ?? "",
                AddressPointId = statements.StatPodkategorie ?? "",
                CountryId = statements.ZemeId
            };
        }

        if (realEstates is not null && realEstates.Any())
        {
            // zjistit zajisteni
            var collateral = await _dbContext.Collaterals
                .AsNoTracking()
                .Where(t => t.UverId == loan.Id)
                .Select(t => new { t.NemovitostId })
                .ToListAsync(cancellation);
            collateral.ForEach(t =>
            {
                var n = realEstates.FirstOrDefault(x => x.RealEstateTypeId == t.NemovitostId);
                if (n is not null)
                {
                    n.IsCollateral = true;
                }
            });

            mortgage.LoanRealEstates.AddRange(realEstates);
        }

        // duvody
        var purposes = await _repository.GetPurposes(request.ProductId, cancellation);
        if (purposes is not null)
        {
            mortgage.LoanPurposes.AddRange(purposes.Select(t => new LoanPurpose
            {
                LoanPurposeId = t.UcelUveruId,
                Sum = t.SumaUcelu
            }));
        }

        return new GetMortgageResponse { Mortgage = mortgage };
    }

}