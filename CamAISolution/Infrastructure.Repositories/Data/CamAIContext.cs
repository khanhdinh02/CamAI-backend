using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Data;

public class CamAIContext : DbContext
{
    public CamAIContext() { }

    public CamAIContext(DbContextOptions<CamAIContext> options)
        : base(options) { }

    public virtual DbSet<Account> Accounts { get; set; } = null!;
    public virtual DbSet<AccountRole> AccountRoles { get; set; } = null!;
    public virtual DbSet<Employee> Employees { get; set; } = null!;
    public virtual DbSet<Brand> Brands { get; set; } = null!;
    public virtual DbSet<Shop> Shops { get; set; } = null!;
    public virtual DbSet<Province> Provinces { get; set; } = null!;
    public virtual DbSet<District> Districts { get; set; } = null!;
    public virtual DbSet<Ward> Wards { get; set; } = null!;
    public virtual DbSet<EdgeBox> EdgeBoxes { get; set; } = null!;
    public virtual DbSet<EdgeBoxInstall> EdgeBoxInstalls { get; set; } = null!;
    public virtual DbSet<EdgeBoxActivity> EdgeBoxActivities { get; set; } = null!;
    public virtual DbSet<Camera> Cameras { get; set; } = null!;
    public virtual DbSet<Request> Requests { get; set; } = null!;
    public virtual DbSet<RequestActivity> RequestActivities { get; set; } = null!;
    public virtual DbSet<Incident> Incidents { get; set; } = null!;
    public virtual DbSet<Evidence> Evidences { get; set; } = null!;
    public virtual DbSet<Notification> Notifications { get; set; } = null!;
    public virtual DbSet<AccountNotification> AccountNotifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountRole>().HasKey(ar => new { ar.AccountId, ar.Role });

        modelBuilder.Entity<Employee>(builder =>
        {
            builder.Property(e => e.Image).HasConversion<string>();

            var employeeId = "EmployeeId";
            var incidentId = "IncidentId";
            builder
                .HasMany(e => e.Incidents)
                .WithMany(i => i.Employees)
                .UsingEntity(
                    r => r.HasOne(typeof(Incident)).WithMany().HasForeignKey(incidentId),
                    l => l.HasOne(typeof(Employee)).WithMany().HasForeignKey(employeeId)
                );
        });

        modelBuilder.Entity<Image>().Property(i => i.HostingUri).HasConversion<string>();

        modelBuilder.Entity<Evidence>().Property(p => p.Uri).HasConversion<string>();

        modelBuilder.Entity<AccountNotification>().HasKey(an => new { an.AccountId, an.NotificationId });
        modelBuilder
            .Entity<Notification>()
            .HasOne(n => n.SentBy)
            .WithMany(a => a.SentNotifications)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
