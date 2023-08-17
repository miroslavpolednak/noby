using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.UpdateRealEstateValuationDetail;

internal sealed class UpdateRealEstateValuationDetailHandler
    : IRequestHandler<UpdateRealEstateValuationDetailRequest, Empty>
{
    public async Task<Empty> Handle(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var realEstate = await _dbContext.RealEstateValuations
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        // Kontrola, zda na daném CaseId nedojde k porušení limitu na maximálně 3 Ocenění, která jsou zároveň objektem úvěru
        if (request.IsLoanRealEstate)
        {
            var existingRev = await _dbContext.RealEstateValuations
                .AsNoTracking()
                .Where(t => t.CaseId == realEstate.CaseId 
                    && t.RealEstateValuationId != request.RealEstateValuationId
                    && t.IsLoanRealEstate 
                    && !_stateIdsForValidation.Contains(t.ValuationStateId))
                .CountAsync(cancellationToken);
            if (existingRev > 2)
            {
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.MaxValuationsForCase);
            }
        }

        // general detail
        realEstate.Address = request.Address;
        realEstate.IsLoanRealEstate = request.IsLoanRealEstate;
        realEstate.RealEstateStateId = request.RealEstateStateId;
        realEstate.RealEstateSubtypeId = request.RealEstateSubtypeId;

        // zatim takto, v budoucnu refaktorovat s ohledem na dalsi nove pridana pole?
        if (request.Documents?.Any() ?? false)
        {
            realEstate.Documents = Newtonsoft.Json.JsonConvert.SerializeObject(request.Documents);
        }
        else
        {
            realEstate.Documents = null;
        }

        if (request.LoanPurposeDetails is null)
        {
            realEstate.LoanPurposeDetails = null!;
            realEstate.LoanPurposeDetailsBin = null!;
        }
        else
        {
            realEstate.LoanPurposeDetails = Newtonsoft.Json.JsonConvert.SerializeObject(request.LoanPurposeDetails);
            realEstate.LoanPurposeDetailsBin = request.LoanPurposeDetails.ToByteArray();
        }
        
        switch (request.SpecificDetailCase)
        {
            case UpdateRealEstateValuationDetailRequest.SpecificDetailOneofCase.HouseAndFlatDetails:
                realEstate.SpecificDetail = Newtonsoft.Json.JsonConvert.SerializeObject(request.HouseAndFlatDetails);
                realEstate.SpecificDetailBin = request.HouseAndFlatDetails.ToByteArray();
                break;

            case UpdateRealEstateValuationDetailRequest.SpecificDetailOneofCase.ParcelDetails:
                realEstate.SpecificDetail = Newtonsoft.Json.JsonConvert.SerializeObject(request.ParcelDetails);
                realEstate.SpecificDetailBin = request.ParcelDetails.ToByteArray();
                break;

            default:
                realEstate.SpecificDetail = null!;
                realEstate.SpecificDetailBin = null!;
                break;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }

    private static int[] _stateIdsForValidation = new[] { 4, 5 };

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public UpdateRealEstateValuationDetailHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
