﻿// <auto-generated />
using System;
using Console_AuditMigrator.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Console_AuditMigrator.Database.Migrations
{
    [DbContext(typeof(LogDbContext))]
    partial class LogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Console_AuditMigrator.Database.Entities.ApplicationLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Assembly")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CisAppKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CisUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CisUserIdent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LogType")
                        .HasColumnType("int");

                    b.Property<string>("MachineName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProcessedFileId")
                        .HasColumnType("int");

                    b.Property<string>("RequestId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceContext")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpanId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThreadId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TraceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProcessedFileId");

                    b.ToTable("ApplicationLogs");
                });

            modelBuilder.Entity("Console_AuditMigrator.Database.Entities.ProcessedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ProcessedFiles");
                });

            modelBuilder.Entity("Console_AuditMigrator.Database.Entities.ApplicationLog", b =>
                {
                    b.HasOne("Console_AuditMigrator.Database.Entities.ProcessedFile", "ProcessedFile")
                        .WithMany("ApplicationLogs")
                        .HasForeignKey("ProcessedFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProcessedFile");
                });

            modelBuilder.Entity("Console_AuditMigrator.Database.Entities.ProcessedFile", b =>
                {
                    b.Navigation("ApplicationLogs");
                });
#pragma warning restore 612, 618
        }
    }
}