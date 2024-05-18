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
    public virtual DbSet<Brand> Brands { get; set; } = null!;
    public virtual DbSet<Shop> Shops { get; set; } = null!;
    public virtual DbSet<Province> Provinces { get; set; } = null!;
    public virtual DbSet<District> Districts { get; set; } = null!;
    public virtual DbSet<Ward> Wards { get; set; } = null!;
    public virtual DbSet<EdgeBox> EdgeBoxes { get; set; } = null!;
    public virtual DbSet<EdgeBoxModel> EdgeBoxModels { get; set; } = null!;
    public virtual DbSet<EdgeBoxInstall> EdgeBoxInstalls { get; set; } = null!;
    public virtual DbSet<EdgeBoxActivity> EdgeBoxActivities { get; set; } = null!;
    public virtual DbSet<Camera> Cameras { get; set; } = null!;
    public virtual DbSet<Incident> Incidents { get; set; } = null!;
    public virtual DbSet<Evidence> Evidences { get; set; } = null!;
    public virtual DbSet<Notification> Notifications { get; set; } = null!;
    public virtual DbSet<AccountNotification> AccountNotifications { get; set; } = null!;
    public virtual DbSet<Image> Images { get; set; } = null!;
    public virtual DbSet<SupervisorAssignment> SupervisorAssignments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Image>().Property(i => i.HostingUri).HasConversion<string>();

        modelBuilder.Entity<Incident>().HasOne(x => x.Shop).WithMany().OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AccountNotification>().HasKey(an => new { an.AccountId, an.NotificationId });

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
