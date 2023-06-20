using CIS.Core.ErrorCodes;

namespace DomainServices.RealEstateValuationService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int RealEstateValuationIdEmpty = 13000;
    public const int RealEstateValuationNotFound = 13000;
    public const int CaseIdEmpty = 13000;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { CaseIdEmpty, "CaseId is empty" },
            { RealEstateValuationIdEmpty, "RealEstateValuationId is empty" },
            { RealEstateValuationNotFound, "RealEstateValuation {PropertyValue} not found" }
        });

        return Messages;
    }
}
