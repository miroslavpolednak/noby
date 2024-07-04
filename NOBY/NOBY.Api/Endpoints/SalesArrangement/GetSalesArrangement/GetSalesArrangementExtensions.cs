using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement;

internal static class GetSalesArrangementExtensions
{
    public static SalesArrangementSharedParametersCustomerChange3602 ToApiResponse(this _SA.SalesArrangementParametersCustomerChange3602 parameters)
        => new()
        {
            HouseholdId = parameters.HouseholdId,
            IsSpouseInDebt = parameters.IsSpouseInDebt
        };

    public static SalesArrangementSharedParametersMortgage ToApiResponse(this _SA.SalesArrangementParametersMortgage mortgage)
        => new()
        {
            ContractSignatureTypeId = mortgage.ContractSignatureTypeId,
            ExpectedDateOfDrawing = mortgage.ExpectedDateOfDrawing,
            IncomeCurrencyCode = mortgage.IncomeCurrencyCode,
            ResidencyCurrencyCode = mortgage.ResidencyCurrencyCode,
            Agent = mortgage.Agent,
            LoanRealEstates = mortgage.LoanRealEstates?.Select(x => new SalesArrangementSharedParametersLoanRealEstate
            {
                IsCollateral = x.IsCollateral,
                RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                RealEstateTypeId = x.RealEstateTypeId
            }).ToList()
        };

    public static SalesArrangementSharedParametersDrawing ToApiResponse(this _SA.SalesArrangementParametersDrawing model)
        => new()
        {
            DrawingDate = model.DrawingDate,
            IsImmediateDrawing = model.IsImmediateDrawing,
            Applicant = model.Applicant?.Select(t => (SharedTypesCustomerIdentity)t!)?.ToList(),
            PayoutList = model.PayoutList?.Select(x => new SalesArrangementSharedDrawingPayout
            {
                ProductObligationId = x.ProductObligationId,   
                Order = x.Order,
                SpecificSymbol = x.SpecificSymbol,
                AccountNumber = x.AccountNumber,
                ConstantSymbol = x.ConstantSymbol,
                AccountBankCode = x.BankCode,
                DrawingAmount = x.DrawingAmount,
                VariableSymbol = x.VariableSymbol,
                PayoutTypeId = x.PayoutTypeId,
                AccountPrefix = x.PrefixAccount
            }).ToList(),
            RepaymentAccount = model.RepaymentAccount is null ? null : new()
            {
                AccountBankCode = model.RepaymentAccount.BankCode,
                IsAccountNumberMissing = model.RepaymentAccount.IsAccountNumberMissing,
                AccountNumber = model.RepaymentAccount.Number,
                AccountPrefix = model.RepaymentAccount.Prefix
            },
            Agent = model.Agent is null ? new SalesArrangementSharedDrawingAgent() : new()
            {
                IsActive = model.Agent.IsActive,
                DateOfBirth = model.Agent.DateOfBirth,
                FirstName = model.Agent.FirstName,
                LastName = model.Agent.LastName,
                IdentificationDocument = model.Agent?.IdentificationDocument is null ? null : new()
                {
                    Number = model.Agent.IdentificationDocument.Number,
                    IdentificationDocumentTypeId = model.Agent.IdentificationDocument.IdentificationDocumentTypeId
                }
            }
        };

