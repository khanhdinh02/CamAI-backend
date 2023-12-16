using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Data;
public class CamAIContext(DbContextOptions<CamAIContext> options) : DbContext(options)
{
    public virtual DbSet<Account> Accounts { get; set; }
}
