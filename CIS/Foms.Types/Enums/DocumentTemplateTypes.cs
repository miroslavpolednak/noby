using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum DocumentTemplateTypes : byte
{
    [Display(Name = "unknown", ShortName = "")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Nabídka hypotečního úvěru", ShortName = "NABIDKA")]
    NABIDKA = 1,
        
    [EnumMember]
    [Display(Name = "Hypoteční úvěr - kalkulace", ShortName = "KALKULHU")]
    KALKULHU = 2,
        
    [EnumMember]
    [Display(Name = "Simulace splátkového kalendáře", ShortName = "SPLKALHU")]
    SPLKALHU = 3,

    [EnumMember]
    [Display(Name = "Žádost o poskytnutí hypotečního úvěru - první domácnost", ShortName = "ZADOSTHU")]
    ZADOSTHU = 4,

    [EnumMember]
    [Display(Name = "Žádost o poskytnutí hypotečního úvěru - druhá domácnost", ShortName = "ZADOSTHD")]
    ZADOSTHD = 5,

    [EnumMember]
    [Display(Name = "Žádost o čerpání hypotečního úvěru", ShortName = "ZADOCERP")]
    ZADOCERP = 6,

    [EnumMember]
    [Display(Name = "Sdělení čísla účtu pro čerpání", ShortName = "SDELUCET")]
    SDELUCET = 7,

    [EnumMember]
    [Display(Name = "Žádost o změnu obecná", ShortName = "ZAOZMPAR")]
    ZAOZMPAR = 8,

    [EnumMember]
    [Display(Name = "Žádost o změnu dlužníka", ShortName = "ZAOZMDLU")]
    ZAOZMDLU = 9,

    [EnumMember]
    [Display(Name = "Žádost o změnu - HÚ bez nemovitosti", ShortName = "ZAODHUBN")]
    ZAODHUBN = 10,

    [EnumMember]
    [Display(Name = "Žádost o změnu Flexi", ShortName = "ZADOOPCI")]
    ZADOOPCI = 11,

    // TODO ???
    //[EnumMember]
    //[Display(Name = "Údaje o zůstávajícím v dluhu", ShortName = "TBD")]
    //XXX = 12,

    //[EnumMember]
    //[Display(Name = "Údaje o přistupujícím k dluhu", ShortName = "TBD")]
    //YYY = 13,
}
