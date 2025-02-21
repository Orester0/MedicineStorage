using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.NotificationModels;
using MedicineStorage.Models.TemplateModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace MedicineStorage.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User, AppRole, int, IdentityUserClaim<int>,
        UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
    {

            //public class CustomNameValueGenerator : ValueGenerator<string>
            //{
            //    public override bool GeneratesTemporaryValues => false;

            //    public override string Next(EntityEntry entry)
            //    {
            //        var isDeleted = (bool)entry.Property("IsDeleted").CurrentValue!;
            //        var name = (string)entry.Property("Name").CurrentValue!;

            //        return isDeleted ? $"{name} [DELETED]" : name;
            //    }
            //}
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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditTemplate> AuditTemplates { get; set; }
        public DbSet<TenderTemplate> TenderTemplates { get; set; }
        public DbSet<MedicineRequestTemplate> MedicineRequestTemplates { get; set; }


        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable)
                {
                    entry.State = EntityState.Modified;
                    ((ISoftDeletable)entry.Entity).IsDeleted = true;
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Medicine>().HasQueryFilter(p => !p.IsDeleted);


           // var nameConverter = new ValueConverter<string, string>(
           //    v => v,
           //    v => v 
           //);

           // modelBuilder.Entity<Medicine>()
           //     .Property(m => m.Name)
           //     .HasConversion(nameConverter);

           // modelBuilder.Entity<Medicine>()
           //     .HasQueryFilter(m => !m.IsDeleted) 
           //     .Metadata
           //     .FindProperty(nameof(Medicine.Name))
           //     ?.SetValueGeneratorFactory((_, __) => new CustomNameValueGenerator());


            modelBuilder.Entity<AuditTemplate>(entity =>
            {
                entity.ToTable("AuditTemplates");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.RecurrenceInterval).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
            });

            modelBuilder.Entity<TenderTemplate>(entity =>
            {
                entity.ToTable("TenderTemplates");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.RecurrenceInterval).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
            });

            modelBuilder.Entity<MedicineRequestTemplate>(entity =>
            {
                entity.ToTable("MedicineRequestTemplates");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.RecurrenceInterval).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
            });

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

            modelBuilder.Entity<Medicine>()
                .Property(m => m.Stock)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Medicine>()
                .Property(m => m.MinimumStock)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<MedicineRequest>()
                .Property(m => m.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<MedicineUsage>()
                .Property(m => m.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<MedicineSupply>()
                .Property(m => m.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<AuditItem>(entity =>
            {
                entity.Property(a => a.ExpectedQuantity)
                    .HasColumnType("decimal(18,2)");
                entity.Property(a => a.ActualQuantity)
                    .HasColumnType("decimal(18,2)");
            });

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

                entity.HasMany(t => t.TenderItems)
                    .WithOne()
                    .HasForeignKey(ti => ti.TenderId);

                entity.HasMany(t => t.TenderProposals)
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

                entity.HasOne(mr => mr.Medicine)
                    .WithMany()
                    .HasForeignKey(mr => mr.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MedicineUsage>(entity =>
            {
                entity.HasOne(mu => mu.UsedByUser)
                    .WithMany()
                    .HasForeignKey(mu => mu.UsedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(mu => mu.Medicine)
                    .WithMany()
                    .HasForeignKey(mu => mu.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Audit>(entity =>
            {
                entity.HasOne(a => a.PlannedByUser)
                    .WithMany()
                    .HasForeignKey(a => a.PlannedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.ClosedByUser)
                    .WithMany()
                    .HasForeignKey(a => a.ClosedByUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasMany(a => a.AuditItems)
                    .WithOne()
                    .HasForeignKey(ai => ai.AuditId);
            });

            modelBuilder.Entity<AuditItem>(entity =>
            {
                entity.HasOne(ai => ai.Medicine)
                    .WithMany()
                    .HasForeignKey(ai => ai.MedicineId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MedicineSupply>(entity =>
            {
                entity.HasOne(ms => ms.Medicine)
                    .WithMany()
                    .HasForeignKey(ms => ms.MedicineId)
                    .OnDelete(DeleteBehavior.NoAction); 

            });
        }
    }
}

