﻿using SharedTypes.GrpcTypes;
using _SA = DomainServices.SalesArrangementService.Contracts;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal static class UpdateParametersExtensions
{
    public static _SA.SalesArrangementParametersCustomerChange3602 ToDomainService(this SalesArrangementUpdateParametersCustomerChange3602 parameters, _SA.SalesArrangementParametersCustomerChange3602 originalParameters)
    {
        return new _SA.SalesArrangementParametersCustomerChange3602
        {
            HouseholdId = originalParameters.HouseholdId,
            IsSpouseInDebt = parameters.IsSpouseInDebt
        };
    }

    public static _SA.SalesArrangementParametersMortgage ToDomainService(this SalesArrangementSharedParametersMortgage parameters, _SA.SalesArrangementParametersMortgage originalParameter)
    {
        var model = new _SA.SalesArrangementParametersMortgage
        {
            ContractSignatureTypeId = parameters.ContractSignatureTypeId,
            ExpectedDateOfDrawing = parameters.ExpectedDateOfDrawing,
            IncomeCurrencyCode = parameters.IncomeCurrencyCode,
            ResidencyCurrencyCode = parameters.ResidencyCurrencyCode,
            Agent = parameters.Agent,
            Comment = originalParameter.Comment,
            FirstSignatureDate = originalParameter.FirstSignatureDate
        };

        if (parameters.LoanRealEstates is not null)
            model.LoanRealEstates.AddRange(parameters.LoanRealEstates.Select(x => new _SA.SalesArrangementParametersMortgage.Types.LoanRealEstate
            {
                IsCollateral = x.IsCollateral,
                RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                RealEstateTypeId = x.RealEstateTypeId
            }));

        return model;
    }

    public static _SA.SalesArrangementParametersDrawing ToDomainService(this SalesArrangementSharedParametersDrawing parameters)
    {
        var model = new _SA.SalesArrangementParametersDrawing()
        {
            DrawingDate = parameters.DrawingDate,
            IsImmediateDrawing = parameters.IsImmediateDrawing,
            RepaymentAccount = parameters.RepaymentAccount is null ? null : new()
            {
                BankCode = parameters.RepaymentAccount.AccountBankCode,
                IsAccountNumberMissing = parameters.RepaymentAccount.IsAccountNumberMissing,
                Number = parameters.RepaymentAccount.AccountNumber,
                Prefix = parameters.RepaymentAccount.AccountPrefix
            },
            Agent = new()
            {
                IsActive = parameters.Agent?.IsActive ?? false,
                DateOfBirth = parameters.Agent?.DateOfBirth,
                FirstName = parameters.Agent?.FirstName,
                LastName = parameters.Agent?.LastName,
                IdentificationDocument = parameters.Agent?.IdentificationDocument?.IdentificationDocumentTypeId is null ? null : new()
                {
                    Number = parameters.Agent.IdentificationDocument.Number,
                    IdentificationDocumentTypeId = parameters.Agent.IdentificationDocument.IdentificationDocumentTypeId.Value
                }
            }
        };

        if (parameters.Applicant?.Any() ?? false)
        {
            model.Applicant.AddRange(parameters.Applicant.Select(identity => (Identity)identity));
        }

        if (parameters.PayoutList is not null)
            model.PayoutList.AddRange(parameters.PayoutList?.Select(x => new _SA.SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingPayoutList
            {
                ProductObligationId = x.ProductObligationId,
                Order = x.Order,
                SpecificSymbol = x.SpecificSymbol,
                AccountNumber = x.AccountNumber,
                ConstantSymbol = x.ConstantSymbol,
                BankCode = x.AccountBankCode,
                DrawingAmount = x.DrawingAmount,
                VariableSymbol = x.VariableSymbol,
                PayoutTypeId = x.PayoutTypeId ?? 0,
                PrefixAccount = x.AccountPrefix
            }));

        return model;
    }

    public static _SA.SalesArrangementParametersGeneralChange ToDomainService(this SalesArrangementUpdateParametersGeneralChange parameters, _SA.SalesArrangementParametersGeneralChange? originalParameter)
    {
        var model = new _SA.SalesArrangementParametersGeneralChange()
        {
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
                ExtensionDrawingDateToByMonths = parameters.DrawingDateTo.ExtensionDrawingDateToByMonths,
                IsDrawingDateEarlier = parameters.DrawingDateTo?.IsDrawingDateEarlier ?? false
            },
            RepaymentAccount = new()
            {
                IsActive = parameters.RepaymentAccount.IsActive,
                AgreedBankCode = originalParameter?.RepaymentAccount?.AgreedBankCode,
                AgreedNumber = originalParameter?.RepaymentAccount?.AgreedNumber,
                AgreedPrefix = originalParameter?.RepaymentAccount?.AgreedPrefix,
                BankCode = parameters.RepaymentAccount.AccountBankCode,
                Number = parameters.RepaymentAccount.AccountNumber,
                OwnerDateOfBirth = parameters.RepaymentAccount.OwnerDateOfBirth,
                OwnerFirstName = parameters.RepaymentAccount.OwnerFirstName,
                OwnerLastName = parameters.RepaymentAccount.OwnerLastName,
                Prefix = parameters.RepaymentAccount.AccountPrefix
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

        if (parameters.Applicant?.Any() ?? false)
        {
            model.Applicant.AddRange(parameters.Applicant.Select(t => (Identity)t));
        }

        if (parameters.LoanRealEstate?.LoanRealEstates != null)
            model.LoanRealEstate.LoanRealEstates.AddRange(parameters.LoanRealEstate.LoanRealEstates.Select(t => new _SA.SalesArrangementParametersGeneralChange.Types.LoanRealEstatesItem
            {
                RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                RealEstateTypeId = t.RealEstateTypeId
            }));

        return model;
    }

    public static _SA.SalesArrangementParametersHUBN ToDomainService(this SalesArrangementUpdateParametersHubn parameters, _SA.SalesArrangementParametersHUBN? originalParameter)
    {
        var model = new _SA.SalesArrangementParametersHUBN()
        {
            CollateralIdentification = new()
            {
                RealEstateIdentification = parameters.CollateralIdentification?.RealEstateIdentification ?? ""
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
                AgreedExpectedDateOfDrawing = originalParameter?.ExpectedDateOfDrawing?.AgreedExpectedDateOfDrawing,
                NewExpectedDateOfDrawing = parameters.ExpectedDateOfDrawing?.NewExpectedDateOfDrawing
            },
            DrawingDateTo = new()
            {
                IsActive = parameters.DrawingDateTo?.IsActive ?? false,
                AgreedDrawingDateTo = originalParameter?.DrawingDateTo?.AgreedDrawingDateTo,
                ExtensionDrawingDateToByMonths = parameters.DrawingDateTo?.ExtensionDrawingDateToByMonths,
                IsDrawingDateEarlier = parameters.DrawingDateTo?.IsDrawingDateEarlier ?? false
            },
            CommentToChangeRequest = new()
            {
                IsActive = parameters.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = parameters.CommentToChangeRequest?.GeneralComment
            }
        };

        if (parameters.Applicant?.Any() ?? false)
        {
            model.Applicant.AddRange(parameters.Applicant.Select(t => (Identity)t));
        }

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

    public static _SA.SalesArrangementParametersCustomerChange ToDomainService(this SalesArrangementUpdateParametersCustomerChange parameters, _SA.SalesArrangementParametersCustomerChange? originalParameter)
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
                BankCode = parameters.RepaymentAccount?.AccountBankCode,
                Number = parameters.RepaymentAccount?.AccountNumber,
                Prefix = parameters.RepaymentAccount?.AccountPrefix
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
                    Identity = t.Identity ?? new SharedTypesCustomerIdentity(),
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
