using Core.Domain.DTO;
using Core.Domain.DTO.Requests;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Data;

public class CamAIContext : DbContext
{
    public CamAIContext() { }

    public CamAIContext(DbContextOptions<CamAIContext> options)
        : base(options) { }

    public virtual DbSet<Account> Accounts { get; set; } = null!;
    public virtual DbSet<Employee> Employees { get; set; } = null!;
    public virtual DbSet<EmployeeStatus> EmployeeStatuses { get; set; } = null!;
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
    public virtual DbSet<Behavior> Behaviors { get; set; } = null!;
    public virtual DbSet<BehaviorType> BehaviorTypes { get; set; } = null!;
    public virtual DbSet<Evidence> Evidences { get; set; } = null!;
    public virtual DbSet<EvidenceType> EvidenceTypes { get; set; } = null!;
    public virtual DbSet<Notification> Notifications { get; set; } = null!;
    public virtual DbSet<NotificationStatus> NotificationStatuses { get; set; } = null!;
    public virtual DbSet<AccountNotification> AccountNotifications { get; set; } = null!;
    public virtual DbSet<NotificationType> NotificationTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var adminRole = new Role { Id = RoleEnum.Admin, Name = "Admin" };
        var accountStatusActive = new AccountStatus { Id = AccountStatusEnum.Active, Name = "Active" };
        // AccountStatus=New when account is created and its password have not been changed.
        modelBuilder
            .Entity<AccountStatus>()
            .HasData(
                new AccountStatus { Id = AccountStatusEnum.New, Name = "New" },
                accountStatusActive,
                new AccountStatus { Id = AccountStatusEnum.Inactive, Name = "Inactive" }
            );
        modelBuilder
            .Entity<EmployeeStatus>()
            .HasData(
                new EmployeeStatus { Id = EmployeeStatusEnum.Active, Name = "Active" },
                new EmployeeStatus { Id = EmployeeStatusEnum.Inactive, Name = "Inactive" }
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
                new ShopStatus { Id = ShopStatusEnum.Inactive, Name = "Inactive" }
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
            builder
                .HasMany(e => e.Roles)
                .WithMany(e => e.Accounts)
                .UsingEntity(
                    r => r.HasOne(typeof(Role)).WithMany().HasForeignKey(roleId),
                    l => l.HasOne(typeof(Account)).WithMany().HasForeignKey(accountId),
                    je => je.HasKey(roleId, accountId)
                );
        });

        modelBuilder
            .Entity<EdgeBoxStatus>()
            .HasData(
                new EdgeBoxStatus { Id = EdgeBoxStatusEnum.Active, Name = "Active" },
                new EdgeBoxStatus { Id = EdgeBoxStatusEnum.Inactive, Name = "Inactive" },
                new EdgeBoxStatus { Id = EdgeBoxStatusEnum.Broken, Name = "Broken" }
            );

        modelBuilder
            .Entity<EdgeBoxLocation>()
            .HasData(
                new EdgeBoxLocation { Id = EdgeBoxLocationEnum.Idle, Name = "Idle" },
                new EdgeBoxLocation { Id = EdgeBoxLocationEnum.Installing, Name = "Installing" },
                new EdgeBoxLocation { Id = EdgeBoxLocationEnum.Occupied, Name = "Occupied" },
                new EdgeBoxLocation { Id = EdgeBoxLocationEnum.Uninstalling, Name = "Uninstalling" },
                new EdgeBoxLocation { Id = EdgeBoxLocationEnum.Disposed, Name = "Disposed" }
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

        modelBuilder.Entity<Employee>(builder =>
        {
            builder.Property(e => e.Image).HasConversion<string>();
            builder.Property(e => e.Gender).HasConversion<string>();
        });

        modelBuilder.Entity<Image>().Property(i => i.HostingUri).HasConversion<string>();

        modelBuilder.Entity<Evidence>().Property(p => p.Uri).HasConversion<string>();

        modelBuilder
            .Entity<NotificationStatus>()
            .HasData(
                new NotificationStatus
                {
                    Id = NotificationStatusEnum.Unread,
                    Name = nameof(NotificationStatusEnum.Unread)
                },
                new NotificationStatus { Id = NotificationStatusEnum.Read, Name = nameof(NotificationStatusEnum.Read) }
            );
        modelBuilder.Entity<AccountNotification>().HasKey(an => new { an.AccountId, an.NotificationId });
        modelBuilder
            .Entity<Notification>()
            .HasOne(n => n.SentBy)
            .WithMany(a => a.SentNotifications)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<NotificationType>()
            .HasData(
                new NotificationType { Id = NotificationTypeEnum.Normal, Name = nameof(NotificationTypeEnum.Normal) },
                new NotificationType
                {
                    Id = NotificationTypeEnum.Warnning,
                    Name = nameof(NotificationTypeEnum.Warnning)
                },
                new NotificationType { Id = NotificationTypeEnum.Urgent, Name = nameof(NotificationTypeEnum.Urgent) }
            );
    }
}
