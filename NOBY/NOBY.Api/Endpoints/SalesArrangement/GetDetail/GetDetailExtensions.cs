using NOBY.Api.Endpoints.SalesArrangement.Dto;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.GetDetail;

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
                ProductObligationId = x.ProductObligationId,   
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

    public static ParametersGeneralChange ToApiResponse(this _SA.SalesArrangementParametersGeneralChange model)
        => new()
        {
            Applicant = model.Applicant,
            Collateral = new CollateralObject
            {
                IsActive = model.Collateral.IsActive,
                AddLoanRealEstateCollateral = model.Collateral.AddLoanRealEstateCollateral,
                ReleaseLoanRealEstateCollateral = model.Collateral.ReleaseLoanRealEstateCollateral
            },
            PaymentDay = new PaymentDayObject
            {
                IsActive = model.PaymentDay.IsActive,
                AgreedPaymentDay = model.PaymentDay.AgreedPaymentDay,
                NewPaymentDay = model.PaymentDay.NewPaymentDay
            },
            DrawingDateTo = new DrawingDateToObject
            {
                IsActive = model.DrawingDateTo.IsActive,
                AgreedDrawingDateTo = model.DrawingDateTo.AgreedDrawingDateTo,
                CommentToDrawingDateTo = model.DrawingDateTo.CommentToDrawingDateTo,
                ExtensionDrawingDateToByMonths = model.DrawingDateTo.ExtensionDrawingDateToByMonths
            },
            PaymentAccount = new PaymentAccountObject
            {
                IsActive = model.PaymentAccount.IsActive,
                AgreedBankCode = model.PaymentAccount.AgreedBankCode,
                AgreedNumber = model.PaymentAccount.AgreedNumber,
                AgreedPrefix = model.PaymentAccount.AgreedPrefix,
                BankCode = model.PaymentAccount.BankCode,
                Number = model.PaymentAccount.Number,
                OwnerDateOfBirth = model.PaymentAccount.OwnerDateOfBirth,
                OwnerFirstName = model.PaymentAccount.OwnerFirstName,
                OwnerLastName = model.PaymentAccount.OwnerLastName
            },
            LoanPaymentAmount = new LoanPaymentAmountObject
            {
                IsActive = model.LoanPaymentAmount.IsActive,
                ActualLoanPaymentAmount = model.LoanPaymentAmount.ActualLoanPaymentAmount,
                NewLoanPaymentAmount = model.LoanPaymentAmount.NewLoanPaymentAmount,
                ConnectionExtraordinaryPayment = model.LoanPaymentAmount.ConnectionExtraordinaryPayment
            },
            DueDate = new DueDateObject
            {
                IsActive = model.DueDate.IsActive,
                ActualLoanDueDate = model.DueDate.ActualLoanDueDate,
                ConnectionExtraordinaryPayment = model.DueDate.ConnectionExtraordinaryPayment,
                NewLoanDueDate = model.DueDate.NewLoanDueDate
            },
            LoanRealEstate = new LoanRealEstateObject
            {
                IsActive = model.LoanRealEstate.IsActive,
                LoanRealEstates = model.LoanRealEstate.LoanRealEstates?.Select(t => new LoanRealEstateItem
                {
                    RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                    RealEstateTypeId = t.RealEstateTypeId
                }).ToList()
            },
            LoanPurpose = new LoanPurposeObject
            {
                IsActive = model.LoanPurpose.IsActive,
                LoanPurposesComment = model.LoanPurpose.LoanPurposesComment
            },
            DrawingAndOtherConditions = new DrawingAndOtherConditionsObject
            {
                IsActive = model.DrawingAndOtherConditions.IsActive,
                CommentToChangeContractConditions = model.DrawingAndOtherConditions.CommentToChangeContractConditions
            },
            CommentToChangeRequest = new CommentToChangeRequestObject
            {
                IsActive = model.CommentToChangeRequest.IsActive,
                GeneralComment = model.CommentToChangeRequest.GeneralComment
            }
        };

    public static ParametersHUBN ToApiResponse(this _SA.SalesArrangementParametersHUBN model)
        => new()
        {
            Applicant = model.Applicant,
            CollateralIdentification = new()
            {
                RealEstateIdentification = model.CollateralIdentification.RealEstateIdentification
            },
            LoanAmount = new()
            {
                ChangeAgreedLoanAmount = model.LoanAmount.ChangeAgreedLoanAmount,
                RequiredLoanAmount = model.LoanAmount.RequiredLoanAmount,
                PreserveAgreedPaymentAmount = model.LoanAmount.PreserveAgreedLoanPaymentAmount,
                PreserveLoanDueDate = model.LoanAmount.PreserveAgreedLoanDueDate
            },
            ExpectedDateOfDrawing = new()
            {
                IsActive = model.ExpectedDateOfDrawing.IsActive,
                NewExpectedDateOfDrawing = model.ExpectedDateOfDrawing.NewExpectedDateOfDrawing
            },
            DrawingDateTo = new()
            {
                IsActive = model.DrawingDateTo.IsActive,
                ExtensionDrawingDateToByMonths = model.DrawingDateTo.ExtensionDrawingDateToByMonths
            },
            CommentToChangeRequest = new()
            {
                IsActive = model.CommentToChangeRequest.IsActive,
                GeneralComment = model.CommentToChangeRequest.GeneralComment
            },
            LoanPurposes = model.LoanPurposes.Select(t => new LoanPurposeItem
            {
                Id = t.LoanPurposeId,
                Sum = t.Sum
            }).ToList(),
            LoanRealEstates = model.LoanRealEstates.Select(t => new LoanRealEstateItem2
            {
                IsCollateral = t.IsCollateral,
                RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                RealEstateTypeId = t.RealEstateTypeId
            }).ToList()
        };
}
