namespace NOBY.Api.Endpoints.CustomerObligation.Dto;

public class ObligationCreditorDto
{
    /// <summary>
    /// Id věřitele z číselníku bank <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=455007953">BankCode (CIS_KODY_BANK)</a>
    /// </summary>
    public string? CreditorId { get; set; }

    /// <summary>
    /// Jméno věřitele z číselníku bank <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=455007953">BankCode (CIS_KODY_BANK)</a> nebo jméno věřitele zadané uživatelem
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// True pokud je veřitel externí (=JPÚ - jiný peněžní ústav)<br />False pokud KB/MP (dáno vždy produktem)<br />null pokud je nedefinovaný
    /// </summary>
    public bool? IsExternal { get; set; }
}
