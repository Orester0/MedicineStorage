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
    [Migration("20241218193319_MedicineUsageCorrections")]
    partial class MedicineUsageCorrections
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MedicineStorage.Models.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExecutedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlannedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PlannedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExecutedByUserId");

                    b.HasIndex("PlannedByUserId");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.AuditItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("ActualQuantity")
                        .HasPrecision(10)
                        .HasColumnType("decimal(10,0)");

                    b.Property<int>("AuditId")
                        .HasColumnType("int");

                    b.Property<decimal>("ExpectedQuantity")
                        .HasPrecision(10)
                        .HasColumnType("decimal(10,0)");

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

                    b.Property<int>("AuditFrequencyDays")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal>("MinimumStock")
                        .HasPrecision(12)
                        .HasColumnType("decimal(12,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("RequiresSpecialApproval")
                        .HasColumnType("bit");

                    b.Property<bool>("RequiresStrictAudit")
                        .HasColumnType("bit");

                    b.Property<decimal>("Stock")
                        .HasPrecision(12)
                        .HasColumnType("decimal(12,0)");

                    b.HasKey("Id");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(10)
                        .HasColumnType("decimal(10,0)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RequestedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequiredByDate")
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

                    b.Property<int>("MedicineRequestId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(10)
                        .HasColumnType("decimal(10,0)");

                    b.Property<DateTime>("UsageDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.HasIndex("MedicineRequestId");

                    b.HasIndex("UsedByUserId");

                    b.ToTable("MedicineUsages");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.Tender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClosedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeadlineDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("OpenedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("WinnerSelectedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClosedByUserId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("OpenedByUserId");

                    b.HasIndex("WinnerSelectedByUserId");

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

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TenderId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(12)
                        .HasColumnType("decimal(12,0)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

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
                        .HasPrecision(10)
                        .HasColumnType("decimal(10,0)");

                    b.Property<int>("TenderProposalId")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasPrecision(12, 2)
                        .HasColumnType("decimal(12,2)");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.HasIndex("TenderProposalId");

                    b.ToTable("TenderProposalItems");
                });

            modelBuilder.Entity("MedicineStorage.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("MedicineStorage.Models.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int?>("AppRoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("AppRoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.Audit", b =>
                {
                    b.HasOne("MedicineStorage.Models.User", "ExecutedByUser")
                        .WithMany()
                        .HasForeignKey("ExecutedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.User", "PlannedByUser")
                        .WithMany()
                        .HasForeignKey("PlannedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ExecutedByUser");

                    b.Navigation("PlannedByUser");
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
                    b.HasOne("MedicineStorage.Models.User", "ApprovedByUser")
                        .WithMany()
                        .HasForeignKey("ApprovedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MedicineStorage.Models.MedicineModels.Medicine", "Medicine")
                        .WithMany("Requests")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.User", "RequestedByUser")
                        .WithMany()
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

                    b.HasOne("MedicineStorage.Models.MedicineModels.MedicineRequest", "MedicineRequest")
                        .WithMany("MedicineUsages")
                        .HasForeignKey("MedicineRequestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.User", "UsedByUser")
                        .WithMany()
                        .HasForeignKey("UsedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("MedicineRequest");

                    b.Navigation("UsedByUser");
                });

            modelBuilder.Entity("MedicineStorage.Models.TenderModels.Tender", b =>
                {
                    b.HasOne("MedicineStorage.Models.User", "ClosedByUser")
                        .WithMany()
                        .HasForeignKey("ClosedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.User", "OpenedByUser")
                        .WithMany()
                        .HasForeignKey("OpenedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.User", "WinnerSelectedByUser")
                        .WithMany()
                        .HasForeignKey("WinnerSelectedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ClosedByUser");

                    b.Navigation("CreatedByUser");

                    b.Navigation("OpenedByUser");

                    b.Navigation("WinnerSelectedByUser");
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
                    b.HasOne("MedicineStorage.Models.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.TenderModels.Tender", "Tender")
                        .WithMany("Proposals")
                        .HasForeignKey("TenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

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

            modelBuilder.Entity("MedicineStorage.Models.UserRole", b =>
                {
                    b.HasOne("MedicineStorage.Models.AppRole", null)
                        .WithMany("UserRoles")
                        .HasForeignKey("AppRoleId");

                    b.HasOne("MedicineStorage.Models.AppRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicineStorage.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("MedicineStorage.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("MedicineStorage.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("MedicineStorage.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("MedicineStorage.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicineStorage.Models.AppRole", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("MedicineStorage.Models.AuditModels.Audit", b =>
                {
                    b.Navigation("AuditItems");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.Medicine", b =>
                {
                    b.Navigation("Requests");

                    b.Navigation("UsageRecords");
                });

            modelBuilder.Entity("MedicineStorage.Models.MedicineModels.MedicineRequest", b =>
                {
                    b.Navigation("MedicineUsages");
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

            modelBuilder.Entity("MedicineStorage.Models.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
