using CIS.Core.ErrorCodes;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int ResourceIdentifierIdIsEmpty = 17012;
    public const int ServiceResponseDeserializationException = 17100;
    public const int GeneralServiceResponseValidationError = 17101;
    public const int C4MRBCWaitingForCommitment = 17102;
    public const int C4MCustomerNotFound = 17103;
    public const int C4MCorrectStateRule = 17104;


    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { ResourceIdentifierIdIsEmpty, "ResourceIdentifier.Id not found (User {PropertyValue})." },
            { ServiceResponseDeserializationException, "C4M response cannot be processed (CisExtServiceResponseDeserializationException)." },
            { GeneralServiceResponseValidationError, "General C4M Bad Request response: {PropertyValue}" },
            { C4MRBCWaitingForCommitment, "{PropertyValue}" },
            { C4MCustomerNotFound, "{PropertyValue}" },
            { C4MCorrectStateRule, "{PropertyValue}" }
        });

        return Messages;
    }
}
