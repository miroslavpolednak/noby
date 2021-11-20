using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;

namespace DomainServices.CustomerService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class KonsDbRepository : DapperBaseRepository<KonsDbRepository>
{
    #region sql
    const string sqlGetDetail = @"Select Id, RodneCisloIco, Titul, TitulZa, Jmeno, Prijmeni,
        DatumNarozeni, MistoNarozeni, Pohlavi, ZdrojDat, TypDokladu, PreukazPlatnostDo,
        PrukazTotoznosti, PrukazVydal, PrukazStatVydaniId, PrukazVydalDatum,
        Ulice, VypisyUlice, CisloDomu2, VypisyCisloDomu2, CisloDomu1, VypisyCisloDomu1,
        Psc, VypisyPsc, Misto, VypisyMisto,
        b.KontaktId, b.PrimarniKontakt, b.TypKontaktu, b.[Value] 
        From dbo.PARTNER a
        Left Join (Select Id As KontaktId, PrimarniKontakt, TypKontaktu, [Value], PartnerId
                    From dbo.PartnerKontakty
                    Where PartnerId = @partnerId And PlatnostOd > GetDate() And (PlatnostDo Is Null Or PlatnostDo < GetDate())) b On a.Id = b.PartnerId
        Where a.Id = @partnerId";

    const string sqlGetBasicByIdentifier = @"Select Top 2 Id, RodneCisloIco, Titul, TitulZa, Jmeno, Prijmeni,
        DatumNarozeni, MistoNarozeni, Pohlavi, ZdrojDat, TypDokladu, PreukazPlatnostDo,
        PrukazTotoznosti, PrukazVydal, PrukazStatVydaniId, PrukazVydalDatum
        From dbo.PARTNER
        Where RodneCisloIco = @identifier";

    const string sqlGetBasicByFullIdentification = @"Select Top 2 Id, RodneCisloIco, Titul, TitulZa, Jmeno, Prijmeni,
        DatumNarozeni, MistoNarozeni, Pohlavi, ZdrojDat, TypDokladu, PreukazPlatnostDo,
        PrukazTotoznosti, PrukazVydal, PrukazStatVydaniId, PrukazVydalDatum
        From dbo.PARTNER
        Where Prijmeni = @lastName And Jmeno = @fistName And DatumNarozeni = @dateOfBirth And PrukazTotoznosti = @identificationDocument";

    const string sqlGetList = @"Select Id, RodneCisloIco, Titul, TitulZa, Jmeno, Prijmeni,
        DatumNarozeni, MistoNarozeni, Pohlavi, ZdrojDat, TypDokladu, PreukazPlatnostDo,
        PrukazTotoznosti, PrukazVydal, PrukazStatVydaniId, PrukazVydalDatum
        From dbo.PARTNER
        Where Id in @identities";
    #endregion

    public KonsDbRepository(ILogger<KonsDbRepository> logger, IConnectionProvider<KonsDbRepository> connectionProvider) : base(logger, connectionProvider)
    { }

    public async Task<List<Entities.Partner>> GetBasic(string identifier)
        => await WithConnection(async t => (await t.QueryAsync<Entities.Partner>(sqlGetBasicByIdentifier, new { identifier })).AsList());

    public async Task<List<Entities.Partner>> GetList(List<int> identities)
        => await WithConnection(async t => (await t.QueryAsync<Entities.Partner>(sqlGetList, new { identities = identities.ToArray() })).AsList());

    public async Task<Entities.Partner> GetDetail(int partnerId)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await WithConnection(async t => (await t.QueryAsync<Entities.Partner, Entities.PartnerKontakt, Entities.Partner>(
            sqlGetDetail,
            (partner, kontakt) => 
            {
                if (kontakt != null)
                    partner.Kontakty.Add(kontakt);

                return partner;
            },
            new { partnerId },
            splitOn: "KontaktId")).FirstOrDefault());
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async Task<List<Entities.Partner>> GetBasic(Contracts.GetBasicDataByFullIdentificationRequest fullIdentification)
    {
        //TODO: jak ma fungovat logika where?
        //TODO: typy dokladu
        return await WithConnection(async t => (await t.QueryAsync<Entities.Partner>(sqlGetBasicByFullIdentification, 
            new { 
                lastName = fullIdentification.LastName,
                fistName = fullIdentification.FirstName,
                dateOfBirth = fullIdentification.DateOfBirth,
                identificationDocument = fullIdentification.IdentificationDocument.Number
            })).AsList());
    }


}
