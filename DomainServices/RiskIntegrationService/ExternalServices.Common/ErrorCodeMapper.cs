using CIS.Core.ErrorCodes;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int ResourceIdentifierIdIsEmpty = 17000;
    public const int ServiceResponseDeserializationException = 17001;
    public const int ResourceIdentifierDealerSchemeIsNull = 17012;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { ResourceIdentifierIdIsEmpty, "Can not find Id for ResourceIdentifier {PropertyValue}" },
            { ServiceResponseDeserializationException, "CisExtServiceResponseDeserializationException" },
            { ResourceIdentifierDealerSchemeIsNull, "GetResourceIdentifierInstanceForDealer() input parameter is null" }
        });

        return Messages;
    }
}
