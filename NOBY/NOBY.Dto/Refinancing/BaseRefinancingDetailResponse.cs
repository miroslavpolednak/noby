﻿using SharedTypes.Enums;

namespace NOBY.Dto.Refinancing;

public class BaseRefinancingDetailResponse
{
    public int? SalesArrangementId { get; set; }

    /// <summary>
    /// Informace zda se jedná o readonly režim
    /// </summary>
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// Komentář k IC
    /// </summary>
    public string? IndividualPriceCommentLastVersion { get; set; }

    /// <summary>
    /// Všechny workflow úkoly pro daný proces retence
    /// </summary>
    public List<NOBY.Dto.Workflow.WorkflowTask>? Tasks { get; set; }

    /// <summary>
    /// Příznak nastavený na true pokud jsou data o IC v nalinkované simulaci rozdílné od aktuální IC v SB
    /// </summary>
    public bool ContainsInconsistentIndividualPriceData { get; set; }

    /// <summary>
    /// Stav
    /// </summary>
    public EnumRefinancingStates RefinancingStateId { get; set; }

    /// <summary>
    /// Existuje workflow úkol cenové vyjímky
    /// </summary>
    public bool IsPriceExceptionActive { get; set; }
}
