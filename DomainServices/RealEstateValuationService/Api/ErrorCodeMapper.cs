using CIS.Core.ErrorCodes;

namespace DomainServices.RealEstateValuationService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int RealEstateValuationNotFound = 22000;
    public const int RealEstateValuationIdEmpty = 22001;
    public const int CaseIdEmpty = 22002;
    public const int OrderIdEmpty = 22003;
    public const int RealEstateTypeIdNotFound = 22004;
    public const int ValuationStateIdNotFound = 22005;
    public const int ACVRealEstateTypeIsEmpty = 22006;
    public const int DeedOfOwnershipDocumentIdIsEmpty = 22007;
    public const int DeedOfOwnershipDocumentNotFound = 22008;
    public const int RealEstateValuationAttachmentIdIsEmpty = 22009;
    public const int RealEstateValuationAttachmentNotFound = 22010;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { RealEstateValuationNotFound, "RealEstateValuation {PropertyValue} not found" },
            { RealEstateValuationIdEmpty, "RealEstateValuationId is empty" },
            { CaseIdEmpty, "CaseId is empty" },
            { OrderIdEmpty, "OrderId is empty" },
            { RealEstateTypeIdNotFound, "RealEstateTypeId {PropertyValue} not found" },
            { ValuationStateIdNotFound, "ValuationStateId {PropertyValue} not found" },
            { ACVRealEstateTypeIsEmpty, "ACVRealEstateType is empty" },
            { DeedOfOwnershipDocumentIdIsEmpty, "DeedOfOwnershipDocumentId is empty" },
            { DeedOfOwnershipDocumentNotFound, "DeedOfOwnershipDocumentId {PropertyValue} not found" },
            { RealEstateValuationAttachmentIdIsEmpty, "RealEstateValuationAttachmentId is empty" },
            { RealEstateValuationAttachmentNotFound, "RealEstateValuationAttachmentId {PropertyValue} not found" }
        });

        return Messages;
    }
}
