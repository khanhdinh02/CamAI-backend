using Core.Domain.DTO;
using Core.Domain.DTO.EdgeBoxes;
using Core.Domain.DTO.Requests;
using Core.Domain.DTO.Tickets;
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

    public virtual DbSet<Account> Accounts { get; set; } = null!;
    public virtual DbSet<Brand> Brands { get; set; } = null!;
    public virtual DbSet<Shop> Shops { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Province> Provinces { get; set; } = null!;
    public virtual DbSet<District> Districts { get; set; } = null!;
    public virtual DbSet<Ward> Wards { get; set; } = null!;
    public virtual DbSet<ShopStatus> ShopStatuses { get; set; } = null!;
    public virtual DbSet<BrandStatus> BrandStatuses { get; set; } = null!;
    public virtual DbSet<AccountStatus> AccountStatuses { get; set; } = null!;
    public virtual DbSet<EdgeBox> EdgeBoxes { get; set; } = null!;
    public virtual DbSet<EdgeBoxStatus> EdgeBoxStatuses { get; set; } = null!;
    public virtual DbSet<EdgeBoxInstall> EdgeBoxInstalls { get; set; } = null!;
    public virtual DbSet<EdgeBoxInstallStatus> EdgeBoxInstallStatuses { get; set; } = null!;
    public virtual DbSet<EdgeBoxActivity> EdgeBoxActivities { get; set; } = null!;
    public virtual DbSet<Camera> Cameras { get; set; } = null!;
    public virtual DbSet<Request> Requests { get; set; } = null!;
    public virtual DbSet<RequestStatus> RequestStatuses { get; set; } = null!;
    public virtual DbSet<RequestType> RequestTypes { get; set; } = null!;
    public virtual DbSet<RequestActivity> RequestActivities { get; set; } = null!;
    public virtual DbSet<Ticket> Tickets { get; set; } = null!;
    public virtual DbSet<TicketStatus> TicketStatuses { get; set; } = null!;
    public virtual DbSet<TicketType> TicketTypes { get; set; } = null!;
    public virtual DbSet<TicketActivity> TicketActivities { get; set; } = null!;
    public virtual DbSet<Behavior> Behaviors { get; set; } = null!;
    public virtual DbSet<BehaviorType> BehaviorTypes { get; set; } = null!;
    public virtual DbSet<Evidence> Evidences { get; set; } = null!;
    public virtual DbSet<EvidenceType> EvidenceTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var adminRole = new Role { Id = RoleEnum.Admin, Name = "Admin" };
        var accountStatusActive = new AccountStatus { Id = AccountStatusEnum.Active, Name = "Active" };
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
                new AccountStatus { Id = AccountStatusEnum.New, Name = "New" },
                accountStatusActive,
                new AccountStatus { Id = AccountStatusEnum.Inactive, Name = "Inactive" }
            );
        modelBuilder
            .Entity<BrandStatus>()
            .HasData(
                new BrandStatus { Id = BrandStatusEnum.Active, Name = "Active" },
                new BrandStatus { Id = BrandStatusEnum.Inactive, Name = "Inactive" }
            );
        modelBuilder
            .Entity<ShopStatus>()
            .HasData(
                new ShopStatus { Id = ShopStatusEnum.Active, Name = "Active" },
                new ShopStatus { Id = ShopStatusEnum.Inactive, Name = "Inactive" },
                new ShopStatus { Id = ShopStatusEnum.Pending, Name = "Pending" }
            );

        modelBuilder
            .Entity<Role>()
            .HasData(
                adminRole,
                new Role { Id = RoleEnum.Technician, Name = "Technician" },
                new Role { Id = RoleEnum.BrandManager, Name = "Brand manager" },
                new Role { Id = RoleEnum.ShopManager, Name = "Shop manager" },
                new Role { Id = RoleEnum.Employee, Name = "Employee" }
            );

        // AccountRole junction table
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
            .HasData(
                new EdgeBoxStatus { Id = EdgeBoxStatusEnum.Active, Name = "Active" },
                new EdgeBoxStatus { Id = EdgeBoxStatusEnum.Inactive, Name = "Inactive" }
            );

        modelBuilder
            .Entity<EdgeBoxInstallStatus>()
            .HasData(
                new EdgeBoxInstallStatus { Id = EdgeBoxInstallStatusEnum.Valid, Name = "Valid" },
                new EdgeBoxInstallStatus { Id = EdgeBoxInstallStatusEnum.Expired, Name = "Expired" }
            );

        modelBuilder
            .Entity<RequestStatus>()
            .HasData(
                new RequestStatus { Id = RequestStatusEnum.Open, Name = "Open" },
                new RequestStatus { Id = RequestStatusEnum.Canceled, Name = "Canceled" },
                new RequestStatus { Id = RequestStatusEnum.Done, Name = "Done" },
                new RequestStatus { Id = RequestStatusEnum.Rejected, Name = "Rejected" }
            );

        modelBuilder
            .Entity<RequestType>()
            .HasData(
                new RequestType { Id = RequestTypeEnum.Install, Name = "Install" },
                new RequestType { Id = RequestTypeEnum.Repair, Name = "Repair" },
                new RequestType { Id = RequestTypeEnum.Remove, Name = "Remove" }
            );

        modelBuilder
            .Entity<TicketStatus>()
            .HasData(
                new TicketStatus { Id = TicketStatusEnum.Open, Name = "Open" },
                new TicketStatus { Id = TicketStatusEnum.Canceled, Name = "Canceled" },
                new TicketStatus { Id = TicketStatusEnum.Done, Name = "Done" },
                new TicketStatus { Id = TicketStatusEnum.Failed, Name = "Failed" }
            );

        modelBuilder
            .Entity<TicketType>()
            .HasData(
                new TicketType { Id = TicketTypeEnum.Install, Name = "Install" },
                new TicketType { Id = TicketTypeEnum.Repair, Name = "Repair" },
                new TicketType { Id = TicketTypeEnum.Remove, Name = "Remove" }
            );

        modelBuilder.Entity<Brand>(builder =>
        {
            builder.Property(x => x.LogoUri).HasConversion<string>();
            builder.Property(x => x.BannerUri).HasConversion<string>();
        });

        modelBuilder
            .Entity<Brand>()
            .HasOne(x => x.BrandManager)
            .WithOne(x => x.Brand)
            .HasForeignKey<Brand>(x => x.BrandManagerId);

        modelBuilder
            .Entity<Shop>()
            .HasOne(x => x.ShopManager)
            .WithOne(x => x.ManagingShop)
            .HasForeignKey<Shop>(x => x.ShopManagerId);

        modelBuilder.Entity<Evidence>().Property(p => p.Uri).HasConversion<string>();
    }
}
