using Core.Domain.Entities;
using Core.Domain.Entities.Base;
using Core.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Data;

public class CamAIContext : DbContext
{
    public CamAIContext() { }

    public CamAIContext(DbContextOptions<CamAIContext> options)
        : base(options) { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        //TODO: Add CreatedBy and ModifiedBy in BaseEntity and implement update here.
        foreach (var entry in ChangeTracker.Entries<BusinessEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTimeHelper.VNDateTime;
                    //entry.Entity.CreatedBy = //authenticateService.GetCurrentUser();
                    entry.Entity.ModifiedDate = DateTimeHelper.VNDateTime;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedDate = DateTimeHelper.VNDateTime;
                    //entry.Entity.ModifiedBy = //authenticateService.GetCurrentUser();
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<Shop> Shops { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Province> Provinces { get; set; }
    public virtual DbSet<District> Districts { get; set; }
    public virtual DbSet<Ward> Wards { get; set; }
    public virtual DbSet<ShopStatus> ShopStatuses { get; set; }
    public virtual DbSet<BrandStatus> BrandStatuses { get; set; }
    public virtual DbSet<AccountStatus> AccountStatuses { get; set; }
    public virtual DbSet<EdgeBox> EdgeBoxes { get; set; }
    public virtual DbSet<EdgeBoxStatus> EdgeBoxStatuses { get; set; }
    public virtual DbSet<EdgeBoxInstall> EdgeBoxInstalls { get; set; }
    public virtual DbSet<EdgeBoxInstallStatus> EdgeBoxInstallStatuses { get; set; }
    public virtual DbSet<EdgeBoxActivity> EdgeBoxActivities { get; set; }
    public virtual DbSet<Camera> Cameras { get; set; }
    public virtual DbSet<Request> Requests { get; set; }
    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }
    public virtual DbSet<RequestType> RequestTypes { get; set; }
    public virtual DbSet<RequestActivity> RequestActivities { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<TicketStatus> TicketStatuses { get; set; }
    public virtual DbSet<TicketType> TicketTypes { get; set; }
    public virtual DbSet<TicketActivity> TicketActivities { get; set; }
    public virtual DbSet<Behavior> Behaviors { get; set; }
    public virtual DbSet<BehaviorType> BehaviorTypes { get; set; }
    public virtual DbSet<Evidence> Evidences { get; set; }
    public virtual DbSet<EvidenceType> EvidenceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var adminRole = new Role { Id = 1, Name = "Admin" };
        var accountStatusActive = new AccountStatus { Id = 2, Name = "Active" };
        var adminAccount = new Account
        {
            Email = "admin@camai.com",
            Password = "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af",
            Name = "Admin",
            AccountStatusId = accountStatusActive.Id
        };

        // AccountStatus=New when account is created and its password have not been changed.
        modelBuilder
            .Entity<AccountStatus>()
            .HasData(
                new AccountStatus { Id = 1, Name = "New" },
                accountStatusActive,
                new AccountStatus { Id = 3, Name = "Inactive" }
            );
        modelBuilder
            .Entity<BrandStatus>()
            .HasData(new BrandStatus { Id = 1, Name = "Active" }, new BrandStatus { Id = 2, Name = "Inactive" });
        modelBuilder
            .Entity<ShopStatus>()
            .HasData(new ShopStatus { Id = 1, Name = "Active" }, new ShopStatus { Id = 2, Name = "Inactive" });

        modelBuilder
            .Entity<Role>()
            .HasData(
                adminRole,
                new Role { Id = 2, Name = "Technician" },
                new Role { Id = 3, Name = "Brand manager" },
                new Role { Id = 4, Name = "Shop manager" },
                new Role { Id = 5, Name = "Employee" }
            );

        modelBuilder.Entity<Account>(builder =>
        {
            const string roleId = "RoleId";
            const string accountId = "AccountId";

            builder.Property(e => e.Gender).HasConversion<string>();
            builder.HasData(adminAccount);
            builder
                .HasMany(e => e.Roles)
                .WithMany(e => e.Accounts)
                .UsingEntity(
                    r => r.HasOne(typeof(Role)).WithMany().HasForeignKey(roleId),
                    l => l.HasOne(typeof(Account)).WithMany().HasForeignKey(accountId),
                    je =>
                    {
                        je.HasKey(roleId, accountId);
                        je.HasData(new { AccountId = adminAccount.Id, RoleId = adminRole.Id });
                    }
                );
        });

        modelBuilder
            .Entity<EdgeBoxStatus>()
            .HasData(new EdgeBoxStatus { Id = 1, Name = "Active" }, new EdgeBoxStatus { Id = 2, Name = "Inactive" });

        modelBuilder
            .Entity<EdgeBoxInstallStatus>()
            .HasData(
                new EdgeBoxInstallStatus { Id = 1, Name = "Valid" },
                new EdgeBoxInstallStatus { Id = 2, Name = "Expired" }
            );

        modelBuilder
            .Entity<RequestStatus>()
            .HasData(
                new RequestStatus { Id = 1, Name = "Open" },
                new RequestStatus { Id = 2, Name = "Cancelled" },
                new RequestStatus { Id = 3, Name = "Done" },
                new RequestStatus { Id = 4, Name = "Rejected" }
            );

        modelBuilder
            .Entity<RequestType>()
            .HasData(
                new RequestType { Id = 1, Name = "Install" },
                new RequestType { Id = 2, Name = "Repair" },
                new RequestType { Id = 3, Name = "Remove" }
            );

        modelBuilder
            .Entity<TicketStatus>()
            .HasData(
                new TicketStatus { Id = 1, Name = "Open" },
                new TicketStatus { Id = 2, Name = "Cancelled" },
                new TicketStatus { Id = 3, Name = "Done" },
                new TicketStatus { Id = 4, Name = "Failed" }
            );

        modelBuilder
            .Entity<TicketType>()
            .HasData(
                new TicketType { Id = 1, Name = "Install" },
                new TicketType { Id = 2, Name = "Repair" },
                new TicketType { Id = 3, Name = "Remove" }
            );
    }
}
