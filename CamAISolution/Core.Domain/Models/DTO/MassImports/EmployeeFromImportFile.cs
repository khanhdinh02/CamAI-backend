using System.Net.Mail;
using Core.Domain.Constants;
using Core.Domain.Enums;
using Core.Domain.Utilities;

namespace Core.Domain.DTO;

public class EmployeeFromImportFile
{
    public string? ExternalId { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public Gender Gender { get; set; }
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? AddressLine { get; set; }

    public bool IsValid() => !EmployeeFromImportFileValidation().Any();

    public IDictionary<string, object?> EmployeeFromImportFileValidation()
    {
        var result = new Dictionary<string, object?>();
        if(Name.Length > 50)
            result.Add($"{nameof(Name)}", "Employee name's length must be less than or equal to 50");
        if(!string.IsNullOrEmpty(Email) && !MailAddress.TryCreate(Email, out _))
            result.Add($"{nameof(Email)}", $"{Email} is wrong format");
        if(!string.IsNullOrEmpty(Phone) && !RegexHelper.VietNamPhoneNumber.IsMatch(Phone))
            result.Add($"{nameof(Phone)}", $"{Phone} is wrong");
        if(Birthday.HasValue && Birthday.Value.Year > (DateTimeHelper.VNDateTime.Year - 18))
            result.Add($"{Birthday}", "Employee is not 18 years old");
        return result;
    }
}