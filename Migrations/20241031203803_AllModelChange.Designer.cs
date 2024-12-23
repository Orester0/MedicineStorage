﻿// <auto-generated />
using System;
using MedicineStorage.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MedicineStorage.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241031203803_AllModelChange")]
    partial class AllModelChange
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AuditDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ConductedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConductedByUserId");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.AuditItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("ActualQuantity")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AuditId")
                        .HasColumnType("int");

                    b.Property<string>("Discrepancy")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<decimal>("ExpectedQuantity")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuditId");

                    b.HasIndex("MedicineId");

                    b.ToTable("AuditItems");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.Medicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("MinimumStock")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("RequiresSpecialApproval")
                        .HasColumnType("bit");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ApprovedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("Justification")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RequestType")
                        .HasColumnType("int");

                    b.Property<int>("RequestedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RequiredByDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApprovedByUserId");

                    b.HasIndex("MedicineId");

                    b.HasIndex("RequestedByUserId");

                    b.ToTable("MedicineRequests");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineUsage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UsageDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.HasIndex("UsedByUserId");

                    b.ToTable("MedicineUsages");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BatchNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<decimal>("PurchasePrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BatchNumber")
                        .IsUnique();

                    b.HasIndex("MedicineId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.Tender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeadlineDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Tenders");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.TenderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<decimal>("RequiredQuantity")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TenderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.HasIndex("TenderId");

                    b.ToTable("TenderItems");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.TenderProposal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DistributorId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TenderId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("DistributorId");

                    b.HasIndex("TenderId");

                    b.ToTable("TenderProposals");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.TenderProposalItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TenderProposalId")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.HasIndex("TenderProposalId");

                    b.ToTable("TenderProposalItems");
                });

            modelBuilder.Entity("MedicineStorage.Models.UserModels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.Audit", b =>
                {
                    b.HasOne("MedicineStorage.Models.UserModels", "ConductedByUser")
                        .WithMany("ConductedAudits")
                        .HasForeignKey("ConductedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ConductedByUser");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.AuditItem", b =>
                {
                    b.HasOne("MedicineStorage.Models.AuditModels.Audit", "Audit")
                        .WithMany("AuditItems")
                        .HasForeignKey("AuditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany()
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Audit");

                    b.Navigation("Medicine");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineRequest", b =>
                {
                    b.HasOne("MedicineStorage.Models.UserModels", "ApprovedByUser")
                        .WithMany()
                        .HasForeignKey("ApprovedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany("Requests")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.UserModels", "RequestedByUser")
                        .WithMany("Requests")
                        .HasForeignKey("RequestedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ApprovedByUser");

                    b.Navigation("Medicine");

                    b.Navigation("RequestedByUser");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineUsage", b =>
                {
                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany("UsageRecords")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.UserModels", "UsedByUser")
                        .WithMany()
                        .HasForeignKey("UsedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("UsedByUser");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.Stock", b =>
                {
                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany("StockRecords")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Medicine");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.Tender", b =>
                {
                    b.HasOne("MedicineStorage.Models.UserModels", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.TenderItem", b =>
                {
                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany()
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.TenderModels.Tender", "Tender")
                        .WithMany("Items")
                        .HasForeignKey("TenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("Tender");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.TenderProposal", b =>
                {
                    b.HasOne("MedicineStorage.Models.UserModels", "Distributor")
                        .WithMany()
                        .HasForeignKey("DistributorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.TenderModels.Tender", "Tender")
                        .WithMany("Proposals")
                        .HasForeignKey("TenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Distributor");

                    b.Navigation("Tender");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.TenderProposalItem", b =>
                {
                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany()
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.TenderModels.TenderProposal", "Proposal")
                        .WithMany("Items")
                        .HasForeignKey("TenderProposalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("Proposal");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.Audit", b =>
                {
                    b.Navigation("AuditItems");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.Medicine", b =>
                {
                    b.Navigation("Requests");

                    b.Navigation("StockRecords");

                    b.Navigation("UsageRecords");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.Tender", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("Proposals");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.TenderProposal", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("MedicineStorage.Models.UserModels", b =>
                {
                    b.Navigation("ConductedAudits");

                    b.Navigation("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}
