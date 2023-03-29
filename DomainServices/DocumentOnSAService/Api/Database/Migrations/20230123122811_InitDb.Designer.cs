﻿// <auto-generated />
using System;
using DomainServices.DocumentOnSAService.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations
{
    [DbContext(typeof(DocumentOnSAServiceDbContext))]
    [Migration("20230123122811_InitDb")]
    partial class InitDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DomainServices.DocumentOnSAService.Api.Database.Entities.DocumentOnSa", b =>
                {
                    b.Property<int>("DocumentOnSAId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentOnSAId"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedUserName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("DmsxId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("DocumentTemplateVersionId")
                        .HasColumnType("int");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("EArchivId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FormId")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("HouseholdId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDocumentArchived")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

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
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("DocumentOnSAId");

                    b.HasIndex("SalesArrangementId");

                    b.ToTable("DocumentOnSa");
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
#pragma warning restore 612, 618
        }
    }
}
