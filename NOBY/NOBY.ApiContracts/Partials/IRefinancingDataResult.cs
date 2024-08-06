using SharedTypes.Enums;

namespace NOBY.ApiContracts;

public interface IRefinancingDataResult
{
    int? SalesArrangementId { get; set; }

    /// <summary>
    /// Informace zda se jedná o readonly režim
    /// </summary>
    bool IsReadOnly { get; set; }

    /// <summary>
    /// Komentář k IC
    /// </summary>
    string? IndividualPriceCommentLastVersion { get; set; }

    /// <summary>
    /// Všechny workflow úkoly pro daný proces retence
    /// </summary>
    List<SharedTypesWorkflowTask>? Tasks { get; set; }

    /// <summary>
    /// Příznak nastavený na true pokud jsou data o IC v nalinkované simulaci rozdílné od aktuální IC v SB
    /// </summary>
    bool ContainsInconsistentIndividualPriceData { get; set; }

    /// <summary>
    /// Stav
    /// </summary>
    EnumRefinancingStates RefinancingStateId { get; set; }

    /// <summary>
    /// Existuje workflow úkol cenové vyjímky
    /// </summary>
    bool IsPriceExceptionActive { get; set; }
}
