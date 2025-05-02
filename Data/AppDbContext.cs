using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.NotificationModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace MedicineStorage.Data
{
    public class AppDbContext : IdentityDbContext<User, AppRole, int, IdentityUserClaim<int>,
    UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly string _defaultConnectionString;

        public AppDbContext(IDbConnectionStringProvider connectionStringProvider)
            : base(new DbContextOptions<AppDbContext>())
        {
            _connectionStringProvider = connectionStringProvider ??
                throw new ArgumentNullException(nameof(connectionStringProvider));
            _defaultConnectionString = null;
        }

        public AppDbContext(IDbConnectionStringProvider connectionStringProvider, IConfiguration configuration)
            : base(new DbContextOptions<AppDbContext>())
        {
            _connectionStringProvider = connectionStringProvider ??
                throw new ArgumentNullException(nameof(connectionStringProvider));
            _defaultConnectionString = configuration?.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString;

            try
            {
                connectionString = _connectionStringProvider.GetConnectionString();
            }
            catch (InvalidOperationException)
            {
                if (_defaultConnectionString != null)
                {
                    connectionString = _defaultConnectionString;
                }
                else
                {
                    throw new InvalidOperationException("No connection string available for AppDbContext.");
                }
            }

            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineCategory> MedicineCategories { get; set; }
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

        private readonly IDbConnectionStringProvider _connectionStringProvider;


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
            modelBuilder.Entity<Medicine>()
                .Navigation(m => m.Category)
                .AutoInclude();


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

            modelBuilder.Entity<MedicineRequest>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_MedicineRequest_Status_EnumConstraint",
                $"Status IN ({(int)RequestStatus.Pending}, {(int)RequestStatus.PedingWithSpecial}, {(int)RequestStatus.Approved}, {(int)RequestStatus.Rejected})"));
            modelBuilder.Entity<Audit>()
        .ToTable(t => t.HasCheckConstraint(
            "CK_Audit_Status_EnumConstraint",
            $"Status IN ({(int)AuditStatus.Planned}, {(int)AuditStatus.InProgress}, {(int)AuditStatus.SuccesfullyCompleted}, {(int)AuditStatus.CompletedWithProblems}, {(int)AuditStatus.Cancelled})"));
            modelBuilder.Entity<Tender>()
                    .ToTable(t => t.HasCheckConstraint(
                        "CK_Tender_Status_EnumConstraint",
                        $"Status IN ({(int)TenderStatus.Created}, {(int)TenderStatus.Published}, {(int)TenderStatus.Closed}, {(int)TenderStatus.Awarded}, {(int)TenderStatus.Executing}, {(int)TenderStatus.Executed}, {(int)TenderStatus.Cancelled})"));

            modelBuilder.Entity<TenderItem>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_TenderItem_Status_EnumConstraint",
                    $"Status IN ({(int)TenderItemStatus.Pending}, {(int)TenderItemStatus.Executed})"));

            modelBuilder.Entity<TenderProposal>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_TenderProposal_Status_EnumConstraint",
                    $"Status IN ({(int)ProposalStatus.Submitted}, {(int)ProposalStatus.Accepted}, {(int)ProposalStatus.Rejected})"));
            modelBuilder.Entity<Tender>()
                .ToTable(t => t.HasCheckConstraint("CK_Tender_DeadlineDate", "DeadlineDate >= PublishDate"));

            modelBuilder.Entity<Tender>()
                .ToTable(t => t.HasCheckConstraint("CK_Tender_ClosingDate", "ClosingDate IS NULL OR ClosingDate >= PublishDate"));
            modelBuilder.Entity<TenderProposal>()
    .ToTable(t => t.HasCheckConstraint("CK_TenderProposal_TotalPrice_Positive", "TotalPrice > 0"));

            modelBuilder.Entity<TenderItem>()
                .ToTable(t => t.HasCheckConstraint("CK_TenderItem_RequiredQuantity_Positive", "RequiredQuantity > 0"));

            modelBuilder.Entity<Audit>()
                .ToTable(t => t.HasCheckConstraint("CK_Audit_EndDate", "EndDate IS NULL OR EndDate >= StartDate"));

            modelBuilder.Entity<MedicineRequest>()
    .Property(x => x.Status)
    .HasDefaultValue(RequestStatus.Pending);


            modelBuilder.Entity<Audit>()
    .Property(x => x.Status)
    .HasDefaultValue(AuditStatus.Planned);


            modelBuilder.Entity<Tender>()
    .Property(x => x.Status)
    .HasDefaultValue(TenderStatus.Created);


            modelBuilder.Entity<TenderItem>()
    .Property(x => x.Status)
    .HasDefaultValue(TenderItemStatus.Pending);


            modelBuilder.Entity<TenderProposal>()
    .Property(x => x.Status)
    .HasDefaultValue(ProposalStatus.Submitted);



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

