using CIS.Foms.Types;

namespace NOBY.Api.Endpoints.SalesArrangement.Dto;

public sealed class ParametersHUBN
{
    public CustomerIdentity? Applicant { get; set; }

    public PaymentAccountObject2 PaymentAccount { get; set; }

    public LoanAmountObject LoanAmount { get; set; }

    /// <summary>
    /// Účely úvěru
    /// </summary>
    public List<LoanPurposeItem2> LoanPurposes { get; set; }

    /// <summary>
    /// Objekty úvěru
    /// </summary>
    public List<LoanRealEstateItem2> LoanRealEstates { get; set; }

    public CollateralIdentificationObject CollateralIdentification { get; set; }

    public ExpectedDateOfDrawingObject2 ExpectedDateOfDrawing { get; set; }

    public DrawingDateToObject2 DrawingDateTo { get; set; }

    public CommentToChangeRequestObject CommentToChangeRequest { get; set; }
}

/// <summary>
/// Účet pro splácení
/// </summary>
public sealed class PaymentAccountObject2
{
    /// <summary>
    /// Předčíslí účtu
    /// </summary>
    public string AgreedPrefix { get; set; }

    /// <summary>
    /// Číslo účtu
    /// </summary>
    public string AgreedNumber { get; set; }

    /// <summary>
    /// Kód banky
    /// </summary>
    public string AgreedBankCode { get; set; }
}

public sealed class LoanAmountObject
{
    /// <summary>
    /// Změnit sjednanou výši úvěru
    /// </summary>
    public bool ChangeAgreedLoanAmount { get; set; }

    /// <summary>
    /// Sjednaná výše úvěru
    /// </summary>
    public decimal AgreedLoanAmount { get; set; }

    /// <summary>
    /// Požadovaná výše úvěru
    /// </summary>
    public decimal? RequiredLoanAmount { get; set; }

    /// <summary>
    /// Zachovat sjednanou splatnost do
    /// </summary>
    public bool PreserveLoanDueDate { get; set; }

    /// <summary>
    /// Sjednaná splatnost do
    /// </summary>
    public DateTime AgreedLoanDueDate { get; set; }

    /// <summary>
    /// Zachovat sjednanou splátku
    /// </summary>
    public bool PreserveAgreedPaymentAmount { get; set; }

    /// <summary>
    /// Sjednaná měsíční splátka
    /// </summary>
    public double AgreedLoanPaymentAmount { get; set; }
}

/// <summary>
/// Účel úvěru
/// </summary>
public sealed class LoanPurposeItem2
{
    /// <summary>
    /// Účel úvěru
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Výše úvěru pro zvolený účel v Kč
    /// </summary>
    public decimal Sum { get; set; }
}

public sealed class LoanRealEstateItem2
{
    public int RealEstateTypeId { get; set; }

    public bool IsCollateral { get; set; }

    public int RealEstatePurchaseTypeId { get; set; }
}

public sealed class CollateralIdentificationObject
{
    /// <summary>
    /// Identifikace nemovitosti
    /// </summary>
    public string RealEstateIdentification { get; set; }
}

public sealed class ExpectedDateOfDrawingObject2
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Sjednaný termín prvního čerpání
    /// </summary>
    public DateTime AgreedDateOfDrawing { get; set; }

    /// <summary>
    /// Nový předpokládaný termín prvního čerpání
    /// </summary>
    public DateTime? NewExpectedDateOfDrawing { get; set; }
}

public sealed class DrawingDateToObject2
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Sjednaný termín čerpání do
    /// </summary>
    public DateTime AgreedDrawingDateTo { get; set; }

    /// <summary>
    /// Prodloužení konce lhůty čerpání o kolik měsíců
    /// </summary>
    public int ExtensionDrawingDateToByMonths { get; set; }
}