    public static SalesArrangementSharedParametersGeneralChange ToApiResponse(this _SA.SalesArrangementParametersGeneralChange model)
        => new()
        {
            Applicant = model.Applicant?.Select(t => (SharedTypesCustomerIdentity)t!)?.ToList(),
            Collateral = new()
            {
                IsActive = model.Collateral?.IsActive ?? false,
                AddLoanRealEstateCollateral = model.Collateral?.AddLoanRealEstateCollateral,
                ReleaseLoanRealEstateCollateral = model.Collateral?.ReleaseLoanRealEstateCollateral
            },
            PaymentDay = new()
            {
                IsActive = model.PaymentDay?.IsActive ?? false,
                AgreedPaymentDay = model.PaymentDay?.AgreedPaymentDay ?? 0,
                NewPaymentDay = model.PaymentDay?.NewPaymentDay
            },
            DrawingDateTo = new()
            {
                IsActive = model.DrawingDateTo?.IsActive ?? false,
                AgreedDrawingDateTo = model.DrawingDateTo?.AgreedDrawingDateTo,
                CommentToDrawingDateTo = model.DrawingDateTo?.CommentToDrawingDateTo,
                ExtensionDrawingDateToByMonths = model.DrawingDateTo?.ExtensionDrawingDateToByMonths,
                IsDrawingDateEarlier = model.DrawingDateTo?.IsDrawingDateEarlier ?? false
            },
            RepaymentAccount = new()
            {
                IsActive = model.RepaymentAccount?.IsActive ?? false,
                AgreedBankAccount = model.RepaymentAccount is null ? null : new SharedTypesBankAccount
                {
                    AccountPrefix = model.RepaymentAccount.AgreedPrefix,
                    AccountNumber = model.RepaymentAccount.AgreedNumber,
                    AccountBankCode = model.RepaymentAccount.AgreedBankCode,
                },
                AccountBankCode = model.RepaymentAccount?.BankCode,
                AccountNumber = model.RepaymentAccount?.Number,
                AccountPrefix = model.RepaymentAccount?.Prefix,
                OwnerDateOfBirth = model.RepaymentAccount?.OwnerDateOfBirth,
                OwnerFirstName = model.RepaymentAccount?.OwnerFirstName,
                OwnerLastName = model.RepaymentAccount?.OwnerLastName
            },
            LoanPaymentAmount = new()
            {
                IsActive = model.LoanPaymentAmount?.IsActive ?? false,
                ActualLoanPaymentAmount = model.LoanPaymentAmount?.ActualLoanPaymentAmount,
                NewLoanPaymentAmount = model.LoanPaymentAmount?.NewLoanPaymentAmount,
                ConnectionExtraordinaryPayment = model.LoanPaymentAmount?.ConnectionExtraordinaryPayment ?? false
            },
            DueDate = new()
            {
                IsActive = model.DueDate?.IsActive ?? false,
                ActualLoanDueDate = model.DueDate?.ActualLoanDueDate,
                ConnectionExtraordinaryPayment = model.DueDate?.ConnectionExtraordinaryPayment ?? false,
                NewLoanDueDate = model.DueDate?.NewLoanDueDate
            },
            LoanRealEstate = new()
            {
                IsActive = model.LoanRealEstate?.IsActive ?? false,
                LoanRealEstates = model.LoanRealEstate?.LoanRealEstates?.Select(t => new SalesArrangementSharedParametersLoanRealEstateBase
                {
                    RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                    RealEstateTypeId = t.RealEstateTypeId
                }).ToList()
            },
            LoanPurpose = new()
            {
                IsActive = model.LoanPurpose?.IsActive ?? false,
                LoanPurposesComment = model.LoanPurpose?.LoanPurposesComment
            },
            DrawingAndOtherConditions = new()
            {
                IsActive = model.DrawingAndOtherConditions?.IsActive ?? false,
                CommentToChangeContractConditions = model.DrawingAndOtherConditions?.CommentToChangeContractConditions
            },
            CommentToChangeRequest = new()
            {
                IsActive = model.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = model.CommentToChangeRequest?.GeneralComment
            }
        };

