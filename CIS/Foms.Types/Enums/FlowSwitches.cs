using System.ComponentModel.DataAnnotations;

namespace CIS.Foms.Enums;

public enum FlowSwitches : int
{
    /// <summary>
    /// Nabídka s platnou garancí
    /// </summary>
    [Display(Name = "Nabídka s platnou garancí")]
    FlowSwitch1 = 1,

    /// <summary>
    /// Požadavek na IC
    /// </summary>
    [Display(Name = "Požadavek na IC")]
    FlowSwitch2 = 2,

    /// <summary>
    /// Alespoň jeden příjem na hlavní domácnosti
    /// </summary>
    [Display(Name = "Alespoň jeden příjem na hlavní domácnosti")]
    FlowSwitch3 = 3,

    /// <summary>
    /// Alespoň jeden příjem na spoludlužnícké domácnosti
    /// </summary>
    [Display(Name = "Alespoň jeden příjem na spoludlužnícké domácnosti")]
    FlowSwitch4 = 4
}
