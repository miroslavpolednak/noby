﻿namespace DomainServices.UserService.Clients.Authorization;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1008 // Enums should have zero value

public enum UserPermissions : int
{
    WFL_TASK_DETAIL_PaperSigningDocuments = 100,
    WFL_TASK_DETAIL_SigningAttachments = 101,
    WFL_TASK_DETAIL_DigitalSigningDocuments = 102,
    DASHBOARD_IdentifyCase = 110,
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
    CLIENT_SearchPerson = 220,
    SIGNING_DOCUMENT_UploadDrawingDocument = 221,
    CASE_ViewAfterDrawing = 222,
    DASHBOARD_SearchCases = 224,
    CLIENT_Modify = 226,
    SCORING_Perform = 227,
    SALES_ARRANGEMENT_Send = 228,
    CASE_Cancel = 229,
    REALESTATE_VALUATION_Manage = 230,
    WFL_TASK_DETAIL_OtherManage = 231,
    WFL_TASK_DETAIL_SigningManage = 232,
    CHANGE_REQUESTS_Access = 233,
    WFL_TASK_DETAIL_OtherView = 234,
    WFL_TASK_DETAIL_SigningView = 235,
    DOCUMENT_SIGNING_Manage = 237,
    DOCUMENT_SIGNING_DownloadWorkflowDocument = 238,
    CLIENT_EXPOSURE_Perform = 239,
    CLIENT_EXPOSURE_DisplayRequestedExposure = 240,
    WFL_TASK_DETAIL_RefinancingOtherManage = 241,
    REFINANCING_Manage = 242,
    WFL_TASK_DETAIL_RefinancingSigningManage = 243,
    WFL_TASK_DETAIL_RefinancingOtherView = 244,
    WFL_TASK_DETAIL_RefinancingSigningView = 245,
    DOCUMENT_SIGNING_RefinancingManage = 246,
    CHANGE_REQUESTS_RefinancingAccess = 247,
    SALES_ARRANGEMENT_RefinancingAccess = 248,
    ADMIN_FeBannersManage = 249
}
