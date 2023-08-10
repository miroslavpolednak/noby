namespace NOBY.Services.RealEstateValuationType;

public interface IRealEstateValuationTypeService
{
    Task<List<RealEstateValuationTypes>> GetAllowedTypes(int realEstateValuationId, long caseId, CancellationToken cancellationToken);
}
