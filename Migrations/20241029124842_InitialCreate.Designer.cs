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
    [Migration("20241029124842_InitialCreate")]
    partial class InitialCreate
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

                    b.Property<int>("AuditorId")
                        .HasColumnType("int");

                    b.Property<int?>("DoctorId")
                        .HasColumnType("int");

                    b.Property<string>("Findings")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuditorId");

                    b.HasIndex("DoctorId");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.AuditItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("ActualStock")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AuditId")
                        .HasColumnType("int");

                    b.Property<decimal>("Discrepancy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal>("SystemStock")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AuditId");

                    b.HasIndex("MedicineId");

                    b.ToTable("AuditItems");
                });

            modelBuilder.Entity("MedicineStorage.Models.DistributorModels.BidItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BidId")
                        .HasColumnType("int");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BidId");

                    b.HasIndex("MedicineId");

                    b.ToTable("BidItems");
                });

            modelBuilder.Entity("MedicineStorage.Models.DistributorModels.Distributor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Distributors");
                });

            modelBuilder.Entity("MedicineStorage.Models.DistributorModels.DistributorBid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DistributorId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TenderId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("DistributorId");

                    b.HasIndex("TenderId");

                    b.ToTable("DistributorBids");
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

                    b.Property<decimal>("CurrentStock")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsControlled")
                        .HasColumnType("bit");

                    b.Property<decimal>("MinimumStock")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("RequiresSpecialApproval")
                        .HasColumnType("bit");

                    b.Property<string>("StorageRequirements")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

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

                    b.Property<int?>("ApproverId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRecurring")
                        .HasColumnType("bit");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int?>("RecurringIntervalDays")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RequesterId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequiredForDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("RequesterId");

                    b.ToTable("MedicineRequests");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineRequestItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Justification")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.HasIndex("RequestId");

                    b.ToTable("MedicineRequestItems");
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
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UsageDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.HasIndex("UserId");

                    b.ToTable("MedicineUsages");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.RequestApproval", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ApproverId")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("RequestId");

                    b.ToTable("RequestApprovals");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.StockAdjustment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AdjustmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.HasIndex("UserId");

                    b.ToTable("StockAdjustments");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.Tender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("WinningBidId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WinningBidId");

                    b.ToTable("Tenders");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.TenderRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<int>("TenderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.HasIndex("TenderId");

                    b.ToTable("TenderRequests");
                });

            modelBuilder.Entity("MedicineStorage.Models.UserModels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.Audit", b =>
                {
                    b.HasOne("MedicineStorage.Models.UserModels", "Auditor")
                        .WithMany("ConductedAudits")
                        .HasForeignKey("AuditorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.UserModels", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Auditor");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.AuditItem", b =>
                {
                    b.HasOne("MedicineStorage.Models.AuditModels.Audit", "Audit")
                        .WithMany("Items")
                        .HasForeignKey("AuditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany()
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Audit");

                    b.Navigation("Medicine");
                });

            modelBuilder.Entity("MedicineStorage.Models.DistributorModels.BidItem", b =>
                {
                    b.HasOne("MedicineStorage.Models.DistributorModels.DistributorBid", "Bid")
                        .WithMany("Items")
                        .HasForeignKey("BidId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany()
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bid");

                    b.Navigation("Medicine");
                });

            modelBuilder.Entity("MedicineStorage.Models.DistributorModels.DistributorBid", b =>
                {
                    b.HasOne("MedicineStorage.Models.DistributorModels.Distributor", "Distributor")
                        .WithMany("Bids")
                        .HasForeignKey("DistributorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.TenderModels.Tender", "Tender")
                        .WithMany("Bids")
                        .HasForeignKey("TenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distributor");

                    b.Navigation("Tender");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineRequest", b =>
                {
                    b.HasOne("MedicineStorage.Models.UserModels", "Approver")
                        .WithMany()
                        .HasForeignKey("ApproverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MedicineStorage.Models.UserModels", "Requester")
                        .WithMany("Requests")
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Approver");

                    b.Navigation("Requester");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineRequestItem", b =>
                {
                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany("RequestItems")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.MedicineModels.MedicineRequest", "Request")
                        .WithMany("Items")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineUsage", b =>
                {
                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany("UsageRecords")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.UserModels", "UserModels")
                        .WithMany("MedicineUsages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("UserModels");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.RequestApproval", b =>
                {
                    b.HasOne("MedicineStorage.Models.UserModels", "Approver")
                        .WithMany()
                        .HasForeignKey("ApproverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.MedicineModels.MedicineRequest", "Request")
                        .WithMany("Approvals")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Approver");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.StockAdjustment", b =>
                {
                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany("StockAdjustments")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.UserModels", "UserModels")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("UserModels");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.Tender", b =>
                {
                    b.HasOne("MedicineStorage.Models.DistributorModels.DistributorBid", "WinningBid")
                        .WithMany()
                        .HasForeignKey("WinningBidId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("WinningBid");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.TenderRequest", b =>
                {
                    b.HasOne("MedicineStorage.Models.MedicineModels.MedicineRequest", "Request")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.TenderModels.Tender", "Tender")
                        .WithMany("Requests")
                        .HasForeignKey("TenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Request");

                    b.Navigation("Tender");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.Audit", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("MedicineStorage.Models.DistributorModels.Distributor", b =>
                {
                    b.Navigation("Bids");
                });

            modelBuilder.Entity("MedicineStorage.Models.DistributorModels.DistributorBid", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.Medicine", b =>
                {
                    b.Navigation("RequestItems");

                    b.Navigation("StockAdjustments");

                    b.Navigation("UsageRecords");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineRequest", b =>
                {
                    b.Navigation("Approvals");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.Tender", b =>
                {
                    b.Navigation("Bids");

                    b.Navigation("Requests");
                });

            modelBuilder.Entity("MedicineStorage.Models.UserModels", b =>
                {
                    b.Navigation("ConductedAudits");

                    b.Navigation("MedicineUsages");

                    b.Navigation("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}
