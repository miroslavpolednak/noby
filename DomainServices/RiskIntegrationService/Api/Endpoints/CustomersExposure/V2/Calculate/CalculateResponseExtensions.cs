using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;
using _CB = DomainServices.CodebookService.Contracts.Endpoints;
using DomainServices.RiskIntegrationService.Contracts.Shared;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2.Calculate;

internal static class CalculateResponseExtensions
{
    public static async Task<_V2.CustomersExposureCalculateResponse> ToServiceResponse(
        this _C4M.LoanApplicationRelatedExposureResult response, 
        CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService, 
        CancellationToken cancellation)
    {
        var customerRoles = await _codebookService.CustomerRoles(cancellation);
        var obligationLaExposures = await _codebookService.ObligationLaExposures(cancellation);

        return new _V2.CustomersExposureCalculateResponse
        {
            Customers = response.LoanApplicationCounterparty.Select(t => new _V2.CustomersExposureCustomer
            {
                //CbcbRegisterCalled = t.CbcbRegiterCalled.GetValueOrDefault(),//TODO c4m ma spatne datovy typ=ma byt bool. Ted vraceji nahodne 0 nebo false.
                CbcbReportId = t.CbcbReportId,
                CustomerRoleId = customerRoles.FirstOrDefault(c => c.RdmCode == t.RoleCode)?.Id,
                InternalCustomerId = t.LoanApplicationCounterpartyId,
                PrimaryCustomerId = t.CustomerId,
                ExistingKBGroupNaturalPersonExposures = t.ExistingKBGroupNaturalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                ExistingKBGroupJuridicalPersonExposures = t.ExistingKBGroupJuridicalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                RequestedKBGroupNaturalPersonExposures = t.RequestedKBGroupNaturalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                RequestedKBGroupJuridicalPersonExposures = t.RequestedKBGroupJuridicalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                ExistingCBCBNaturalPersonExposureItem = t.ExistingCBCBNaturalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                ExistingCBCBJuridicalPersonExposureItem = t.ExistingCBCBJuridicalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                RequestedCBCBNaturalPersonExposureItem = t.RequestedCBCBNaturalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList(),
                RequestedCBCBJuridicalPersonExposureItem = t.RequestedCBCBJuridicalPersonExposureItem?.Select(x => x.ToServiceResponse(customerRoles, obligationLaExposures)).ToList()
            }).ToList(),
            ExposureSummary = response.ExposureSummary?.Select(t => t.ToServiceResponse()).ToList()
        };
    }

    public static _V2.CustomersExposureSummary ToServiceResponse(this _C4M.ExposureSummaryForApproval item)
        => new _V2.CustomersExposureSummary
        {
            TotalExistingExposureKB = item.TotalExistingExposureKB?.Value,
            TotalExistingExposureKBNaturalPerson = item.TotalExistingExposureKBNonPurpose?.Value,
            TotalExistingExposureKBNonPurpose = item.TotalExistingExposureKBNonPurpose?.Value,
            TotalExistingExposureUnsecured = item.TotalExistingExposureUnsecured?.Value
        };

    public static _V2.CustomersExposureExistingKBGroupItem ToServiceResponse(this _C4M.ExistingKBGroupExposureItem item, List<_CB.CustomerRoles.CustomerRoleItem> customerRoles, List<_CB.ObligationLaExposures.ObligationLaExposureItem> obligationLaExposures)
        => new _V2.CustomersExposureExistingKBGroupItem
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
            IsSecured = item.IsSecured.GetValueOrDefault(),
            ContractDate = item.ContractDate?.DateTime,
            MaturityDate = item.MaturityDate?.DateTime
        };

    public static _V2.CustomersExposureRequestedKBGroupItem ToServiceResponse(this _C4M.RequestedKBGroupExposureItem item, List<_CB.CustomerRoles.CustomerRoleItem> customerRoles, List<_CB.ObligationLaExposures.ObligationLaExposureItem> obligationLaExposures)
        => new _V2.CustomersExposureRequestedKBGroupItem
        {
            RiskBusinessCaseId = item.RiskBusinessCaseId,
            LoanType = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.Id,
            LoanTypeCategory = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.ObligationTypeId,
            CustomerRoleId = customerRoles.FirstOrDefault(c => c.RdmCode == item.CustomerRoleCode)?.Id,
            LoanAmount = item.LoanAmount?.Value,
            InstallmentAmount = item.InstallmentAmount?.Value,
            StatusCode = item.StatusCode,
            IsSecured = item.IsSecured.GetValueOrDefault()
        };

    public static _V2.CustomersExposureExistingCBCBItem ToServiceResponse(this _C4M.ExistingCBCBExposureItem item, List<_CB.CustomerRoles.CustomerRoleItem> customerRoles, List<_CB.ObligationLaExposures.ObligationLaExposureItem> obligationLaExposures)
        => new _V2.CustomersExposureExistingCBCBItem
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
            KbGroupInstanceCode = item.KbGroupInstanceCode,
        };

    public static _V2.CustomersExposureRequestedCBCBItem ToServiceResponse(this _C4M.RequestedCBCBExposureItem item, List<_CB.CustomerRoles.CustomerRoleItem> customerRoles, List<_CB.ObligationLaExposures.ObligationLaExposureItem> obligationLaExposures)
        => new _V2.CustomersExposureRequestedCBCBItem
        {
            CbcbContractId = item.CbcbContractId,
            CustomerRoleId = customerRoles.FirstOrDefault(c => c.RdmCode == item.CustomerRoleCode)?.Id,
            LoanType = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.Id,
            LoanTypeCategory = obligationLaExposures.FirstOrDefault(t => t.RdmCode == item.LoanType)?.ObligationTypeId,
            MaturityDate = item.MaturityDate?.DateTime,
            LoanAmount = item.LoanAmount?.Value,
            InstallmentAmount = item.InstallmentAmount?.Value,
            KbGroupInstanceCode = item.KbGroupInstanceCode,
            CbcbDataLastUpdate = item.CbcbDataLastUpdate?.DateTime
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
