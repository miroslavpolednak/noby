﻿// <auto-generated />
using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    [DbContext(typeof(ConfigurationContext))]
    [Migration("20221114121855_DynamicInputParameter_PK")]
    partial class DynamicInputParameterPK
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataField", b =>
                {
                    b.Property<int>("DataFieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DataFieldId"));

                    b.Property<int>("DataServiceId")
                        .HasColumnType("int");

                    b.Property<string>("DefaultStringFormat")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FieldPath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("DataFieldId");

                    b.HasIndex("DataServiceId");

                    b.ToTable("DataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataService", b =>
                {
                    b.Property<int>("DataServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DataServiceId"));

                    b.Property<string>("DataServiceName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("DataServiceId");

                    b.ToTable("DataService");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentId"));

                    b.Property<string>("DocumentName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("DocumentId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentDataField", b =>
                {
                    b.Property<int>("DocumentDataFieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentDataFieldId"));

                    b.Property<int>("DataFieldId")
                        .HasColumnType("int");

                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<string>("DocumentVersion")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("StringFormat")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TemplateFieldName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DocumentDataFieldId");

                    b.HasIndex("DataFieldId");

                    b.HasIndex("DocumentId", "DocumentVersion", "TemplateFieldName");

                    b.ToTable("DocumentDataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentDynamicInputParameter", b =>
                {
                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<string>("DocumentVersion")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<int>("InputParameterId")
                        .HasColumnType("int");

                    b.Property<int>("TargetDataServiceId")
                        .HasColumnType("int");

                    b.Property<int>("SourceDataFieldId")
                        .HasColumnType("int");

                    b.HasKey("DocumentId", "DocumentVersion", "InputParameterId", "TargetDataServiceId");

                    b.HasIndex("InputParameterId");

                    b.HasIndex("SourceDataFieldId");

                    b.HasIndex("TargetDataServiceId");

                    b.ToTable("DocumentDynamicInputParameter");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentSpecialDataField", b =>
                {
                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<string>("FieldPath")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("DataServiceId")
                        .HasColumnType("int");

                    b.Property<string>("StringFormat")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TemplateFieldName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DocumentId", "FieldPath");

                    b.HasIndex("DataServiceId");

                    b.ToTable("DocumentSpecialDataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormat", b =>
                {
                    b.Property<int>("DynamicStringFormatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DynamicStringFormatId"));

                    b.Property<int>("DocumentDataFieldId")
                        .HasColumnType("int");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.HasKey("DynamicStringFormatId");

                    b.HasIndex("DocumentDataFieldId");

                    b.ToTable("DynamicStringFormat");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormatCondition", b =>
                {
                    b.Property<int>("DynamicStringFormatId")
                        .HasColumnType("int");

                    b.Property<int>("DynamicStringFormatDataFieldId")
                        .HasColumnType("int");

                    b.Property<string>("EqualToValue")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DynamicStringFormatId", "DynamicStringFormatDataFieldId");

                    b.HasIndex("DynamicStringFormatDataFieldId");

                    b.ToTable("DynamicStringFormatCondition");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormatDataField", b =>
                {
                    b.Property<int>("DynamicStringFormatDataFieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DynamicStringFormatDataFieldId"));

                    b.Property<string>("FieldPath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("DynamicStringFormatDataFieldId");

                    b.ToTable("DynamicStringFormatDataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.EasFormDataField", b =>
                {
                    b.Property<int>("EasFormDataFieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EasFormDataFieldId"));

                    b.Property<int>("DataFieldId")
                        .HasColumnType("int");

                    b.Property<int>("EasFormTypeId")
                        .HasColumnType("int");

                    b.Property<int>("EasRequestTypeId")
                        .HasColumnType("int");

                    b.Property<string>("JsonPropertyName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("EasFormDataFieldId");

                    b.HasIndex("DataFieldId");

                    b.HasIndex("EasFormTypeId");

                    b.HasIndex("EasRequestTypeId", "EasFormTypeId", "JsonPropertyName")
                        .IsUnique();

                    b.ToTable("EasFormDataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.EasFormDynamicInputParameter", b =>
                {
                    b.Property<int>("EasRequestTypeId")
                        .HasColumnType("int");

                    b.Property<int>("InputParameterId")
                        .HasColumnType("int");

                    b.Property<int>("TargetDataServiceId")
                        .HasColumnType("int");

                    b.Property<int>("SourceDataFieldId")
                        .HasColumnType("int");

                    b.HasKey("EasRequestTypeId", "InputParameterId", "TargetDataServiceId");

                    b.HasIndex("InputParameterId");

                    b.HasIndex("SourceDataFieldId");

                    b.HasIndex("TargetDataServiceId");

                    b.ToTable("EasFormDynamicInputParameter");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.EasFormType", b =>
                {
                    b.Property<int>("EasFormTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EasFormTypeId"));

                    b.Property<string>("EasFormTypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EasFormTypeId");

                    b.ToTable("EasFormType");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.EasRequestType", b =>
                {
                    b.Property<int>("EasRequestTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EasRequestTypeId"));

                    b.Property<string>("EasRequestTypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("EasRequestTypeId");

                    b.ToTable("EasRequestType");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.InputParameter", b =>
                {
                    b.Property<int>("InputParameterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InputParameterId"));

                    b.Property<string>("InputParameterName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("InputParameterId");

                    b.ToTable("InputParameter");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataField", b =>
                {
                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataService", "DataService")
                        .WithMany()
                        .HasForeignKey("DataServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataService");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentDataField", b =>
                {
                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataField", "DataField")
                        .WithMany()
                        .HasForeignKey("DataFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.Document", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataField");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentDynamicInputParameter", b =>
                {
                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.Document", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.InputParameter", "InputParameter")
                        .WithMany()
                        .HasForeignKey("InputParameterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataField", "SourceDataField")
                        .WithMany()
                        .HasForeignKey("SourceDataFieldId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataService", "TargetDataService")
                        .WithMany()
                        .HasForeignKey("TargetDataServiceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("InputParameter");

                    b.Navigation("SourceDataField");

                    b.Navigation("TargetDataService");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentSpecialDataField", b =>
                {
                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataService", "DataService")
                        .WithMany()
                        .HasForeignKey("DataServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.Document", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataService");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormat", b =>
                {
                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentDataField", "DocumentDataField")
                        .WithMany("DynamicStringFormats")
                        .HasForeignKey("DocumentDataFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentDataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormatCondition", b =>
                {
                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormatDataField", "DynamicStringFormatDataField")
                        .WithMany()
                        .HasForeignKey("DynamicStringFormatDataFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormat", null)
                        .WithMany("DynamicStringFormatConditions")
                        .HasForeignKey("DynamicStringFormatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DynamicStringFormatDataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.EasFormDataField", b =>
                {
                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataField", "DataField")
                        .WithMany()
                        .HasForeignKey("DataFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.EasFormType", "EasFormType")
                        .WithMany()
                        .HasForeignKey("EasFormTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.EasRequestType", "EasRequestType")
                        .WithMany()
                        .HasForeignKey("EasRequestTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataField");

                    b.Navigation("EasFormType");

                    b.Navigation("EasRequestType");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.EasFormDynamicInputParameter", b =>
                {
                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.EasRequestType", "EasRequestType")
                        .WithMany()
                        .HasForeignKey("EasRequestTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.InputParameter", "InputParameter")
                        .WithMany()
                        .HasForeignKey("InputParameterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataField", "SourceDataField")
                        .WithMany()
                        .HasForeignKey("SourceDataFieldId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataService", "TargetDataService")
                        .WithMany()
                        .HasForeignKey("TargetDataServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EasRequestType");

                    b.Navigation("InputParameter");

                    b.Navigation("SourceDataField");

                    b.Navigation("TargetDataService");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentDataField", b =>
                {
                    b.Navigation("DynamicStringFormats");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormat", b =>
                {
                    b.Navigation("DynamicStringFormatConditions");
                });
#pragma warning restore 612, 618
        }
    }
}
