﻿// <auto-generated />
using System;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Migrations
{
    [DbContext(typeof(NotificationDbContext))]
    [Migration("20221222163946_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction.Result", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Channel")
                        .HasColumnType("int");

                    b.Property<string>("CustomId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DocumentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Errors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("HandoverToMcsTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Identity")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdentityScheme")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("RequestTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomId", "Identity", "IdentityScheme", "DocumentId");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.EmailResult", b =>
                {
                    b.HasBaseType("CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction.Result");

                    b.ToTable("EmailResult", (string)null);
                });

            modelBuilder.Entity("CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.SmsResult", b =>
                {
                    b.HasBaseType("CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction.Result");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("SmsResult", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
