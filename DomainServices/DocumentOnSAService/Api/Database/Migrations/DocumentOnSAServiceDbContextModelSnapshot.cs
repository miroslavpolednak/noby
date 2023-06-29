﻿// <auto-generated />
using System;
using DomainServices.DocumentOnSAService.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations
{
    [DbContext(typeof(DocumentOnSAServiceDbContext))]
    partial class DocumentOnSAServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DomainServices.DocumentOnSAService.Api.Database.Entities.DocumentOnSa", b =>
                {
                    b.Property<int>("DocumentOnSAId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentOnSAId"));

                    b.Property<long?>("CaseId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("CustomerOnSAId1")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerOnSAId2")
                        .HasColumnType("int");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("DmsxId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("DocumentTemplateVariantId")
                        .HasColumnType("int");

                    b.Property<int?>("DocumentTemplateVersionId")
                        .HasColumnType("int");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("EArchivId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ExternalId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FormId")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("int");

                    b.Property<bool>("IsArchived")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsFinal")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPreviewSentToCustomer")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSigned")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsValid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int>("SalesArrangementId")
                        .HasColumnType("int");

                    b.Property<int?>("SignatureConfirmedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SignatureDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SignatureMethodCode")
                        .HasColumnType("nvarchar(15)");

                    b.Property<int?>("SignatureTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Source")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int?>("TaskId")
                        .HasColumnType("int");

                    b.HasKey("DocumentOnSAId");

                    b.HasIndex("SalesArrangementId");

                    b.ToTable("DocumentOnSa");
                });

            modelBuilder.Entity("DomainServices.DocumentOnSAService.Api.Database.Entities.EArchivIdsLinked", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DocumentOnSAId")
                        .HasColumnType("int");

                    b.Property<string>("EArchivId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentOnSAId");

                    b.ToTable("EArchivIdsLinked");
                });

            modelBuilder.Entity("DomainServices.DocumentOnSAService.Api.Database.Entities.GeneratedFormId", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFormIdFinal")
                        .HasColumnType("bit");

                    b.Property<string>("TargetSystem")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(2)")
                        .HasDefaultValue("N");

                    b.Property<short>("Version")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("HouseholdId");

                    b.ToTable("GeneratedFormId");
                });

            modelBuilder.Entity("DomainServices.DocumentOnSAService.Api.Database.Entities.SigningIdentity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DocumentOnSAId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DocumentOnSAId");

                    b.ToTable("SigningIdentity");
                });

            modelBuilder.Entity("DomainServices.DocumentOnSAService.Api.Database.Entities.EArchivIdsLinked", b =>
                {
                    b.HasOne("DomainServices.DocumentOnSAService.Api.Database.Entities.DocumentOnSa", "DocumentOnSa")
                        .WithMany("EArchivIdsLinkeds")
                        .HasForeignKey("DocumentOnSAId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentOnSa");
                });

            modelBuilder.Entity("DomainServices.DocumentOnSAService.Api.Database.Entities.SigningIdentity", b =>
                {
                    b.HasOne("DomainServices.DocumentOnSAService.Api.Database.Entities.DocumentOnSa", "DocumentOnSa")
                        .WithMany("SigningIdentities")
                        .HasForeignKey("DocumentOnSAId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DomainServices.DocumentOnSAService.Api.Database.Entities.SigningIdentityJson", "SigningIdentityJson", b1 =>
                        {
                            b1.Property<int>("SigningIdentityId")
                                .HasColumnType("int");

                            b1.Property<int?>("CustomerOnSAId")
                                .HasColumnType("int");

                            b1.Property<string>("EmailAddress")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FirstName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("LastName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("SignatureDataCode")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("SigningIdentityId");

                            b1.ToTable("SigningIdentity");

                            b1.ToJson("SigningIdentityJson");

                            b1.WithOwner()
                                .HasForeignKey("SigningIdentityId");

                            b1.OwnsMany("DomainServices.DocumentOnSAService.Api.Database.Entities.CustomerIdentifier", "CustomerIdentifiers", b2 =>
                                {
                                    b2.Property<int>("SigningIdentityJsonSigningIdentityId")
                                        .HasColumnType("int");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int");

                                    b2.Property<long>("IdentityId")
                                        .HasColumnType("bigint");

                                    b2.Property<string>("IdentityScheme")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("SigningIdentityJsonSigningIdentityId", "Id");

                                    b2.ToTable("SigningIdentity");

                                    b2.WithOwner()
                                        .HasForeignKey("SigningIdentityJsonSigningIdentityId");
                                });

                            b1.OwnsOne("DomainServices.DocumentOnSAService.Api.Database.Entities.MobilePhone", "MobilePhone", b2 =>
                                {
                                    b2.Property<int>("SigningIdentityJsonSigningIdentityId")
                                        .HasColumnType("int");

                                    b2.Property<string>("PhoneIDC")
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("PhoneNumber")
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("SigningIdentityJsonSigningIdentityId");

                                    b2.ToTable("SigningIdentity");

                                    b2.WithOwner()
                                        .HasForeignKey("SigningIdentityJsonSigningIdentityId");
                                });

                            b1.Navigation("CustomerIdentifiers");

                            b1.Navigation("MobilePhone");
                        });

                    b.Navigation("DocumentOnSa");

                    b.Navigation("SigningIdentityJson")
                        .IsRequired();
                });

            modelBuilder.Entity("DomainServices.DocumentOnSAService.Api.Database.Entities.DocumentOnSa", b =>
                {
                    b.Navigation("EArchivIdsLinkeds");

                    b.Navigation("SigningIdentities");
                });
#pragma warning restore 612, 618
        }
    }
}
