using FOMS.Api.Endpoints.SalesArrangement.Dto;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

internal static class GetDetailExtensions
{
    public static ParametersMortgage ToApiResponse(this _SA.SalesArrangementParametersMortgage mortgage)
        => new()
        {
            ContractSignatureTypeId = mortgage.ContractSignatureTypeId,
            ExpectedDateOfDrawing = mortgage.ExpectedDateOfDrawing,
            IncomeCurrencyCode = mortgage.IncomeCurrencyCode,
            ResidencyCurrencyCode = mortgage.ResidencyCurrencyCode,
            Agent = mortgage.Agent,
            AgentConsentWithElCom = mortgage.AgentConsentWithElCom,
            LoanRealEstates = mortgage.LoanRealEstates is null ? null : mortgage.LoanRealEstates.Select(x => new LoanRealEstateDto
            {
                IsCollateral = x.IsCollateral,
                RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                RealEstateTypeId = x.RealEstateTypeId
            }).ToList()
        };

    public static ParametersDrawing ToApiResponse(this _SA.SalesArrangementParametersDrawing model)
        => new()
        {
            DrawingDate = model.DrawingDate,
            IsImmediateDrawing = model.IsImmediateDrawing,
            Applicant = model.Applicant,
            PayoutList = model.PayoutList?.Select(x => new ParametersDrawingPayout
            {
                Order = x.Order,
                SpecificSymbol = x.SpecificSymbolUcetKeSplaceni,
                AccountNumber = x.AccountNumber,
                ConstantSymbol = x.ConstantSymbol,
                BankCode = x.BankCode,
                DrawingAmount = x.DrawingAmount,
                VariableSymbol = x.VariableSymbol,
                PayoutTypeId = x.PayoutTypeId,
                PrefixAccount = x.PrefixAccount
            }).ToList(),
            RepaymentAccount = model.RepaymentAccount is null ? null : new()
            {
                BankCode = model.RepaymentAccount.BankCode,
                IsAccountNumberMissing = model.RepaymentAccount.IsAccountNumberMissing,
                Number = model.RepaymentAccount.Number,
                Prefix = model.RepaymentAccount.Prefix
            },
            Agent = model.Agent is null ? null : new()
            {
                DateOfBirth = (DateTime?)model.Agent.DateOfBirth,
                FirstName = model.Agent.FirstName,
                LastName = model.Agent.LastName,
                IdentificationDocument = model.Agent?.IdentificationDocument is null ? null : new()
                {
                    Number = model.Agent.IdentificationDocument.Number,
                    IdentificationDocumentTypeId = model.Agent.IdentificationDocument.IdentificationDocumentTypeId
                }
            }
        };
}
