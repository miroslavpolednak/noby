using Azure.Core;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal static class UpdateParametersExtensions
{
    public static _SA.SalesArrangementParametersMortgage ToDomainService(this Dto.ParametersMortgage parameters)
    {
        var model = new _SA.SalesArrangementParametersMortgage
        {
            ContractSignatureTypeId = parameters.ContractSignatureTypeId,
            ExpectedDateOfDrawing = parameters.ExpectedDateOfDrawing,
            IncomeCurrencyCode = parameters.IncomeCurrencyCode,
            ResidencyCurrencyCode = parameters.ResidencyCurrencyCode,
            Agent = parameters.Agent,
            AgentConsentWithElCom = parameters.AgentConsentWithElCom,
        };

        if (parameters.LoanRealEstates is not null)
            model.LoanRealEstates.AddRange(parameters.LoanRealEstates.Select(x => new _SA.SalesArrangementParametersMortgage.Types.LoanRealEstate
            {
                IsCollateral = x.IsCollateral,
                RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                RealEstateTypeId = x.RealEstateTypeId,
            }));

        return model;
    }

    public static _SA.SalesArrangementParametersDrawing ToDomainService(this Dto.ParametersDrawing parameters)
    {
        var model = new _SA.SalesArrangementParametersDrawing()
        {
            DrawingDate = parameters.DrawingDate,
            IsImmediateDrawing = parameters.IsImmediateDrawing,
            Applicant = parameters.Applicant,
            RepaymentAccount = parameters.RepaymentAccount is null ? null : new()
            {
                BankCode = parameters.RepaymentAccount.BankCode,
                IsAccountNumberMissing = parameters.RepaymentAccount.IsAccountNumberMissing,
                Number = parameters.RepaymentAccount.Number,
                Prefix = parameters.RepaymentAccount.Prefix
            },
            Agent = parameters.Agent is null ? null : new()
            {
                DateOfBirth = (DateTime?)parameters.Agent.DateOfBirth,
                FirstName = parameters.Agent.FirstName,
                LastName = parameters.Agent.LastName,
                IdentificationDocument = parameters.Agent?.IdentificationDocument is null ? null : new()
                {
                    Number = parameters.Agent.IdentificationDocument.Number,
                    IdentificationDocumentTypeId = parameters.Agent.IdentificationDocument.IdentificationDocumentTypeId
                }
            }
        };

        if (parameters.PayoutList is not null)
            model.PayoutList.AddRange(parameters.PayoutList?.Select(x => new _SA.SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingPayoutList
            {
                ProductObligationId = x.ProductObligationId,
                Order = x.Order,
                SpecificSymbolUcetKeSplaceni = x.SpecificSymbol,
                AccountNumber = x.AccountNumber,
                ConstantSymbol = x.ConstantSymbol,
                BankCode = x.BankCode,
                DrawingAmount = x.DrawingAmount,
                VariableSymbol = x.VariableSymbol,
                PayoutTypeId = x.PayoutTypeId ?? 0,
                PrefixAccount = x.PrefixAccount
            }));

        return model;
    }

    public static _SA.SalesArrangementParametersGeneralChange ToDomainService(this Dto.ParametersGeneralChange parameters)
    {
        return new()
        {
            Applicant = parameters.Applicant,
            Collateral = new()
            {
                IsActive = parameters.Collateral.IsActive,
                AddLoanRealEstateCollateral = parameters.Collateral.AddLoanRealEstateCollateral,
                ReleaseLoanRealEstateCollateral = parameters.Collateral.ReleaseLoanRealEstateCollateral
            },
            PaymentDay = new()
            {
                IsActive = parameters.PaymentDay.IsActive,
                AgreedPaymentDay = parameters.PaymentDay.AgreedPaymentDay,
                NewPaymentDay = parameters.PaymentDay.NewPaymentDay
            },
            DrawingDateTo = new()
            {
                IsActive = parameters.DrawingDateTo.IsActive,
                AgreedDrawingDateTo = parameters.DrawingDateTo.AgreedDrawingDateTo,
                CommentToDrawingDateTo = parameters.DrawingDateTo.CommentToDrawingDateTo,
                ExtensionByMonths = parameters.DrawingDateTo.ExtensionByMonths
            },
            PaymentAccount = new()
            {
                IsActive = parameters.PaymentAccount.IsActive,
                AgreedBankCode = parameters.PaymentAccount.AgreedBankCode,
                AgreedNumber = parameters.PaymentAccount.AgreedNumber,
                AgreedPrefix = parameters.PaymentAccount.AgreedPrefix,
                BankCode = parameters.PaymentAccount.BankCode,
                Number = parameters.PaymentAccount.Number,
                OwnerDateOfBirth = parameters.PaymentAccount.OwnerDateOfBirth,
                OwnerFirstName = parameters.PaymentAccount.OwnerFirstName,
                OwnerLastName = parameters.PaymentAccount.OwnerLastName,
                Prefix = parameters.PaymentAccount.Prefix
            },
            LoanPaymentAmount = new()
            {
                IsActive = parameters.LoanPaymentAmount.IsActive,
                NewLoanPaymentAmount = parameters.LoanPaymentAmount.NewLoanPaymentAmount,
                ActualLoanPaymentAmount = parameters.LoanPaymentAmount.ActualLoanPaymentAmount,
                ConnectionExtraordinaryPayment = parameters.LoanPaymentAmount.ConnectionExtraordinaryPayment
            },
            DueDate = new()
            {
                IsActive = parameters.DueDate.IsActive,
                ActualLoanDueDate = parameters.DueDate.ActualLoanDueDate,
                ConnectionExtraordinaryPayment = parameters.DueDate.ConnectionExtraordinaryPayment,
                NewLoanDueDate = parameters.DueDate.NewLoanDueDate
            },
            LoanRealEstate = null,
            LoanPurpose = new()
            {
                IsActive = parameters.LoanPurpose.IsActive,
                LoanPurposesComment = parameters.LoanPurpose.LoanPurposesComment
            },
            DrawingAndOtherConditions = new()
            {
                IsActive = parameters.DrawingAndOtherConditions.IsActive,
                CommentToChangeContractConditions = parameters.DrawingAndOtherConditions.CommentToChangeContractConditions
            },
            CommentToChangeRequest = new()
            {
                IsActive = parameters.CommentToChangeRequest.IsActive,
                GeneralComment = parameters.CommentToChangeRequest.GeneralComment
            }
        };
    }

    public static _SA.SalesArrangementParametersHUBN ToDomainService(this Dto.ParametersHUBN parameters)
    {
        return new()
        {

        };
    }
}
