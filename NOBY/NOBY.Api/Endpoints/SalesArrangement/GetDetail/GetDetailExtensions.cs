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
                IsActive = model.Collateral?.IsActive ?? false,
                AddLoanRealEstateCollateral = model.Collateral?.AddLoanRealEstateCollateral,
                ReleaseLoanRealEstateCollateral = model.Collateral?.ReleaseLoanRealEstateCollateral
            },
            PaymentDay = new PaymentDayObject
            {
                IsActive = model.PaymentDay?.IsActive ?? false,
                AgreedPaymentDay = model.PaymentDay?.AgreedPaymentDay,
                NewPaymentDay = model.PaymentDay?.NewPaymentDay
            },
            DrawingDateTo = new DrawingDateToObject
            {
                IsActive = model.DrawingDateTo?.IsActive ?? false,
                AgreedDrawingDateTo = model.DrawingDateTo?.AgreedDrawingDateTo,
                CommentToDrawingDateTo = model.DrawingDateTo?.CommentToDrawingDateTo,
                ExtensionDrawingDateToByMonths = model.DrawingDateTo?.ExtensionDrawingDateToByMonths
            },
            RepaymentAccount = new PaymentAccountObject
            {
                IsActive = model.RepaymentAccount?.IsActive ?? false,
                AgreedBankCode = model.RepaymentAccount?.AgreedBankCode,
                AgreedNumber = model.RepaymentAccount?.AgreedNumber,
                AgreedPrefix = model.RepaymentAccount?.AgreedPrefix,
                BankCode = model.RepaymentAccount?.BankCode,
                Number = model.RepaymentAccount?.Number,
                OwnerDateOfBirth = model.RepaymentAccount?.OwnerDateOfBirth,
                OwnerFirstName = model.RepaymentAccount?.OwnerFirstName,
                OwnerLastName = model.RepaymentAccount?.OwnerLastName
            },
            LoanPaymentAmount = new LoanPaymentAmountObject
            {
                IsActive = model.LoanPaymentAmount?.IsActive ?? false,
                ActualLoanPaymentAmount = model.LoanPaymentAmount?.ActualLoanPaymentAmount,
                NewLoanPaymentAmount = model.LoanPaymentAmount?.NewLoanPaymentAmount,
                ConnectionExtraordinaryPayment = model.LoanPaymentAmount?.ConnectionExtraordinaryPayment ?? false
            },
            DueDate = new DueDateObject
            {
                IsActive = model.DueDate?.IsActive ?? false,
                ActualLoanDueDate = model.DueDate?.ActualLoanDueDate,
                ConnectionExtraordinaryPayment = model.DueDate?.ConnectionExtraordinaryPayment ?? false,
                NewLoanDueDate = model.DueDate?.NewLoanDueDate
            },
            LoanRealEstate = new LoanRealEstateObject
            {
                IsActive = model.LoanRealEstate?.IsActive ?? false,
                LoanRealEstates = model.LoanRealEstate?.LoanRealEstates?.Select(t => new LoanRealEstateItem
                {
                    RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                    RealEstateTypeId = t.RealEstateTypeId
                }).ToList()
            },
            LoanPurpose = new LoanPurposeObject
            {
                IsActive = model.LoanPurpose?.IsActive ?? false,
                LoanPurposesComment = model.LoanPurpose?.LoanPurposesComment
            },
            DrawingAndOtherConditions = new DrawingAndOtherConditionsObject
            {
                IsActive = model.DrawingAndOtherConditions?.IsActive ?? false,
                CommentToChangeContractConditions = model.DrawingAndOtherConditions?.CommentToChangeContractConditions
            },
            CommentToChangeRequest = new CommentToChangeRequestObject
            {
                IsActive = model.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = model.CommentToChangeRequest?.GeneralComment
            }
        };

    public static ParametersHUBN ToApiResponse(this _SA.SalesArrangementParametersHUBN model)
        => new()
        {
            Applicant = model.Applicant,
            CollateralIdentification = new()
            {
                RealEstateIdentification = model.CollateralIdentification?.RealEstateIdentification
            },
            LoanAmount = new()
            {
                ChangeAgreedLoanAmount = model.LoanAmount?.ChangeAgreedLoanAmount ?? false,
                RequiredLoanAmount = model.LoanAmount?.RequiredLoanAmount,
                PreserveAgreedPaymentAmount = model.LoanAmount?.PreserveAgreedLoanPaymentAmount ?? false,
                PreserveLoanDueDate = model.LoanAmount?.PreserveAgreedLoanDueDate ?? false,
                AgreedLoanAmount = model.LoanAmount?.AgreedLoanAmount,
                AgreedLoanDueDate = model.LoanAmount?.AgreedLoanDueDate,
                AgreedLoanPaymentAmount = model.LoanAmount?.AgreedLoanPaymentAmount
            },
            LoanPurposes = model.LoanPurposes?.Select(t => new LoanPurposeItem
            {
                Id = t.LoanPurposeId,
                Sum = t.Sum
            }).ToList(),
            LoanRealEstates = model.LoanRealEstates?.Select(t => new LoanRealEstateItem2
            {
                IsCollateral = t.IsCollateral,
                RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                RealEstateTypeId = t.RealEstateTypeId
            }).ToList(),
            ExpectedDateOfDrawing = new()
            {
                IsActive = model.ExpectedDateOfDrawing?.IsActive ?? false,
                AgreedExpectedDateOfDrawing = model.ExpectedDateOfDrawing?.AgreedExpectedDateOfDrawing,
                NewExpectedDateOfDrawing = model.ExpectedDateOfDrawing?.NewExpectedDateOfDrawing
            },
            DrawingDateTo = new()
            {
                IsActive = model.DrawingDateTo?.IsActive ?? false,
                AgreedDrawingDateTo = model.DrawingDateTo?.AgreedDrawingDateTo,
                ExtensionDrawingDateToByMonths = model.DrawingDateTo?.ExtensionDrawingDateToByMonths
            },
            CommentToChangeRequest = new()
            {
                IsActive = model.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = model.CommentToChangeRequest?.GeneralComment
            }
        };
}