    public static SalesArrangementSharedParametersHubn ToApiResponse(this _SA.SalesArrangementParametersHUBN model)
        => new()
        {
            Applicant = model.Applicant?.Select(t => (SharedTypesCustomerIdentity)t!)?.ToList(),
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
                AgreedLoanAmount = model.LoanAmount?.AgreedLoanAmount ?? 0,
                AgreedLoanDueDate = model.LoanAmount?.AgreedLoanDueDate,
                AgreedLoanPaymentAmount = model.LoanAmount?.AgreedLoanPaymentAmount ?? 0
            },
            LoanPurposes = model.LoanPurposes?.Select(t => new SharedTypesLoanPurposeItem
            {
                Id = t.LoanPurposeId,
                Sum = t.Sum
            }).ToList(),
            LoanRealEstates = model.LoanRealEstates?.Select(t => new SalesArrangementSharedParametersLoanRealEstate
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
                ExtensionDrawingDateToByMonths = model.DrawingDateTo?.ExtensionDrawingDateToByMonths,
                IsDrawingDateEarlier = model.DrawingDateTo?.IsDrawingDateEarlier ?? false
            },
            CommentToChangeRequest = new()
            {
                IsActive = model.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = model.CommentToChangeRequest?.GeneralComment
            }
        };

    public static SalesArrangementSharedParametersCustomerChange ToApiResponse(this _SA.SalesArrangementParametersCustomerChange model)
        => new()
        {
            Applicants = model.Applicants?.Select(t => new SalesArrangementSharedCustomerChangeDetailApplicant
            {
                Identity = t.Identity?.Select(t => (SharedTypesCustomerIdentity)t!).ToList(),
                IdentificationDocument = t.IdentificationDocument is null ? null : new()
                {
                    IdentificationDocumentTypeId = t.IdentificationDocument.IdentificationDocumentTypeId,
                    Number = t.IdentificationDocument.Number
                },
                NaturalPerson = t.NaturalPerson is null ? null : new()
                {
                    FirstName = t.NaturalPerson.FirstName,
                    LastName = t.NaturalPerson.LastName,
                    DateOfBirth = t.NaturalPerson.DateOfBirth
                }
            }).ToList(),
            Release = new()
            {
                IsActive = model.Release?.IsActive ?? false,
                Customers = model.Release?.Customers is null ? null : model.Release.Customers.Select(t => new SalesArrangementSharedCustomerChangeDetailReleaseCustomer
                {
                    Identity = t.Identity,
                    NaturalPerson = new()
                    {
                        FirstName = t.NaturalPerson.FirstName,
                        LastName = t.NaturalPerson.LastName,
                        DateOfBirth = t.NaturalPerson.DateOfBirth
                    }
                }).ToList()
            },
            Add = new()
            {
                IsActive = model.Add?.IsActive ?? false,
                Customers = model.Add?.Customers is null ? null : model.Add.Customers.Select(t => new SalesArrangementSharedCustomerChangeDetailAddCustomer
                {
                    DateOfBirth = t.DateOfBirth,
                    Name = t.Name
                }).ToList()
            },
            Agent = new()
            {
                IsActive = model.Agent?.IsActive ?? false,
                ActualAgent = model.Agent?.ActualAgent ?? "",
                NewAgent = model.Agent?.NewAgent
            },
            RepaymentAccount = new()
            {
                IsActive = model.RepaymentAccount?.IsActive ?? false,
                AgreedBankAccount = model.RepaymentAccount is null ? null : new SharedTypesBankAccount
                {
                    AccountPrefix = model.RepaymentAccount.Prefix,
                    AccountNumber = model.RepaymentAccount.Number,
                    AccountBankCode = model.RepaymentAccount.BankCode,
                },
                AccountBankCode = model.RepaymentAccount?.BankCode,
                AccountNumber = model.RepaymentAccount?.Number,
                OwnerDateOfBirth = model.RepaymentAccount?.OwnerDateOfBirth,
                OwnerFirstName = model.RepaymentAccount?.OwnerFirstName,
                OwnerLastName = model.RepaymentAccount?.OwnerLastName,
                AccountPrefix = model.RepaymentAccount?.Prefix,
            },
            CommentToChangeRequest = new()
            {
                IsActive = model.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = model.CommentToChangeRequest?.GeneralComment
            }
        };
}
