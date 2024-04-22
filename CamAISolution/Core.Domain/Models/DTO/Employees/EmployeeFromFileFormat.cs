using System.ComponentModel.DataAnnotations;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class EmployeeFromFileFormat
{
    /// <summary>
    /// Identity of employee from original company, organiztion, etc...
    /// </summary>
    public string? OrganizationId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;
    public Gender Gender { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? AddressLine { get; set; }
    public string IsSupervisor { get; set; }
}