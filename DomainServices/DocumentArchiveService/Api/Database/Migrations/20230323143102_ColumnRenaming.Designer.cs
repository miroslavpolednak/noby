﻿// <auto-generated />
using System;
using DomainServices.DocumentArchiveService.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DomainServices.DocumentArchiveService.Api.Database.Migrations
{
    [DbContext(typeof(DocumentArchiveDbContext))]
    [Migration("20230323143102_ColumnRenaming")]
    partial class ColumnRenaming
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DomainServices.DocumentArchiveService.Api.Database.Entities.DocumentInterface", b =>
                {
                    b.Property<string>("DocumentId")
                        .HasColumnType("varchar(30)")
                        .HasColumnName("DOCUMENT_ID");

                    b.Property<string>("AuthorUserLogin")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnName("AUTHOR_USER_LOGIN");

                    b.Property<long>("CaseId")
                        .HasColumnType("bigint")
                        .HasColumnName("CASEID");

                    b.Property<string>("ContractNumber")
                        .HasColumnType("varchar(13)")
                        .HasColumnName("CONTRACT_NUMBER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime")
                        .HasColumnName("CREATED_ON");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(254)")
                        .HasColumnName("DESCRIPTION");

                    b.Property<byte[]>("DocumentData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("DOCUMENT_DATA");

                    b.Property<string>("DocumentDirection")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(1)")
                        .HasDefaultValue("E")
                        .HasColumnName("DOCUMENT_DIRECTION");

                    b.Property<int>("EaCodeMainId")
                        .HasColumnType("int")
                        .HasColumnName("EA_CODE_MAIN_ID");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("FILENAME");

                    b.Property<string>("FileNameSuffix")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnName("FILENAME_SUFFIX");

                    b.Property<string>("FolderDocument")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(1)")
                        .HasDefaultValue("N")
                        .HasColumnName("FOLDER_DOCUMENT");

                    b.Property<string>("FolderDocumentId")
                        .HasColumnType("varchar(30)")
                        .HasColumnName("FOLDER_DOCUMENT_ID");

                    b.Property<string>("FormId")
                        .HasColumnType("varchar(15)")
                        .HasColumnName("FORMID");

                    b.Property<byte>("Kdv")
                        .HasColumnType("tinyint")
                        .HasColumnName("KDV");

                    b.Property<byte>("SendDocumentOnly")
                        .HasColumnType("tinyint")
                        .HasColumnName("SEND_DOCUMENT_ONLY");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(100)
                        .HasColumnName("STATUS");

                    b.Property<string>("StatusErrorText")
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("STATUS_ERROR_TEXT");

                    b.HasKey("DocumentId");

                    b.ToTable("DocumentInterface");
                });
#pragma warning restore 612, 618
        }
    }
}
