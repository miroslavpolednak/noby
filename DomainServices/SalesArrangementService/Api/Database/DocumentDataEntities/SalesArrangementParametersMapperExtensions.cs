using DomainServices.SalesArrangementService.Contracts;
using SharedTypes.GrpcTypes;
using SharedTypes.Types;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal static class SalesArrangementParametersMapperExtensions
{
    public static SalesArrangementParametersMortgage MapMortgage(this MortgageData mortgageData)
    {
        return new SalesArrangementParametersMortgage
        {
            ExpectedDateOfDrawing = mortgageData.ExpectedDateOfDrawing,
            IncomeCurrencyCode = mortgageData.IncomeCurrencyCode,
            ResidencyCurrencyCode = mortgageData.ResidencyCurrencyCode,
            ContractSignatureTypeId = mortgageData.ContractSignatureTypeId,
            LoanRealEstates =
            {
                mortgageData.LoanRealEstates.Select(x => new SalesArrangementParametersMortgage.Types.LoanRealEstate
                {
                    RealEstateTypeId = x.RealEstateTypeId,
                    IsCollateral = x.IsCollateral,
                    RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId
                })
            },
            Agent = mortgageData.Agent,
            Comment = mortgageData.Comment,
            FirstSignatureDate = mortgageData.FirstSignatureDate
        };
    }

    public static MortgageData MapMortgage(this SalesArrangementParametersMortgage mortgage)
    {
        return new MortgageData
        {
            ExpectedDateOfDrawing = mortgage.ExpectedDateOfDrawing,
            IncomeCurrencyCode = mortgage.IncomeCurrencyCode,
            ResidencyCurrencyCode = mortgage.ResidencyCurrencyCode,
            ContractSignatureTypeId = mortgage.ContractSignatureTypeId,
            LoanRealEstates = mortgage.LoanRealEstates.Select(x => new MortgageData.LoanRealEstateData
            {
                RealEstateTypeId = x.RealEstateTypeId,
                IsCollateral = x.IsCollateral,
                RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId
            }).ToList(),
            Agent = mortgage.Agent,
            Comment = mortgage.Comment,
            FirstSignatureDate = mortgage.FirstSignatureDate,
        };
    }

    public static SalesArrangementParametersDrawing MapDrawing(this DrawingData drawingData)
    {
        return new SalesArrangementParametersDrawing
        {
            Applicant = { drawingData.Applicant.Select(identity => (Identity)identity) },
            Agent = drawingData.Agent is null ? null : new SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingAgent
            {
                FirstName = drawingData.Agent.FirstName,
                LastName = drawingData.Agent.LastName,
                DateOfBirth = drawingData.Agent.DateOfBirth,
                IsActive = drawingData.Agent.IsActive,
                IdentificationDocument = drawingData.Agent.IdentificationDocument is null ? null : new SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingIdentificationDocument
                {
                    IdentificationDocumentTypeId = drawingData.Agent.IdentificationDocument.IdentificationDocumentTypeId,
                    Number = drawingData.Agent.IdentificationDocument.Number
                }
            },
            RepaymentAccount = drawingData.RepaymentAccount is null ? null : new SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingRepaymentAccount
            {
                IsAccountNumberMissing = drawingData.RepaymentAccount.IsAccountNumberMissing,
                Prefix = drawingData.RepaymentAccount.Prefix,
                Number = drawingData.RepaymentAccount.Number,
                BankCode = drawingData.RepaymentAccount.BankCode
            },
            PayoutList =
            {
                drawingData.PayoutList.Select(x => new SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingPayoutList
                {
                    ProductObligationId = x.ProductObligationId,
                    Order = x.Order,
                    DrawingAmount = x.DrawingAmount,
                    PrefixAccount = x.PrefixAccount,
                    AccountNumber = x.AccountNumber,
                    BankCode = x.BankCode,
                    VariableSymbol = x.VariableSymbol,
                    SpecificSymbol = x.SpecificSymbol
                })
            },
            DrawingDate = drawingData.DrawingDate,
            IsImmediateDrawing = drawingData.IsImmediateDrawing
        };
    }

    public static DrawingData MapDrawing(this SalesArrangementParametersDrawing drawing)
    {
        return new DrawingData
        {
            Applicant = drawing.Applicant.Select(identity => (CustomerIdentity)identity!).ToList(),
            Agent = drawing.Agent is null ? null : new DrawingData.DrawingAgentData
            {
                FirstName = drawing.Agent.FirstName,
                LastName = drawing.Agent.LastName,
                DateOfBirth = drawing.Agent.DateOfBirth,
                IsActive = drawing.Agent.IsActive,
                IdentificationDocument = drawing.Agent.IdentificationDocument is null ? null : new DrawingData.DrawingAgentData.DrawingAgentIdentificationDocumentData
                {
                    IdentificationDocumentTypeId = drawing.Agent.IdentificationDocument.IdentificationDocumentTypeId,
                    Number = drawing.Agent.IdentificationDocument.Number
                }
            },
            RepaymentAccount = drawing.RepaymentAccount is null ? null : new DrawingData.DrawingRepaymentAccount
            {
                IsAccountNumberMissing = drawing.RepaymentAccount.IsAccountNumberMissing,
                Prefix = drawing.RepaymentAccount.Prefix,
                Number = drawing.RepaymentAccount.Number,
                BankCode = drawing.RepaymentAccount.BankCode
            },
            PayoutList = drawing.PayoutList.Select(x => new DrawingData.DrawingPayoutListItem
            {
                ProductObligationId = x.ProductObligationId,
                Order = x.Order,
                DrawingAmount = x.DrawingAmount,
                PrefixAccount = x.PrefixAccount,
                AccountNumber = x.AccountNumber,
                BankCode = x.BankCode,
                VariableSymbol = x.VariableSymbol,
                SpecificSymbol = x.SpecificSymbol,
                ConstantSymbol = x.ConstantSymbol,
                PayoutTypeId = x.PayoutTypeId
            }).ToList(),
            DrawingDate = drawing.DrawingDate,
            IsImmediateDrawing = drawing.IsImmediateDrawing
        };
    }

    public static SalesArrangementParametersHUBN MapHUBN(this HUBNData hubnData)
    {
        return new SalesArrangementParametersHUBN
        {
            Applicant = { hubnData.Applicant.Select(identity => (Identity)identity) },
            LoanAmount = hubnData.LoanAmount is null ? null : new SalesArrangementParametersHUBN.Types.LoanAmountObject
            {
                ChangeAgreedLoanAmount = hubnData.LoanAmount.ChangeAgreedLoanAmount,
                AgreedLoanAmount = hubnData.LoanAmount.AgreedLoanAmount,
                RequiredLoanAmount = hubnData.LoanAmount.RequiredLoanAmount,
                PreserveAgreedLoanDueDate = hubnData.LoanAmount.PreserveAgreedLoanDueDate,
                AgreedLoanDueDate = hubnData.LoanAmount.AgreedLoanDueDate,
                PreserveAgreedLoanPaymentAmount = hubnData.LoanAmount.PreserveAgreedLoanPaymentAmount,
                AgreedLoanPaymentAmount = hubnData.LoanAmount.AgreedLoanPaymentAmount
            },
            LoanPurposes =
            {
                hubnData.LoanPurposes.Select(x => new SalesArrangementParametersHUBN.Types.LoanPurposeItem
                {
                    LoanPurposeId = x.LoanPurposeId,
                    Sum = x.Sum
                })
            },
            LoanRealEstates =
            {
                hubnData.LoanRealEstates.Select(x => new SalesArrangementParametersHUBN.Types.LoanRealEstateItem
                {
                    RealEstateTypeId = x.RealEstateTypeId,
                    RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                    IsCollateral = x.IsCollateral
                })
            },
            CollateralIdentification = hubnData.CollateralIdentification is null ? null : new SalesArrangementParametersHUBN.Types.CollateralIdentificationObject
            {
                RealEstateIdentification = hubnData.CollateralIdentification.RealEstateIdentification
            },
            ExpectedDateOfDrawing = hubnData.ExpectedDateOfDrawing is null ? null : new SalesArrangementParametersHUBN.Types.ExpectedDateOfDrawingObject
            {
                IsActive = hubnData.ExpectedDateOfDrawing.IsActive,
                NewExpectedDateOfDrawing = hubnData.ExpectedDateOfDrawing.NewExpectedDateOfDrawing,
                AgreedExpectedDateOfDrawing = hubnData.ExpectedDateOfDrawing.AgreedExpectedDateOfDrawing
            },
            DrawingDateTo = hubnData.DrawingDateTo is null ? null : new SalesArrangementParametersHUBN.Types.DrawingDateToObject
            {
                IsActive = hubnData.DrawingDateTo.IsActive,
                AgreedDrawingDateTo = hubnData.DrawingDateTo.AgreedDrawingDateTo,
                ExtensionDrawingDateToByMonths = hubnData.DrawingDateTo.ExtensionDrawingDateToByMonths
            },
            CommentToChangeRequest = hubnData.CommentToChangeRequest is null ? null : new SalesArrangementParametersHUBN.Types.CommentToChangeRequestObject
            {
                IsActive = hubnData.CommentToChangeRequest.IsActive,
                GeneralComment = hubnData.CommentToChangeRequest.GeneralComment
            }
        };
    }

    public static HUBNData MapHUBN(this SalesArrangementParametersHUBN hubn)
    {
        return new HUBNData
        {
            Applicant = hubn.Applicant.Select(identity => (CustomerIdentity)identity!).ToList(),
            LoanAmount = hubn.LoanAmount is null ? null : new HUBNData.HUBNLoanAmountData
            {
                ChangeAgreedLoanAmount = hubn.LoanAmount.ChangeAgreedLoanAmount,
                AgreedLoanAmount = hubn.LoanAmount.AgreedLoanAmount,
                RequiredLoanAmount = hubn.LoanAmount.RequiredLoanAmount,
                PreserveAgreedLoanDueDate = hubn.LoanAmount.PreserveAgreedLoanDueDate,
                AgreedLoanDueDate = hubn.LoanAmount.AgreedLoanDueDate,
                PreserveAgreedLoanPaymentAmount = hubn.LoanAmount.PreserveAgreedLoanPaymentAmount,
                AgreedLoanPaymentAmount = hubn.LoanAmount.AgreedLoanPaymentAmount
            },
            LoanPurposes = hubn.LoanPurposes.Select(x => new HUBNData.HUBNLoanPurposeItemData
            {
                LoanPurposeId = x.LoanPurposeId,
                Sum = x.Sum
            }).ToList(),
            LoanRealEstates = hubn.LoanRealEstates.Select(x => new HUBNData.HUBNLoanRealEstateItemData
            {
                RealEstateTypeId = x.RealEstateTypeId,
                RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                IsCollateral = x.IsCollateral
            }).ToList(),
            CollateralIdentification = hubn.CollateralIdentification is null ? null : new HUBNData.HUBNCollateralIdentificationData
            {
                RealEstateIdentification = hubn.CollateralIdentification.RealEstateIdentification
            },
            ExpectedDateOfDrawing = hubn.ExpectedDateOfDrawing is null ? null : new HUBNData.HUBNExpectedDateOfDrawingData
            {
                IsActive = hubn.ExpectedDateOfDrawing.IsActive,
                NewExpectedDateOfDrawing = hubn.ExpectedDateOfDrawing.NewExpectedDateOfDrawing,
                AgreedExpectedDateOfDrawing = hubn.ExpectedDateOfDrawing.AgreedExpectedDateOfDrawing
            },
            DrawingDateTo = hubn.DrawingDateTo is null ? null : new HUBNData.HUBNDrawingDateToData
            {
                IsActive = hubn.DrawingDateTo.IsActive,
                AgreedDrawingDateTo = hubn.DrawingDateTo.AgreedDrawingDateTo,
                ExtensionDrawingDateToByMonths = hubn.DrawingDateTo.ExtensionDrawingDateToByMonths
            },
            CommentToChangeRequest = hubn.CommentToChangeRequest is null ? null : new HUBNData.HUBNCommentToChangeRequestData
            {
                IsActive = hubn.CommentToChangeRequest.IsActive,
                GeneralComment = hubn.CommentToChangeRequest.GeneralComment
            }
        };
    }

    public static SalesArrangementParametersGeneralChange MapGeneralChange(this GeneralChangeData generalChangeData)
    {
        return new SalesArrangementParametersGeneralChange
        {
            Applicant = { generalChangeData.Applicant.Select(identity => (Identity)identity) },
            Collateral = generalChangeData.Collateral is null ? null : new SalesArrangementParametersGeneralChange.Types.CollateralObject
            {
                IsActive = generalChangeData.Collateral.IsActive,
                AddLoanRealEstateCollateral = generalChangeData.Collateral.AddLoanRealEstateCollateral,
                ReleaseLoanRealEstateCollateral = generalChangeData.Collateral.ReleaseLoanRealEstateCollateral
            },
            PaymentDay = generalChangeData.PaymentDay is null ? null : new SalesArrangementParametersGeneralChange.Types.PaymentDayObject
            {
                IsActive = generalChangeData.PaymentDay.IsActive,
                AgreedPaymentDay = generalChangeData.PaymentDay.AgreedPaymentDay,
                NewPaymentDay = generalChangeData.PaymentDay.NewPaymentDay
            },
            DrawingDateTo = generalChangeData.DrawingDateTo is null ? null : new SalesArrangementParametersGeneralChange.Types.DrawingDateToObject
            {
                IsActive = generalChangeData.DrawingDateTo.IsActive,
                AgreedDrawingDateTo = generalChangeData.DrawingDateTo.AgreedDrawingDateTo,
                ExtensionDrawingDateToByMonths = generalChangeData.DrawingDateTo.ExtensionDrawingDateToByMonths,
                CommentToDrawingDateTo = generalChangeData.DrawingDateTo.CommentToDrawingDateTo
            },
            RepaymentAccount = generalChangeData.RepaymentAccount is null ? null : new SalesArrangementParametersGeneralChange.Types.PaymentAccountObject
            {
                IsActive = generalChangeData.RepaymentAccount.IsActive,
                AgreedPrefix = generalChangeData.RepaymentAccount.AgreedPrefix,
                AgreedNumber = generalChangeData.RepaymentAccount.AgreedNumber,
                AgreedBankCode = generalChangeData.RepaymentAccount.AgreedBankCode,
                Prefix = generalChangeData.RepaymentAccount.Prefix,
                Number = generalChangeData.RepaymentAccount.Number,
                BankCode = generalChangeData.RepaymentAccount.BankCode,
                OwnerFirstName = generalChangeData.RepaymentAccount.OwnerFirstName,
                OwnerLastName = generalChangeData.RepaymentAccount.OwnerLastName,
                OwnerDateOfBirth = generalChangeData.RepaymentAccount.OwnerDateOfBirth
            },
            LoanPaymentAmount = generalChangeData.LoanPaymentAmount is null ? null : new SalesArrangementParametersGeneralChange.Types.LoanPaymentAmountObject
            {
                IsActive = generalChangeData.LoanPaymentAmount.IsActive,
                NewLoanPaymentAmount = generalChangeData.LoanPaymentAmount.NewLoanPaymentAmount,
                ActualLoanPaymentAmount = generalChangeData.LoanPaymentAmount.ActualLoanPaymentAmount,
                ConnectionExtraordinaryPayment = generalChangeData.LoanPaymentAmount.ConnectionExtraordinaryPayment
            },
            DueDate = generalChangeData.DueDate is null ? null : new SalesArrangementParametersGeneralChange.Types.DueDateObject
            {
                IsActive = generalChangeData.DueDate.IsActive,
                NewLoanDueDate = generalChangeData.DueDate.NewLoanDueDate,
                ActualLoanDueDate = generalChangeData.DueDate.ActualLoanDueDate,
                ConnectionExtraordinaryPayment = generalChangeData.DueDate.ConnectionExtraordinaryPayment
            },
            LoanRealEstate = generalChangeData.LoanRealEstate is null ? null : new SalesArrangementParametersGeneralChange.Types.LoanRealEstateObject
            {
                IsActive = generalChangeData.LoanRealEstate.IsActive,
                LoanRealEstates =
                {
                    generalChangeData.LoanRealEstate.LoanRealEstates.Select(realEstate => new SalesArrangementParametersGeneralChange.Types.LoanRealEstatesItem
                    {
                        RealEstateTypeId = realEstate.RealEstateTypeId,
                        RealEstatePurchaseTypeId = realEstate.RealEstatePurchaseTypeId,
                    })
                }
            },
            LoanPurpose = generalChangeData.LoanPurpose is null ? null : new SalesArrangementParametersGeneralChange.Types.LoanPurposeObject
            {
                IsActive = generalChangeData.LoanPurpose.IsActive,
                LoanPurposesComment = generalChangeData.LoanPurpose.LoanPurposesComment
            },
            DrawingAndOtherConditions = generalChangeData.DrawingAndOtherConditions is null ? null : new SalesArrangementParametersGeneralChange.Types.DrawingAndOtherConditionsObject
            {
                IsActive = generalChangeData.DrawingAndOtherConditions.IsActive,
                CommentToChangeContractConditions = generalChangeData.DrawingAndOtherConditions.CommentToChangeContractConditions
            },
            CommentToChangeRequest = generalChangeData.CommentToChangeRequest is null ? null : new SalesArrangementParametersGeneralChange.Types.CommentToChangeRequestObject
            {
                IsActive = generalChangeData.CommentToChangeRequest.IsActive,
                GeneralComment = generalChangeData.CommentToChangeRequest.GeneralComment
            }
        };
    }

    public static GeneralChangeData MapGeneralChange(this SalesArrangementParametersGeneralChange generalChange)
    {
        return new GeneralChangeData
        {
            Applicant = generalChange.Applicant.Select(identity => (CustomerIdentity)identity!).ToList(),
            Collateral = generalChange.Collateral is null ? null : new GeneralChangeData.GeneralChangeCollateralData
            {
                IsActive = generalChange.Collateral.IsActive,
                AddLoanRealEstateCollateral = generalChange.Collateral.AddLoanRealEstateCollateral,
                ReleaseLoanRealEstateCollateral = generalChange.Collateral.ReleaseLoanRealEstateCollateral
            },
            PaymentDay = generalChange.PaymentDay is null ? null : new GeneralChangeData.GeneralChangePaymentDayData
            {
                IsActive = generalChange.PaymentDay.IsActive,
                AgreedPaymentDay = generalChange.PaymentDay.AgreedPaymentDay,
                NewPaymentDay = generalChange.PaymentDay.NewPaymentDay
            },
            DrawingDateTo = generalChange.DrawingDateTo is null ? null : new GeneralChangeData.GeneralChangeDrawingDateToData
            {
                IsActive = generalChange.DrawingDateTo.IsActive,
                AgreedDrawingDateTo = generalChange.DrawingDateTo.AgreedDrawingDateTo,
                ExtensionDrawingDateToByMonths = generalChange.DrawingDateTo.ExtensionDrawingDateToByMonths,
                CommentToDrawingDateTo = generalChange.DrawingDateTo.CommentToDrawingDateTo
            },
            RepaymentAccount = generalChange.RepaymentAccount is null ? null : new GeneralChangeData.GeneralChangePaymentAccountData
            {
                IsActive = generalChange.RepaymentAccount.IsActive,
                AgreedPrefix = generalChange.RepaymentAccount.AgreedPrefix,
                AgreedNumber = generalChange.RepaymentAccount.AgreedNumber,
                AgreedBankCode = generalChange.RepaymentAccount.AgreedBankCode,
                Prefix = generalChange.RepaymentAccount.Prefix,
                Number = generalChange.RepaymentAccount.Number,
                BankCode = generalChange.RepaymentAccount.BankCode,
                OwnerFirstName = generalChange.RepaymentAccount.OwnerFirstName,
                OwnerLastName = generalChange.RepaymentAccount.OwnerLastName,
                OwnerDateOfBirth = generalChange.RepaymentAccount.OwnerDateOfBirth
            },
            LoanPaymentAmount = generalChange.LoanPaymentAmount is null ? null : new GeneralChangeData.GeneralChangeLoanPaymentAmountData
            {
                IsActive = generalChange.LoanPaymentAmount.IsActive,
                NewLoanPaymentAmount = generalChange.LoanPaymentAmount.NewLoanPaymentAmount,
                ActualLoanPaymentAmount = generalChange.LoanPaymentAmount.ActualLoanPaymentAmount,
                ConnectionExtraordinaryPayment = generalChange.LoanPaymentAmount.ConnectionExtraordinaryPayment
            },
            DueDate = generalChange.DueDate is null ? null : new GeneralChangeData.GeneralChangeDueDateData
            {
                IsActive = generalChange.DueDate.IsActive,
                NewLoanDueDate = generalChange.DueDate.NewLoanDueDate,
                ActualLoanDueDate = generalChange.DueDate.ActualLoanDueDate,
                ConnectionExtraordinaryPayment = generalChange.DueDate.ConnectionExtraordinaryPayment
            },
            LoanRealEstate = generalChange.LoanRealEstate is null ? null : new GeneralChangeData.GeneralChangeLoanRealEstateData
            {
                IsActive = generalChange.LoanRealEstate.IsActive,
                LoanRealEstates = generalChange.LoanRealEstate.LoanRealEstates.Select(realEstate => new GeneralChangeData.GeneralChangeLoanRealEstatesItemData
                {
                    RealEstateTypeId = realEstate.RealEstateTypeId,
                    RealEstatePurchaseTypeId = realEstate.RealEstatePurchaseTypeId,
                }).ToList()
            },
            LoanPurpose = generalChange.LoanPurpose is null ? null : new GeneralChangeData.GeneralChangeLoanPurposeData
            {
                IsActive = generalChange.LoanPurpose.IsActive,
                LoanPurposesComment = generalChange.LoanPurpose.LoanPurposesComment
            },
            DrawingAndOtherConditions = generalChange.DrawingAndOtherConditions is null ? null : new GeneralChangeData.GeneralChangeDrawingAndOtherConditionsData
            {
                IsActive = generalChange.DrawingAndOtherConditions.IsActive,
                CommentToChangeContractConditions = generalChange.DrawingAndOtherConditions.CommentToChangeContractConditions
            },
            CommentToChangeRequest = generalChange.CommentToChangeRequest is null ? null : new GeneralChangeData.GeneralChangeCommentToChangeRequestData
            {
                IsActive = generalChange.CommentToChangeRequest.IsActive,
                GeneralComment = generalChange.CommentToChangeRequest.GeneralComment
            }
        };
    }

    public static SalesArrangementParametersCustomerChange MapCustomerChange(this CustomerChangeData customerChangeData)
    {
        return new SalesArrangementParametersCustomerChange
        {
            Applicants =
            {
                customerChangeData.Applicants.Select(applicant => new SalesArrangementParametersCustomerChange.Types.ApplicantObject
                {
                    Identity = { applicant.Identity.Select(identity => (Identity)identity) },
                    NaturalPerson = applicant.NaturalPerson is null ? null : new SalesArrangementParametersCustomerChange.Types.NaturalPersonObject
                    {
                        FirstName = applicant.NaturalPerson.FirstName,
                        LastName = applicant.NaturalPerson.LastName,
                        DateOfBirth = applicant.NaturalPerson.DateOfBirth,
                    },
                    IdentificationDocument = applicant.IdentificationDocument is null ? null : new SalesArrangementParametersCustomerChange.Types.IdentificationDocumentObject
                    {
                        IdentificationDocumentTypeId = applicant.IdentificationDocument.IdentificationDocumentTypeId,
                        Number = applicant.IdentificationDocument.Number
                    }
                })
            },
            Release = customerChangeData.Release is null ? null : new SalesArrangementParametersCustomerChange.Types.ReleaseObject
            {
                IsActive = customerChangeData.Release.IsActive,
                Customers =
                {
                    customerChangeData.Release.Customers.Select(customer => new SalesArrangementParametersCustomerChange.Types.ReleaseCustomerObject
                    {
                        Identity = (Identity)customer.Identity,
                        NaturalPerson = customer.NaturalPerson is null ? null : new SalesArrangementParametersCustomerChange.Types.NaturalPersonObject
                        {
                            FirstName = customer.NaturalPerson.FirstName,
                            LastName = customer.NaturalPerson.LastName,
                            DateOfBirth = customer.NaturalPerson.DateOfBirth,
                        }
                    })
                }
            },
            Add = customerChangeData.Add is null ? null : new SalesArrangementParametersCustomerChange.Types.AddObject
            {
                IsActive = customerChangeData.Add.IsActive,
                Customers =
                {
                    customerChangeData.Add.Customers.Select(customer => new SalesArrangementParametersCustomerChange.Types.AddCustomerObject
                    {
                        Name = customer.Name,
                        DateOfBirth = customer.DateOfBirth
                    })
                }
            },
            Agent = customerChangeData.Agent is null ? null : new SalesArrangementParametersCustomerChange.Types.AgentObject
            {
                IsActive = customerChangeData.Agent.IsActive,
                ActualAgent = customerChangeData.Agent.ActualAgent,
                NewAgent = customerChangeData.Agent.NewAgent
            },
            RepaymentAccount = customerChangeData.RepaymentAccount is null ? null : new SalesArrangementParametersCustomerChange.Types.PaymentAccountObject
            {
                IsActive = customerChangeData.RepaymentAccount.IsActive,
                AgreedPrefix = customerChangeData.RepaymentAccount.AgreedPrefix,
                AgreedNumber = customerChangeData.RepaymentAccount.AgreedNumber,
                AgreedBankCode = customerChangeData.RepaymentAccount.AgreedBankCode,
                Prefix = customerChangeData.RepaymentAccount.Prefix,
                Number = customerChangeData.RepaymentAccount.Number,
                BankCode = customerChangeData.RepaymentAccount.BankCode,
                OwnerFirstName = customerChangeData.RepaymentAccount.OwnerFirstName,
                OwnerLastName = customerChangeData.RepaymentAccount.OwnerLastName,
                OwnerDateOfBirth = customerChangeData.RepaymentAccount.OwnerDateOfBirth
            },
            CommentToChangeRequest = customerChangeData.CommentToChangeRequest is null ? null : new SalesArrangementParametersCustomerChange.Types.CommentToChangeRequestObject
            {
                IsActive = customerChangeData.CommentToChangeRequest.IsActive,
                GeneralComment = customerChangeData.CommentToChangeRequest.GeneralComment
            }
        };
    }

    public static CustomerChangeData MapCustomerChange(this SalesArrangementParametersCustomerChange customerChange)
    {
        return new CustomerChangeData
        {
            Applicants = customerChange.Applicants.Select(applicant => new CustomerChangeData.CustomerChangeApplicantData
            {
                Identity = applicant.Identity.Select(identity => (CustomerIdentity)identity!).ToList(),
                NaturalPerson = applicant.NaturalPerson is null ? null : new CustomerChangeData.CustomerChangeNaturalPersonData
                {
                    FirstName = applicant.NaturalPerson.FirstName,
                    LastName = applicant.NaturalPerson.LastName,
                    DateOfBirth = applicant.NaturalPerson.DateOfBirth,
                },
                IdentificationDocument = applicant.IdentificationDocument is null ? null : new CustomerChangeData.CustomerChangeIdentificationDocumentData
                {
                    IdentificationDocumentTypeId = applicant.IdentificationDocument.IdentificationDocumentTypeId,
                    Number = applicant.IdentificationDocument.Number
                }
            }).ToList(),
            Release = customerChange.Release is null ? null : new CustomerChangeData.CustomerChangeReleaseData
            {
                IsActive = customerChange.Release.IsActive,
                Customers = customerChange.Release.Customers.Select(customer => new CustomerChangeData.CustomerChangeReleaseCustomerData
                {
                    Identity = customer.Identity!,
                    NaturalPerson = customer.NaturalPerson is null ? null : new CustomerChangeData.CustomerChangeNaturalPersonData
                    {
                        FirstName = customer.NaturalPerson.FirstName,
                        LastName = customer.NaturalPerson.LastName,
                        DateOfBirth = customer.NaturalPerson.DateOfBirth,
                    }
                }).ToList()
            },
            Add = customerChange.Add is null ? null : new CustomerChangeData.CustomerChangeAddData
            {
                IsActive = customerChange.Add.IsActive,
                Customers = customerChange.Add.Customers.Select(customer => new CustomerChangeData.CustomerChangeAddCustomerData
                {
                    Name = customer.Name,
                    DateOfBirth = customer.DateOfBirth
                }).ToList()
            },
            Agent = customerChange.Agent is null ? null : new CustomerChangeData.CustomerChangeAgentData
            {
                IsActive = customerChange.Agent.IsActive,
                ActualAgent = customerChange.Agent.ActualAgent,
                NewAgent = customerChange.Agent.NewAgent
            },
            RepaymentAccount = customerChange.RepaymentAccount is null ? null : new CustomerChangeData.CustomerChangePaymentAccountData
            {
                IsActive = customerChange.RepaymentAccount.IsActive,
                AgreedPrefix = customerChange.RepaymentAccount.AgreedPrefix,
                AgreedNumber = customerChange.RepaymentAccount.AgreedNumber,
                AgreedBankCode = customerChange.RepaymentAccount.AgreedBankCode,
                Prefix = customerChange.RepaymentAccount.Prefix,
                Number = customerChange.RepaymentAccount.Number,
                BankCode = customerChange.RepaymentAccount.BankCode,
                OwnerFirstName = customerChange.RepaymentAccount.OwnerFirstName,
                OwnerLastName = customerChange.RepaymentAccount.OwnerLastName,
                OwnerDateOfBirth = customerChange.RepaymentAccount.OwnerDateOfBirth
            },
        };
    }

    public static SalesArrangementParametersCustomerChange3602 MapCustomerChange3602(this CustomerChange3602Data customerChange3602Data)
    {
        return new SalesArrangementParametersCustomerChange3602
        {
            HouseholdId = customerChange3602Data.HouseholdId,
            IsSpouseInDebt = customerChange3602Data.IsSpouseInDebt
        };
    }

    public static CustomerChange3602Data MapCustomerChange3602(this SalesArrangementParametersCustomerChange3602 customerChange3602)
    {
        return new CustomerChange3602Data
        {
            HouseholdId = customerChange3602.HouseholdId,
            IsSpouseInDebt = customerChange3602.IsSpouseInDebt
        };
    }
}