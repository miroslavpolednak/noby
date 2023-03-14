using NOBY.Api.Endpoints.SalesArrangement.Dto;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal static class UpdateParametersExtensions
{
    public static _SA.SalesArrangementParametersMortgage ToDomainService(this ParametersMortgage parameters)
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

    public static _SA.SalesArrangementParametersDrawing ToDomainService(this ParametersDrawing parameters)
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
            Agent = new()
            {
                IsActive = parameters.Agent?.IsActive ?? false,
                DateOfBirth = parameters.Agent?.DateOfBirth,
                FirstName = parameters.Agent?.FirstName,
                LastName = parameters.Agent?.LastName,
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

    public static _SA.SalesArrangementParametersGeneralChange ToDomainService(this Dto.GeneralChangeUpdate parameters, _SA.SalesArrangementParametersGeneralChange? originalParameter)
    {
        var model = new _SA.SalesArrangementParametersGeneralChange()
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
                AgreedPaymentDay = originalParameter?.PaymentDay?.AgreedPaymentDay ?? 0,
                NewPaymentDay = parameters.PaymentDay.NewPaymentDay
            },
            DrawingDateTo = new()
            {
                IsActive = parameters.DrawingDateTo.IsActive,
                AgreedDrawingDateTo = originalParameter?.DrawingDateTo?.AgreedDrawingDateTo,
                CommentToDrawingDateTo = parameters.DrawingDateTo.CommentToDrawingDateTo,
                ExtensionDrawingDateToByMonths = parameters.DrawingDateTo.ExtensionDrawingDateToByMonths
            },
            RepaymentAccount = new()
            {
                IsActive = parameters.RepaymentAccount.IsActive,
                AgreedBankCode = originalParameter?.RepaymentAccount?.AgreedBankCode,
                AgreedNumber = originalParameter?.RepaymentAccount?.AgreedNumber,
                AgreedPrefix = originalParameter?.RepaymentAccount?.AgreedPrefix,
                BankCode = parameters.RepaymentAccount.BankCode,
                Number = parameters.RepaymentAccount.Number,
                OwnerDateOfBirth = parameters.RepaymentAccount.OwnerDateOfBirth,
                OwnerFirstName = parameters.RepaymentAccount.OwnerFirstName,
                OwnerLastName = parameters.RepaymentAccount.OwnerLastName,
                Prefix = parameters.RepaymentAccount.Prefix
            },
            LoanPaymentAmount = new()
            {
                IsActive = parameters.LoanPaymentAmount.IsActive,
                NewLoanPaymentAmount = parameters.LoanPaymentAmount.NewLoanPaymentAmount,
                ActualLoanPaymentAmount = originalParameter?.LoanPaymentAmount?.ActualLoanPaymentAmount,
                ConnectionExtraordinaryPayment = parameters.LoanPaymentAmount.ConnectionExtraordinaryPayment
            },
            DueDate = new()
            {
                IsActive = parameters.DueDate.IsActive,
                ActualLoanDueDate = originalParameter?.DueDate?.ActualLoanDueDate,
                ConnectionExtraordinaryPayment = parameters.DueDate.ConnectionExtraordinaryPayment,
                NewLoanDueDate = parameters.DueDate.NewLoanDueDate
            },
            LoanRealEstate = new _SA.SalesArrangementParametersGeneralChange.Types.LoanRealEstateObject
            {
                IsActive = parameters.LoanRealEstate.IsActive
            },
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

        if (parameters.LoanRealEstate?.LoanRealEstates != null)
            model.LoanRealEstate.LoanRealEstates.AddRange(parameters.LoanRealEstate.LoanRealEstates.Select(t => new _SA.SalesArrangementParametersGeneralChange.Types.LoanRealEstatesItem
            {
                RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                RealEstateTypeId = t.RealEstateTypeId
            }));

        return model;
    }

    public static _SA.SalesArrangementParametersHUBN ToDomainService(this Dto.HUBNUpdate parameters, _SA.SalesArrangementParametersHUBN? originalParameter)
    {
        var model = new _SA.SalesArrangementParametersHUBN()
        {
            Applicant = parameters.Applicant,
            CollateralIdentification = new()
            {
                RealEstateIdentification = parameters.CollateralIdentification?.RealEstateIdentification
            },
            LoanAmount = new()
            {
                ChangeAgreedLoanAmount = parameters.LoanAmount?.ChangeAgreedLoanAmount ?? false,
                PreserveAgreedLoanDueDate = parameters.LoanAmount?.PreserveLoanDueDate ?? false,
                PreserveAgreedLoanPaymentAmount = parameters.LoanAmount?.PreserveAgreedPaymentAmount ?? false,
                RequiredLoanAmount = parameters.LoanAmount?.RequiredLoanAmount,
                AgreedLoanAmount = originalParameter?.LoanAmount?.AgreedLoanAmount,
                AgreedLoanDueDate = originalParameter?.LoanAmount?.AgreedLoanDueDate,
                AgreedLoanPaymentAmount = originalParameter?.LoanAmount?.AgreedLoanPaymentAmount
            },
            ExpectedDateOfDrawing = new()
            {
                IsActive = parameters.ExpectedDateOfDrawing?.IsActive ?? false,
                AgreedExpectedDateOfDrawing = originalParameter.ExpectedDateOfDrawing?.AgreedExpectedDateOfDrawing,
                NewExpectedDateOfDrawing = parameters.ExpectedDateOfDrawing?.NewExpectedDateOfDrawing
            },
            DrawingDateTo = new()
            {
                IsActive = parameters.DrawingDateTo?.IsActive ?? false,
                AgreedDrawingDateTo = originalParameter.DrawingDateTo?.AgreedDrawingDateTo,
                ExtensionDrawingDateToByMonths = parameters.DrawingDateTo?.ExtensionDrawingDateToByMonths
            },
            CommentToChangeRequest = new()
            {
                IsActive = parameters.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = parameters.CommentToChangeRequest?.GeneralComment
            }
        };

        if (parameters.LoanPurposes != null)
            model.LoanPurposes.AddRange(parameters.LoanPurposes.Select(t => new _SA.SalesArrangementParametersHUBN.Types.LoanPurposeItem
            {
                LoanPurposeId = t.Id,
                Sum = t.Sum
            }));
        if (parameters.LoanRealEstates != null)
            model.LoanRealEstates.AddRange(parameters.LoanRealEstates.Select(t => new _SA.SalesArrangementParametersHUBN.Types.LoanRealEstateItem
            {
                IsCollateral = t.IsCollateral,
                RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                RealEstateTypeId = t.RealEstateTypeId
            }));

        return model;
    }

    public static _SA.SalesArrangementParametersCustomerChange ToDomainService(this Dto.CustomerChangeUpdate parameters, _SA.SalesArrangementParametersCustomerChange? originalParameter)
    {
        var model = new _SA.SalesArrangementParametersCustomerChange()
        {
            Agent = new()
            {
                IsActive = parameters.Agent?.IsActive ?? false,
                ActualAgent = originalParameter?.Agent?.ActualAgent ?? "",
                NewAgent = parameters.Agent?.NewAgent ?? ""
            },
            RepaymentAccount = new()
            {
                IsActive = parameters.RepaymentAccount?.IsActive ?? false,
                OwnerDateOfBirth = parameters.RepaymentAccount?.OwnerDateOfBirth,
                OwnerFirstName = parameters.RepaymentAccount?.OwnerFirstName,
                OwnerLastName = parameters.RepaymentAccount?.OwnerLastName,
                BankCode = parameters.RepaymentAccount?.BankCode,
                Number = parameters.RepaymentAccount?.Number,
                Prefix = parameters.RepaymentAccount?.Prefix
            },
            CommentToChangeRequest = new()
            {
                IsActive = parameters.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = parameters.CommentToChangeRequest?.GeneralComment ?? ""
            }
        };

        if (parameters.Release is not null)
        {
            model.Release = new()
            {
                IsActive = parameters.Release.IsActive
            };
            if (parameters.Release.Customers is not null)
                model.Release.Customers.AddRange(parameters.Release.Customers.Select(t => new _SA.SalesArrangementParametersCustomerChange.Types.ReleaseCustomerObject
                {
                    Identity = t.Identity ?? new CIS.Foms.Types.CustomerIdentity(),
                    NaturalPerson = new()
                    {
                        FirstName = t.NaturalPerson?.FirstName ?? "",
                        LastName = t.NaturalPerson?.LastName ?? "",
                        DateOfBirth = t.NaturalPerson?.DateOfBirth
                    }
                }));
        }

        if (parameters.Add is not null)
        {
            model.Add = new()
            {
                IsActive = parameters.Add.IsActive
            };
            if (parameters.Add.Customers is not null)
                model.Add.Customers.AddRange(parameters.Add.Customers.Select(t => new _SA.SalesArrangementParametersCustomerChange.Types.AddCustomerObject
                {
                    Name = t.Name ?? "",
                    DateOfBirth = t.DateOfBirth
                }));
        }

        if (originalParameter?.Applicants is not null)
            model.Applicants.AddRange(originalParameter.Applicants);

        return model;
    }
}
