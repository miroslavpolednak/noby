﻿using System.ComponentModel.DataAnnotations;

namespace SharedTypes.Enums;

public enum WorkflowTaskTypes : byte
{
    [Display(Name = "Dožádání")]
    Dozadani = 1,

    [Display(Name = "Cenová výjimka")]
    PriceException = 2,

    [Display(Name = "Konzultace")]
    Consultation = 3,

    TaskType4 = 4,

    TaskType5 = 5,

    [Display(Name = "Podepisování")]
    Signing = 6,

    [Display(Name = "Předání na specialistu")]
    PredaniNaSpecialitu = 7,

    [Display(Name = "Čerpání")]
    Drawing = 8,

    [Display(Name = "Retence")]
    RetentionRefixation = 9,
        
    [Display(Name = "Mimořádná splátka")]
    ExtraPayment = 10
}
