using DomainServices.SalesArrangementService.Contracts;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement;

internal static class GetSalesArrangementExtensions
{
    public static SalesArrangementParametersCustomerChange3602 ToApiResponse(this _SA.SalesArrangementParametersCustomerChange3602 parameters)
        => new()
        {
            HouseholdId = parameters.HouseholdId,
            IsSpouseInDebt = parameters.IsSpouseInDebt
        };

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
            Agent = model.Agent is null ? new ParametersDrawingAgent() : new()
            {
                IsActive = model.Agent.IsActive,
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

    public static Dto.GeneralChangeDetail ToApiResponse(this _SA.SalesArrangementParametersGeneralChange model)
        => new()
        {
            Applicant = model.Applicant,
            Collateral = new Collateral
            {
                IsActive = model.Collateral?.IsActive ?? false,
                AddLoanRealEstateCollateral = model.Collateral?.AddLoanRealEstateCollateral,
                ReleaseLoanRealEstateCollateral = model.Collateral?.ReleaseLoanRealEstateCollateral
            },
            PaymentDay = new()
            {
                IsActive = model.PaymentDay?.IsActive ?? false,
                AgreedPaymentDay = model.PaymentDay?.AgreedPaymentDay,
                NewPaymentDay = model.PaymentDay?.NewPaymentDay
            },
            DrawingDateTo = new()
            {
                IsActive = model.DrawingDateTo?.IsActive ?? false,
                AgreedDrawingDateTo = model.DrawingDateTo?.AgreedDrawingDateTo,
                CommentToDrawingDateTo = model.DrawingDateTo?.CommentToDrawingDateTo,
                ExtensionDrawingDateToByMonths = model.DrawingDateTo?.ExtensionDrawingDateToByMonths
            },
            RepaymentAccount = new()
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
            LoanRealEstate = new LoanRealEstate
            {
                IsActive = model.LoanRealEstate?.IsActive ?? false,
                LoanRealEstates = model.LoanRealEstate?.LoanRealEstates?.Select(t => new LoanRealEstateItem
                {
                    RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                    RealEstateTypeId = t.RealEstateTypeId
                }).ToList()
            },
            LoanPurpose = new LoanPurpose
            {
                IsActive = model.LoanPurpose?.IsActive ?? false,
                LoanPurposesComment = model.LoanPurpose?.LoanPurposesComment
            },
            DrawingAndOtherConditions = new DrawingAndOtherConditions
            {
                IsActive = model.DrawingAndOtherConditions?.IsActive ?? false,
                CommentToChangeContractConditions = model.DrawingAndOtherConditions?.CommentToChangeContractConditions
            },
            CommentToChangeRequest = new CommentToChangeRequest
            {
                IsActive = model.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = model.CommentToChangeRequest?.GeneralComment
            }
        };

    public static Dto.HUBNDetail ToApiResponse(this _SA.SalesArrangementParametersHUBN model)
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
            LoanRealEstates = model.LoanRealEstates?.Select(t => new LoanRealEstateItemExtended
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

    public static Dto.CustomerChangeDetail ToApiResponse(this _SA.SalesArrangementParametersCustomerChange model)
        => new()
        {
            Applicants = model.Applicants is null ? null : model.Applicants.Select(t => new Dto.CustomerChangeDetailApplicant
            {
                Identity = t.Identity,
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
            Release = new Dto.CustomerChangeDetailRelease
            {
                IsActive = model.Release?.IsActive ?? false,
                Customers = model.Release?.Customers is null ? null : model.Release.Customers.Select(t => new Dto.CustomerChangeDetailReleaseCustomer
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
            Add = new Dto.CustomerChangeDetailAdd
            {
                IsActive = model.Add?.IsActive ?? false,
                Customers = model.Add?.Customers is null ? null : model.Add.Customers.Select(t => new Dto.CustomerChangeDetailAddCustomer
                {
                    DateOfBirth = t.DateOfBirth,
                    Name = t.Name
                }).ToList()
            },
            Agent = new Dto.CustomerChangeDetailAgent
            {
                IsActive = model.Agent?.IsActive ?? false,
                ActualAgent = model.Agent?.ActualAgent ?? "",
                NewAgent = model.Agent?.NewAgent
            },
            RepaymentAccount = new Dto.PaymentAccount
            {
                IsActive = model.RepaymentAccount?.IsActive ?? false,
                AgreedBankCode = model.RepaymentAccount?.AgreedBankCode,
                AgreedNumber = model.RepaymentAccount?.AgreedNumber,
                AgreedPrefix = model.RepaymentAccount?.AgreedPrefix,
                BankCode = model.RepaymentAccount?.BankCode,
                Number = model.RepaymentAccount?.Number,
                OwnerDateOfBirth = model.RepaymentAccount?.OwnerDateOfBirth,
                OwnerFirstName = model.RepaymentAccount?.OwnerFirstName,
                OwnerLastName = model.RepaymentAccount?.OwnerLastName,
                Prefix = model.RepaymentAccount?.Prefix,
            },
            CommentToChangeRequest = new CommentToChangeRequest
            {
                IsActive = model.CommentToChangeRequest?.IsActive ?? false,
                GeneralComment = model.CommentToChangeRequest?.GeneralComment
            }
        };
}
