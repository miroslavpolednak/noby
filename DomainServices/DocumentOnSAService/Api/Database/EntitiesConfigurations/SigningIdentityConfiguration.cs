using DomainServices.DocumentOnSAService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainServices.DocumentOnSAService.Api.Database.EntitiesConfigurations;

public class SigningIdentityConfiguration : IEntityTypeConfiguration<SigningIdentity>
{
    public void Configure(EntityTypeBuilder<SigningIdentity> builder)
    {
        builder.HasKey(e => e.Id);

        if (DocumentOnSAServiceDbContext.IsSqlite)
        {
            builder.Ignore(e => e.SigningIdentityJson);
        }
        else
        {
            builder.OwnsOne(signingIdentity => signingIdentity.SigningIdentityJson, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.OwnsMany(ci => ci.CustomerIdentifiers);
                ownedNavigationBuilder.OwnsOne(mob => mob.MobilePhone);
            });
        }
    }
}
