﻿using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3.Contracts;
using DomainServices.RiskIntegrationService.Contracts.Shared;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomerExposure.V2.Calculate;

internal static class CalculateResponseExtensions
{
    public static async Task<_V2.CustomerExposureCalculateResponse> ToServiceResponse(
        this _C4M.LoanApplicationRelatedExposureResult response, 
        CodebookService.Clients.ICodebookServiceClient _codebookService, 
        CancellationToken cancellation)
    {
        var customerRoles = await _codebookService.CustomerRoles(cancellation);
        var obligationLaExposures = await _codebookService.ObligationLaExposures(cancellation);

        return new _V2.CustomerExposureCalculateResponse
        {
            Customers = response.LoanApplicationCounterparty?.Select(t => new _V2.CustomerExposureCustomer
            {
                CbcbRegisterCalled = t.CbcbRegisterCalled,
                CbcbReportId = t.CbcbReportId,
                CustomerRoleId = customerRoles.FirstOrDefault(c => c.RdmCode == t.RoleCode)?.Id,
                InternalCustomerId = t.LoanApplicationCounterpartyId,
                PrimaryCustomerId = t.CustomerId.ToPrimaryCustomerId(),
                ExistingKBGroupNaturalPersonExposures = t.ExistingKBGroupNaturalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                ExistingKBGroupJuridicalPersonExposures = t.ExistingKBGroupJuridicalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                RequestedKBGroupNaturalPersonExposures = t.RequestedKBGroupNaturalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                RequestedKBGroupJuridicalPersonExposures = t.RequestedKBGroupJuridicalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                ExistingCBCBNaturalPersonExposureItem = t.ExistingCBCBNaturalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                ExistingCBCBJuridicalPersonExposureItem = t.ExistingCBCBJuridicalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                RequestedCBCBNaturalPersonExposureItem = t.RequestedCBCBNaturalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                RequestedCBCBJuridicalPersonExposureItem = t.RequestedCBCBJuridicalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList()
            }).ToList(),
            ExposureSummary = response.ExposureSummary.ToServiceResponse()
        };
    }

    public static _V2.CustomerExposureSummary ToServiceResponse(this _C4M.ExposureSummaryForApproval item)
        => new _V2.CustomerExposureSummary
        {
            TotalExistingExposureKB = item.TotalExistingExposureKB?.Value,
            TotalExistingExposureKBNaturalPerson = item.TotalExistingExposureKBNonPurpose?.Value,
            TotalExistingExposureKBNonPurpose = item.TotalExistingExposureKBNonPurpose?.Value,
            TotalExistingExposureUnsecured = item.TotalExistingExposureUnsecured?.Value
        };

    public static _V2.CustomerExposureExistingKBGroupItem ToServiceResponse(this _C4M.ExistingKBGroupExposureItem item, List<CustomerRolesResponse.Types.CustomerRoleItem> customerRoles, List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> obligationLaExposures)
        => new _V2.CustomerExposureExistingKBGroupItem
        {
            BankAccount = getBankAccountFromIdentifier(item.ProductId),
            LoanType = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.Id,
            LoanTypeCategory = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.ObligationTypeId,
            CustomerRoleId = customerRoles.FirstOrDefault(c => c.RdmCode == item.CustomerRoleCode)?.Id,
            LoanAmount = item.LoanAmount?.Value,
            DrawingAmount = item.DrawingAmount?.Value,
            LoanBalanceAmount = item.LoanBalanceAmount?.Value,
            LoanOffBalanceAmount = item.LoanOffBalanceAmount?.Value,
            LoanOnBalanceAmount = item.LoanOnBalanceAmount?.Value,
            ExposureAmount = item.ExposureAmount?.Value,
            InstallmentAmount = item.InstallmentAmount?.Value,
            IsSecured = item.IsSecured,
            ContractDate = item.ContractDate?.DateTime,
            MaturityDate = item.MaturityDate?.DateTime
        };

