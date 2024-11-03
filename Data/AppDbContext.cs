using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MedicineStorage.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User, AppRole, int, IdentityUserClaim<int>,
        UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
    {

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<MedicineRequest> MedicineRequests { get; set; }
        public DbSet<Tender> Tenders { get; set; }
        public DbSet<TenderItem> TenderItems { get; set; }
        public DbSet<TenderProposal> TenderProposals { get; set; }
        public DbSet<TenderProposalItem> TenderProposalItems { get; set; }
        public DbSet<MedicineUsage> MedicineUsages { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditItem> AuditItems { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();

                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(255);
            });

            // UserRole configurations
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                entity.HasOne(ur => ur.Role)
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<Audit>()
            .HasOne(a => a.ConductedByUser)
            .WithMany()
            .HasForeignKey(a => a.ConductedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Audit>()
                .HasMany(a => a.AuditItems)
                .WithOne(ai => ai.Audit)
                .HasForeignKey(ai => ai.AuditId)
                .OnDelete(DeleteBehavior.Cascade);

            // AuditItem configurations
            modelBuilder.Entity<AuditItem>()
                .HasOne(ai => ai.Medicine)
                .WithMany()
                .HasForeignKey(ai => ai.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Medicine configurations
            modelBuilder.Entity<Medicine>()
                .HasMany(m => m.StockRecords)
                .WithOne(s => s.Medicine)
                .HasForeignKey(s => s.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medicine>()
                .HasMany(m => m.Requests)
                .WithOne(r => r.Medicine)
                .HasForeignKey(r => r.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medicine>()
                .HasMany(m => m.UsageRecords)
                .WithOne(u => u.Medicine)
                .HasForeignKey(u => u.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // MedicineRequest configurations
            modelBuilder.Entity<MedicineRequest>()
                .HasOne(mr => mr.RequestedByUser)
                .WithMany()
                .HasForeignKey(mr => mr.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicineRequest>()
                .HasOne(mr => mr.ApprovedByUser)
                .WithMany()
                .HasForeignKey(mr => mr.ApprovedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // MedicineUsage configurations
            modelBuilder.Entity<MedicineUsage>()
                .HasOne(mu => mu.UsedByUser)
                .WithMany()
                .HasForeignKey(mu => mu.UsedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicineUsage>()
                .HasOne(mu => mu.Stock)
                .WithMany()
                .HasForeignKey(mu => mu.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tender configurations
            modelBuilder.Entity<Tender>()
                .HasOne(t => t.CreatedByUser)
                .WithMany()
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tender>()
                .HasMany(t => t.Items)
                .WithOne(ti => ti.Tender)
                .HasForeignKey(ti => ti.TenderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tender>()
                .HasMany(t => t.Proposals)
                .WithOne(tp => tp.Tender)
                .HasForeignKey(tp => tp.TenderId)
                .OnDelete(DeleteBehavior.Cascade);

            // TenderItem configurations
            modelBuilder.Entity<TenderItem>()
                .HasOne(ti => ti.Medicine)
                .WithMany()
                .HasForeignKey(ti => ti.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tender configurations
            modelBuilder.Entity<Tender>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TenderItem configurations
            modelBuilder.Entity<TenderItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.RequiredQuantity).HasPrecision(18, 2);

                entity.HasOne(e => e.Tender)
                    .WithMany(t => t.Items)
                    .HasForeignKey(e => e.TenderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Medicine)
                    .WithMany()
                    .HasForeignKey(e => e.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<TenderProposal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TenderId).IsRequired();
                entity.Property(e => e.DistributorId).IsRequired();
                entity.Property(e => e.TotalPrice).IsRequired();
                entity.Property(e => e.SubmissionDate).IsRequired();
                entity.Property(e => e.Status).IsRequired();

                entity.HasOne(tp => tp.Distributor)
                    .WithMany()
                    .HasForeignKey(tp => tp.DistributorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tp => tp.Tender)
                    .WithMany(t => t.Proposals)
                    .HasForeignKey(tp => tp.TenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(tp => tp.Items)
                    .WithOne(tpi => tpi.Proposal)
                    .HasForeignKey(tpi => tpi.TenderProposalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // TenderProposalItem configurations
            modelBuilder.Entity<TenderProposalItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TenderProposalId).IsRequired();
                entity.Property(e => e.MedicineId).IsRequired();
                entity.Property(e => e.UnitPrice).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();

                entity.HasOne(tpi => tpi.Medicine)
                    .WithMany()
                    .HasForeignKey(tpi => tpi.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tpi => tpi.Proposal)
                    .WithMany(tp => tp.Items)
                    .HasForeignKey(tpi => tpi.TenderProposalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}

