using CIS.Core.ErrorCodes;

namespace DomainServices.SalesArrangementService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int SalesArrangementNotFound = 18000;
    public const int SalesArrangementTypeNotFound = 18005;
    public const int SalesArrangementStateNotFound = 18006;
    public const int AlreadyInSalesArrangementState = 18007;
    public const int CaseIdIsEmpty = 18008;
    public const int SalesArrangementTypeIdIsEmpty = 18009;
    public const int SalesArrangementIdIsEmpty = 18010;
    public const int OfferIdIsEmpty = 18011;
    public const int AlreadyLinkedToOffer = 18012;
    public const int SATypeNotSupported = 18013;
    public const int MortgageAgentIsNotEmpty = 18014;
    public const int DataObjectIsNotValid = 18015;
    public const int SalesArrangementCantDelete = 18016;
    public const int FlowSwitchesIsEmpty = 18017;
    public const int ProductSalesArrangementNotFound = 18018;
    public const int RealEstateTypeIdNotFound = 18037;
    public const int SendAndValidateForm1 = 18040;
    public const int SendAndValidateForm2 = 18041;
    public const int AlreadyLinkedToAnotherSA = 18057;
    public const int InvalidGuaranteeDateFrom = 18058;
    public const int IncomeCurrencyCodeNotFound = 18059;
    public const int ResidencyCurrencyCodeNotFound = 18060;
    public const int ContractSignatureTypeNotFound = 18061;
    public const int FormValidation1 = 18064;
    public const int FormValidation2 = 18065;
    public const int FormValidation9 = 18066;
    public const int FormValidation3 = 18067;
    public const int FormValidation4 = 18068;
    public const int FormValidation5 = 18069;
    public const int FormValidation6 = 18070;
    public const int FormValidation7 = 18071;
    public const int FormValidation8 = 18072;
    public const int FormValidation10 = 18073;
    public const int AgentNotFound = 18078;
    public const int SalesArrangementStateIsEmpty = 18079;
    public const int RepaymentAccountCantChange = 18081;
    public const int NotAllCustomersOnSaAreIdentified = 18085;
    public const int ApplicantIsNotSet = 18086;
    
    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { SalesArrangementNotFound, "Sales arrangement ID {PropertyValue} does not exist." },
            { SalesArrangementTypeNotFound, "SalesArrangementTypeId {PropertyValue} does not exist." },
            { SalesArrangementStateNotFound, "SalesArrangementState {PropertyValue} does not exist." },
            { AlreadyInSalesArrangementState, "SalesArrangement is already in state {PropertyValue}" },
            { CaseIdIsEmpty, "Case Id must be > 0" },
            { SalesArrangementTypeIdIsEmpty, "SalesArrangementTypeId must be > 0" },
            { SalesArrangementIdIsEmpty, "SalesArrangementId must be > 0" },
            { OfferIdIsEmpty, "OfferId must be > 0" },
            { AlreadyLinkedToOffer, "SalesArrangement {PropertyValue} is already linked to the same Offer" },
            { SATypeNotSupported, "SalesArrangementTypeId {PropertyValue} not supported" },
            { MortgageAgentIsNotEmpty, "Agent can not be set while creating Mortgage" },
            { DataObjectIsNotValid, "CreateSalesArrangementRequest.DataOneofCase is not valid for SalesArrangementTypeId={PropertyValue}" },
            { SalesArrangementCantDelete, "SalesArrangement cannot be updated/deleted in this state {PropertyValue}" },
            { FlowSwitchesIsEmpty, "FlowSwitches collection must not be empty" },
            { ProductSalesArrangementNotFound, "Product SA for CaseId {PropertyValue} not found" },
            { RealEstateTypeIdNotFound, "RealEstateTypeId not found" },
            { AlreadyLinkedToAnotherSA, "Offer {PropertyValue} is already linked to another SA" },
            { InvalidGuaranteeDateFrom, "Old offer GuaranteeDateFrom > than new GuaranteeDateFrom" },
            { IncomeCurrencyCodeNotFound, "IncomeCurrencyCode not found" },
            { ResidencyCurrencyCodeNotFound, "ResidencyCurrencyCode not found" },
            { ContractSignatureTypeNotFound, "ContractSignatureTypeId not found" },
            { FormValidation1, "Sales arrangement mandatory fields not provided [{PropertyValue}]." },
            { FormValidation2, "Sales Arrangement #{PropertyValue} is not linked to Offer" },
            { FormValidation9, "Income mandatory fields not provided [{PropertyValue}]." },
            { FormValidation3, "Sales arrangement customers [{PropertyValue}] don't contain both [KB,MP] identities." },
            { FormValidation4, "Sales arrangement contains duplicit household types [{PropertyValue}]." },
            { FormValidation5, "Sales arrangement must contain just one SharedTypes.Enums.HouseholdTypes.Main household." },
            { FormValidation6, "Sales arrangement contains households [{PropertyValue}] with CustomerOnSAId2 but without CustomerOnSAId1." },
            { FormValidation7, "Main household´s CustomerOnSAId1 not defined [{PropertyValue}]." },
            { FormValidation8, "Sales arrangement households contain duplicit customers [{PropertyValue}] on sales arrangement." },
            { FormValidation10, "Customers [{PropertyValue}] on sales arrangement don't correspond to customers on households." },
            { AgentNotFound, "Agent {PropertyValue} not found amoung customersOnSA for current SA" },
            { SalesArrangementStateIsEmpty, "SalesArrangement State must be > 0" },
            { RepaymentAccountCantChange, "Repayment account cannot be changed with IsAccountNumberMissing set to false" },
            { NotAllCustomersOnSaAreIdentified, "Some of the CustomersOnSa are not identified." },
            { ApplicantIsNotSet, "The Applicant is not set." },
        });

        return Messages;
    }
}
