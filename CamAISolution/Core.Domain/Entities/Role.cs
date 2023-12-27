using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Role : LookupEntity
{
    public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
}
