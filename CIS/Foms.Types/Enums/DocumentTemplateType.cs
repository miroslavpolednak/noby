using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CIS.Foms.Enums;

[DataContract]
public enum DocumentTemplateType : byte
{
    [Display(Name = "unknown")]
    [EnumMember]
    Unknown = 0,
    
    [EnumMember]
    [Display(Name = "Nabídka hypotečního úvěru")]
    NABIDKA = 1,
        
    [EnumMember]
    [Display(Name = "Hypoteční úvěr - kalkulace")]
    KALKULHU = 2,
        
    [EnumMember]
    [Display(Name = "Simulace splátkového kalendáře")]
    SPLKALHU = 3,

    [EnumMember]
    [Display(Name = "Žádost o poskytnutí hypotečního úvěru - první domácnost")]
    ZADOSTHU = 4,

    [EnumMember]
    [Display(Name = "Žádost o poskytnutí hypotečního úvěru - druhá domácnost")]
    ZADOSTHD = 5,

    [EnumMember]
    [Display(Name = "Žádost o čerpání hypotečního úvěru")]
    ZADOCERP = 6,

    [EnumMember]
    [Display(Name = "Sdělení čísla účtu pro čerpání")]
    SDELUCET = 7,

    [EnumMember]
    [Display(Name = "Žádost o změnu obecná")]
    ZAOZMPAR = 8,

    [EnumMember]
    [Display(Name = "Žádost o změnu dlužníka")]
    ZAOZMDLU = 9,

    [EnumMember]
    [Display(Name = "Žádost o změnu - HÚ bez nemovitosti")]
    ZAODHUBN = 10,

    [EnumMember]
    [Display(Name = "Žádost o změnu Flexi")]
    ZADOOPCI = 11,

    // TODO ???
    //[EnumMember]
    //[Display(Name = "Údaje o zůstávajícím v dluhu")]
    //XXX = 12,

    //[EnumMember]
    //[Display(Name = "Údaje o přistupujícím k dluhu")]
    //YYY = 13,
}
