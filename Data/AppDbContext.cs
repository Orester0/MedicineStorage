using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Tender;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
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
        public DbSet<MedicineSupply> MedicineSupplies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(u => u.UserRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.HasMany(r => r.UserRoles)
                    .WithOne(ur => ur.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<MedicineSupply>()
        .Property(m => m.Quantity)
        .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<TenderItem>()
                .Property(t => t.RequiredQuantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<TenderProposal>()
                .Property(t => t.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<TenderProposalItem>(entity =>
            {
                entity.Property(t => t.Quantity)
                    .HasColumnType("decimal(18,2)");

                entity.Property(t => t.UnitPrice)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Tender>(entity =>
            {
                entity.HasOne(t => t.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.OpenedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.OpenedByUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasOne(t => t.ClosedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.ClosedByUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasOne(t => t.WinnerSelectedByUser)
                    .WithMany()
                    .HasForeignKey(t => t.WinnerSelectedByUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasMany(t => t.Items)
                    .WithOne()
                    .HasForeignKey(ti => ti.TenderId);

                entity.HasMany(t => t.Proposals)
                    .WithOne()
                    .HasForeignKey(tp => tp.TenderId);
            });

            modelBuilder.Entity<TenderProposal>(entity =>
            {
                entity.HasOne(tp => tp.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(tp => tp.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(tp => tp.Items)
                    .WithOne()
                    .HasForeignKey(tpi => tpi.TenderProposalId);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasMany(m => m.Requests)
                    .WithOne()
                    .HasForeignKey(mr => mr.MedicineId);

                entity.HasMany(m => m.UsageRecords)
                    .WithOne()
                    .HasForeignKey(mu => mu.MedicineId);
            });

            modelBuilder.Entity<MedicineRequest>(entity =>
            {
                entity.HasOne(mr => mr.RequestedByUser)
                    .WithMany()
                    .HasForeignKey(mr => mr.RequestedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(mr => mr.ApprovedByUser)
                    .WithMany()
                    .HasForeignKey(mr => mr.ApprovedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(mr => mr.MedicineUsages)
                    .WithOne()
                    .HasForeignKey(mu => mu.MedicineRequestId);
            });

            modelBuilder.Entity<MedicineUsage>(entity =>
            {
                entity.HasOne(mu => mu.UsedByUser)
                    .WithMany()
                    .HasForeignKey(mu => mu.UsedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(mu => mu.MedicineRequest)
                    .WithMany(mr => mr.MedicineUsages)
                    .HasForeignKey(mu => mu.MedicineRequestId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Audit>(entity =>
            {
                entity.HasOne(a => a.PlannedByUser)
                    .WithMany()
                    .HasForeignKey(a => a.PlannedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.ExecutedByUser)
                    .WithMany()
                    .HasForeignKey(a => a.ExecutedByUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasMany(a => a.AuditItems)
                    .WithOne()
                    .HasForeignKey(ai => ai.AuditId);
            });
        }
    }
}

