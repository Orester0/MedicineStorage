using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Tender;
using MedicineStorage.Models.TenderModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User, AppRole, int, IdentityUserClaim<int>,
        UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
    {

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Tender> Tenders { get; set; }
        public DbSet<TenderItem> TenderItems { get; set; }
        public DbSet<TenderProposal> TenderProposals { get; set; }
        public DbSet<TenderProposalItem> TenderProposalItems { get; set; }

        public DbSet<MedicineRequest> MedicineRequests { get; set; }
        public DbSet<MedicineUsage> MedicineUsages { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditItem> AuditItems { get; set; }
        public DbSet<MedicineSupply> InventoryTransactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MedicineSupply>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TenderProposalItemId)
                      .IsRequired();

                entity.Property(e => e.Quantity)
                      .IsRequired()
                      .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.TransactionDate)
                      .IsRequired();

                entity.HasOne(e => e.TenderProposalItem)
                      .WithMany() 
                      .HasForeignKey(e => e.TenderProposalItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

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

            modelBuilder.Entity<AuditItem>(entity =>
            {
                entity.Property(e => e.ActualQuantity)
                    .HasPrecision(10, 0);
                entity.Property(e => e.ExpectedQuantity)
                    .HasPrecision(10, 0);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.Property(e => e.MinimumStock)
                    .HasPrecision(12, 0);
                entity.Property(e => e.Stock)
                    .HasPrecision(12, 0);
            });

            modelBuilder.Entity<MedicineRequest>(entity =>
            {
                entity.Property(e => e.Quantity)
                    .HasPrecision(10, 0);
            });

            modelBuilder.Entity<MedicineUsage>(entity =>
            {
                entity.Property(e => e.Quantity)
                    .HasPrecision(10, 0);
            });


            modelBuilder.Entity<TenderProposal>(entity =>
            {
                entity.Property(e => e.TotalPrice)
                    .HasPrecision(12, 0);
            });

            modelBuilder.Entity<TenderProposalItem>(entity =>
            {
                entity.Property(e => e.Quantity)
                    .HasPrecision(10, 0);
            });

            modelBuilder.Entity<TenderProposalItem>(entity =>
            {
                entity.Property(e => e.UnitPrice)
                    .HasPrecision(12, 2);
            });

            modelBuilder.Entity<Audit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(a => a.PlannedByUser)
                    .WithMany()
                    .HasForeignKey(a => a.PlannedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.ExecutedByUser)
                    .WithMany()
                    .HasForeignKey(a => a.ExecutedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(a => a.AuditItems)
                    .WithOne(ai => ai.Audit)
                    .HasForeignKey(ai => ai.AuditId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AuditItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ActualQuantity).HasPrecision(10, 0);
                entity.Property(e => e.ExpectedQuantity).HasPrecision(10, 0);
                entity.HasOne(ai => ai.Medicine)
                    .WithMany()
                    .HasForeignKey(ai => ai.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MinimumStock).HasPrecision(12, 0);
                entity.Property(e => e.Stock).HasPrecision(12, 0);
                entity.HasMany(m => m.Requests)
                    .WithOne(r => r.Medicine)
                    .HasForeignKey(r => r.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(m => m.UsageRecords)
                    .WithOne(u => u.Medicine)
                    .HasForeignKey(u => u.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<MedicineRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasPrecision(10, 0);

                entity.HasOne(mr => mr.RequestedByUser)
                    .WithMany()
                    .HasForeignKey(mr => mr.RequestedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mr => mr.ApprovedByUser)
                    .WithMany()
                    .HasForeignKey(mr => mr.ApprovedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mr => mr.Medicine)
                    .WithMany(m => m.Requests)
                    .HasForeignKey(mr => mr.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(mr => mr.MedicineUsages)
                    .WithOne(mu => mu.MedicineRequest)
                    .HasForeignKey(mu => mu.MedicineRequestId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MedicineUsage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasPrecision(10, 0);

                entity.HasOne(mu => mu.MedicineRequest)
                    .WithMany(mr => mr.MedicineUsages) 
                    .HasForeignKey(mu => mu.MedicineRequestId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(mu => mu.Medicine)
                    .WithMany(m => m.UsageRecords)
                    .HasForeignKey(mu => mu.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mu => mu.UsedByUser)
                    .WithMany()
                    .HasForeignKey(mu => mu.UsedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Tender>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
                entity.HasOne(t => t.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.OpenedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.OpenedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.ClosedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.ClosedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.WinnerSelectedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.WinnerSelectedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(t => t.Items)
                    .WithOne(ti => ti.Tender)
                    .HasForeignKey(ti => ti.TenderId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(t => t.Proposals)
                    .WithOne(tp => tp.Tender)
                    .HasForeignKey(tp => tp.TenderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TenderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RequiredQuantity).HasPrecision(18, 2);
                entity.HasOne(ti => ti.Tender)
                    .WithMany(t => t.Items)
                    .HasForeignKey(ti => ti.TenderId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ti => ti.Medicine)
                    .WithMany()
                    .HasForeignKey(ti => ti.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<TenderProposal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalPrice).HasPrecision(12, 0);
                entity.HasOne(tp => tp.Tender)
                    .WithMany(t => t.Proposals)
                    .HasForeignKey(tp => tp.TenderId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(tp => tp.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(tp => tp.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(tp => tp.Items)
                    .WithOne(tpi => tpi.Proposal)
                    .HasForeignKey(tpi => tpi.TenderProposalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TenderProposalItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasPrecision(12, 2);
                entity.Property(e => e.Quantity).HasPrecision(10, 0);
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

