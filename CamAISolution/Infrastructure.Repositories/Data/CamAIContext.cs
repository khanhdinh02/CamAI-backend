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
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
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
    public virtual DbSet<AccountRole> AccountRoles { get; set; }
    public virtual DbSet<Gender> Genders { get; set; }
    public virtual DbSet<Province> Provinces { get; set; }
    public virtual DbSet<District> Districts { get; set; }
    public virtual DbSet<Ward> Wards { get; set; }
    public virtual DbSet<ShopStatus> ShopStatuses { get; set; }
    public virtual DbSet<BrandStatus> BrandStatuses { get; set; }
    public virtual DbSet<AccountStatus> AccountStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountRole>().HasKey(e => new { e.AccountId, e.RoleId });

        var adminRole = new Role { Name = "Admin" };
        var accountStatusActive = new AccountStatus { Name = "Active" };
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
            .HasData(new AccountStatus { Name = "New" }, accountStatusActive, new AccountStatus { Name = "Inactive" });
        modelBuilder
            .Entity<BrandStatus>()
            .HasData(new BrandStatus { Name = "Active" }, new BrandStatus { Name = "Inactive" });
        modelBuilder
            .Entity<ShopStatus>()
            .HasData(new ShopStatus { Name = "Active" }, new ShopStatus { Name = "Inactive" });

        modelBuilder
            .Entity<Role>()
            .HasData(
                adminRole,
                new Role { Name = "Technician" },
                new Role { Name = "Brand manager" },
                new Role { Name = "Shop manager" },
                new Role { Name = "Employee" }
            );

        modelBuilder.Entity<Account>().HasData(adminAccount);

        modelBuilder
            .Entity<AccountRole>()
            .HasData(new AccountRole { AccountId = adminAccount.Id, RoleId = adminRole.Id });

        modelBuilder.Entity<Gender>().HasData(new Gender { Name = "Male" }, new Gender { Name = "Female" });
    }
}
