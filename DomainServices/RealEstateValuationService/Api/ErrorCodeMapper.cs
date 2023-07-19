using CIS.Core.ErrorCodes;

namespace DomainServices.RealEstateValuationService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int RealEstateValuationIdEmpty = 22001;
    public const int RealEstateValuationNotFound = 22000;
    public const int CaseIdEmpty = 22002;
    public const int OrderIdEmpty = 22003;
    public const int RealEstateTypeIdNotFound = 22004;
    public const int ValuationStateIdNotFound = 22005;
    public const int ACVRealEstateTypeIsEmpty = 22006;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { CaseIdEmpty, "CaseId is empty" },
            { RealEstateValuationIdEmpty, "RealEstateValuationId is empty" },
            { OrderIdEmpty, "OrderId is empty" },
            { RealEstateValuationNotFound, "RealEstateValuation {PropertyValue} not found" },
            { RealEstateTypeIdNotFound, "RealEstateTypeId {PropertyValue} not found" },
            { ValuationStateIdNotFound, "ValuationStateId {PropertyValue} not found" },
            { ACVRealEstateTypeIsEmpty, "ACVRealEstateType is empty" }
        });

        return Messages;
    }
}
