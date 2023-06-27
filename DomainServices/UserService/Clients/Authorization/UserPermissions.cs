namespace DomainServices.UserService.Clients.Authorization;

public enum UserPermissions : int
{
    UC_getWflSigningDocuments = 100,
    UC_getWflSigningAttachments = 101,
    FEAPI_IdentifyCase = 110,
    APPLICATION_BasicAccess = 201,
    DASHBOARD_CreateNewCase = 202,
    DASHBOARD_ViewOwnedCases = 203,
    DASHBOARD_SearchOwnedCases = 204,
    DASHBOARD_FilterOwnedCases = 205,
    DASHBOARD_AccessToOwnedCase = 206,
    DASHBOARD_ViewAllCases = 207,
    DASHBOARD_SearchAllCases = 208,
    DASHBOARD_FilterAllCases = 209,
    DASHBOARD_AccessAllCases = 210,
    LOANMODELING_EmployeeMortgageAccess = 211,
    CASEDETAIL_BasicAccess = 212,
    SALES_ARRANGEMENT_Access = 213,
    CASEDETAIL_DOCUMENT_ViewDocument = 214,
    CASEDETAIL_DOCUMENT_ViewDocumentDetail = 215,
    CASEDETAIL_DOCUMENT_DownloadDocument = 216,
    CASEDETAIL_DOCUMENT_InsertDocument = 217,
    CASEDETAIL_APPLICANT_ViewPersonInfo = 218,
    CLIENT_IdentifyPerson = 219,
    CLIENT_SearchPerson = 220
}