    public static _V2.CustomerExposureExistingKBGroupItem ToServiceResponse(this _C4M.ExistingKBGroupExposureFopItem item, List<CustomerRolesResponse.Types.CustomerRoleItem> customerRoles, List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> obligationLaExposures)
        => new _V2.CustomerExposureExistingKBGroupItem
        {
            BankAccount = getBankAccountFromIdentifier(item.ProductId),
            LoanType = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.Id,
            LoanTypeCategory = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.ObligationTypeId,
            CustomerRoleId = customerRoles.FirstOrDefault(c => c.RdmCode == item.CustomerRoleCode)?.Id,
            LoanAmount = item.LoanAmount?.Value,
            DrawingAmount = item.DrawingAmount?.Value,
            LoanBalanceAmount = item.LoanBalanceAmount?.Value,
            LoanOffBalanceAmount = item.LoanOffBalanceAmount?.Value,
            LoanOnBalanceAmount = item.LoanOnBalanceAmount?.Value,
            ExposureAmount = item.ExposureAmount?.Value,
            InstallmentAmount = item.InstallmentAmount?.Value,
            IsSecured = item.IsSecured,
            ContractDate = item.ContractDate?.DateTime,
            MaturityDate = item.MaturityDate?.DateTime
        };

    public static _V2.CustomerExposureRequestedKBGroupItem ToServiceResponse(this _C4M.RequestedKBGroupExposureItem item, List<CustomerRolesResponse.Types.CustomerRoleItem> customerRoles, List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> obligationLaExposures)
        => new _V2.CustomerExposureRequestedKBGroupItem
        {
            RiskBusinessCaseId = item.RiskBusinessCaseId,
            LoanType = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.Id,
            LoanTypeCategory = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.ObligationTypeId,
            CustomerRoleId = customerRoles.FirstOrDefault(c => c.RdmCode == item.CustomerRoleCode)?.Id,
            LoanAmount = item.LoanAmount?.Value,
            InstallmentAmount = item.InstallmentAmount?.Value,
            StatusCode = item.StatusCode,
            IsSecured = item.IsSecured,
            RequestDate = item.RequestDate?.DateTime,
            AppendixAccNbr = item.AppendixAccNbr,
            AppendixCode = item.AppendixCode
        };

    public static _V2.CustomerExposureExistingCBCBItem ToServiceResponse(this _C4M.ExistingCBCBExposureItem item, List<CustomerRolesResponse.Types.CustomerRoleItem> customerRoles, List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> obligationLaExposures)
        => new _V2.CustomerExposureExistingCBCBItem
        {
            CbcbContractId = item.CbcbContractId,
            CustomerRoleId = customerRoles.FirstOrDefault(c => c.RdmCode == item.CustomerRoleCode)?.Id,
            LoanType = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.Id,
            LoanTypeCategory = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.ObligationTypeId,
            MaturityDate = item.MaturityDate?.DateTime,
            LoanAmount = item.LoanAmount?.Value,
            InstallmentAmount = item.InstallmentAmount?.Value,
            ExposureAmount = item.ExposureAmount?.Value,
            ContractDate = item.ContractDate?.DateTime,
            CbcbDataLastUpdate = item.CbcbDataLastUpdate?.DateTime,
            KbGroupInstanceCode = item.KbGroupInstanceCode
        };

    public static _V2.CustomerExposureRequestedCBCBItem ToServiceResponse(this _C4M.RequestedCBCBExposureItem item, List<CustomerRolesResponse.Types.CustomerRoleItem> customerRoles, List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> obligationLaExposures)
        => new _V2.CustomerExposureRequestedCBCBItem
        {
            CbcbContractId = item.CbcbContractId,
            CustomerRoleId = customerRoles.FirstOrDefault(c => c.RdmCode == item.CustomerRoleCode)?.Id,
            LoanType = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.Id,
            LoanTypeCategory = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.ObligationTypeId,
            MaturityDate = item.MaturityDate?.DateTime,
            LoanAmount = item.LoanAmount?.Value,
            InstallmentAmount = item.InstallmentAmount?.Value,
            KbGroupInstanceCode = item.KbGroupInstanceCode,
            CbcbDataLastUpdate = item.CbcbDataLastUpdate?.DateTime,
            RequestDate = item.RequestDate?.DateTime
        };

    private static BankAccountDetail? getBankAccountFromIdentifier(string? identifier)
    {
        if (string.IsNullOrEmpty(identifier) || identifier.Length < 20) return null;
        return new()
        {
            BankCode = "0100",
            Number = identifier[^10..].TrimStart('0'),
            NumberPrefix = identifier[^16..^10].TrimStart('0')
        };
    }
}
