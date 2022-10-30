﻿// <auto-generated />
using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    [DbContext(typeof(ConfigurationContext))]
    partial class ConfigurationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataField", b =>
                {
                    b.Property<int>("DataFieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DataFieldId"), 1L, 1);

                    b.Property<int>("DataServiceId")
                        .HasColumnType("int");

                    b.Property<string>("DefaultStringFormat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FieldPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DataFieldId");

                    b.HasIndex("DataServiceId");

                    b.ToTable("DataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DataService", b =>
                {
                    b.Property<int>("DataServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DataServiceId"), 1L, 1);

                    b.Property<string>("DataServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DataServiceId");

                    b.ToTable("DataService");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentId"), 1L, 1);

                    b.Property<string>("DocumentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentDataField", b =>
                {
                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<int>("DocumentVersion")
                        .HasColumnType("int");

                    b.Property<int>("DataFieldId")
                        .HasColumnType("int");

                    b.Property<string>("StringFormat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateFieldName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentId", "DocumentVersion", "DataFieldId");

                    b.HasIndex("DataFieldId");

                    b.ToTable("DocumentDataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentDynamicInputParameter", b =>
                {
                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<int>("DocumentVersion")
                        .HasColumnType("int");

                    b.Property<int>("InputParameterId")
                        .HasColumnType("int");

                    b.Property<int>("SourceDataFieldId")
                        .HasColumnType("int");

                    b.Property<int>("TargetDataServiceId")
                        .HasColumnType("int");

                    b.HasKey("DocumentId", "DocumentVersion", "InputParameterId");

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
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DataServiceId")
                        .HasColumnType("int");

                    b.Property<string>("TemplateFieldName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentId", "FieldPath");

                    b.HasIndex("DataServiceId");

                    b.ToTable("DocumentSpecialDataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormat", b =>
                {
                    b.Property<int>("DynamicStringFormatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DynamicStringFormatId"), 1L, 1);

                    b.Property<int>("DataFieldId")
                        .HasColumnType("int");

                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<int>("DocumentVersion")
                        .HasColumnType("int");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.HasKey("DynamicStringFormatId");

                    b.HasIndex("DocumentId", "DocumentVersion", "DataFieldId");

                    b.ToTable("DynamicStringFormat");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormatCondition", b =>
                {
                    b.Property<int>("DynamicStringFormatId")
                        .HasColumnType("int");

                    b.Property<int>("DynamicStringFormatDataFieldId")
                        .HasColumnType("int");

                    b.Property<string>("EqualToValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DynamicStringFormatId", "DynamicStringFormatDataFieldId");

                    b.HasIndex("DynamicStringFormatDataFieldId");

                    b.ToTable("DynamicStringFormatCondition");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DynamicStringFormatDataField", b =>
                {
                    b.Property<int>("DynamicStringFormatDataFieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DynamicStringFormatDataFieldId"), 1L, 1);

                    b.Property<string>("FieldPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DynamicStringFormatDataFieldId");

                    b.ToTable("DynamicStringFormatDataField");
                });

            modelBuilder.Entity("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.InputParameter", b =>
                {
                    b.Property<int>("InputParameterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InputParameterId"), 1L, 1);

                    b.Property<string>("InputParameterName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                    b.HasOne("CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities.DocumentDataField", null)
                        .WithMany("DynamicStringFormats")
                        .HasForeignKey("DocumentId", "DocumentVersion", "DataFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
