using NOBY.Api.SharedDto;

namespace NOBY.Api.Endpoints.Customer.Shared;

public abstract class BaseCustomerDetail
{
    public NaturalPerson? NaturalPerson { get; set; }

    public JuridicalPerson? JuridicalPerson { get; set; }

    public IdentificationDocumentFull? IdentificationDocument { get; set; }

    public List<CIS.Foms.Types.Address>? Addresses { get; set; }

    public List<CustomerContact>? Contacts { get; set; }

    /// <summary>
    /// Přihlášen k aktualizaci dat ze základních registrů
    /// </summary>
    public bool? IsBrSubscribed { get; set; }

    /// <summary>
    /// Příznak podle kterého zobrazujeme na FE výsledek z našeptávače
    /// </summary>
    public bool? IsAddressWhispererUsed { get; set; }

    /// <summary>
    /// Zvláštní vztah ke Komerční bance
    /// </summary>
    public bool? HasRelationshipWithKB { get; set; }

    /// <summary>
    /// Osoba blízká zaměstnanci KB
    /// </summary>
    public bool? HasRelationshipWithKBEmployee { get; set; }

    /// <summary>
    /// Propojení s obchodní korporací
    /// </summary>
    public bool? HasRelationshipWithCorporate { get; set; }

    /// <summary>
    /// Politicky exponovaná osoba
    /// </summary>
    public bool? IsPoliticallyExposed { get; set; }

    /// <summary>
    /// Americký občan (FATCA)
    /// </summary>
    public bool? IsUSPerson { get; set; }
}

internal static class BaseCustomerDetailExtensions
{
    public static TCustomerDetail FillResponseDto<TCustomerDetail>(this DomainServices.CustomerService.Contracts.CustomerDetailResponse dsCustomer, TCustomerDetail newCustomer)
        where TCustomerDetail : BaseCustomerDetail
    {
        NaturalPerson person = new();
        dsCustomer.NaturalPerson?.FillResponseDto(person);
        person.EducationLevelId = dsCustomer.NaturalPerson?.EducationLevelId;
        //person.ProfessionCategoryId = customer.NaturalPerson?
        //person.ProfessionId = customer.NaturalPerson ?;
        //person.NetMonthEarningAmountId = customer.NaturalPerson
        //person.NetMonthEarningTypeId = customer.NaturalPerson ?;

        newCustomer.IsBrSubscribed = dsCustomer.NaturalPerson?.IsBrSubscribed;
        //newCustomer.HasRelationshipWithCorporate = customer.NaturalPerson?.HasRelationshipWithCorporate,
        //newCustomer.HasRelationshipWithKB = customer.NaturalPerson?.HasRelationshipWithKB,
        //newCustomer.HasRelationshipWithKBEmployee = customer.NaturalPerson?.HasRelationshipWithKBEmployee,
        //newCustomer.IsUSPerson = customer.NaturalPerson?.IsUSPerson,
        //newCustomer.IsAddressWhispererUsed = customer.NaturalPerson?.AddressWhispererUsed,
        //newCustomer.IsPoliticallyExposed = customer.NaturalPerson?.IsPoliticallyExposed,
        newCustomer.NaturalPerson = person;
        newCustomer.JuridicalPerson = null;
        newCustomer.IdentificationDocument = dsCustomer.IdentificationDocument?.ToResponseDto();
        newCustomer.Contacts = dsCustomer.Contacts?.ToResponseDto();
        newCustomer.Addresses = dsCustomer.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList();

        return newCustomer;
    }
}