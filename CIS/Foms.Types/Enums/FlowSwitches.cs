namespace CIS.Foms.Enums;

public enum FlowSwitches : int
{
    /// <summary>
    /// Nabídka s platnou garancí
    /// </summary>
    IsOfferGuaranteed = 1,

    /// <summary>
    /// Identifikovaní klienti na hlavní domácnosti
    /// </summary>
    CustomerIdentifiedOnMainHousehold = 2,

    /// <summary>
    /// Identifikovaní klienti na spolužadatelské domácnosti
    /// </summary>
    CustomerIdentifiedOnCodebtorHousehold = 3,

    /// <summary>
    /// 3601 pro hlavní domácnost byla změněna po podpisu
    /// </summary>
    Was3601MainChangedAfterSigning = 4,

    /// <summary>
    /// 3602 pro spoludlužnickou domácnost byla změněna po podpisu
    /// </summary>
    Was3602CodebtorChangedAfterSigning = 5,

    /// <summary>
    /// Došlo k uložení parametrů na žádosti
    /// </summary>
    ParametersSavedAtLeastOnce = 6
}
