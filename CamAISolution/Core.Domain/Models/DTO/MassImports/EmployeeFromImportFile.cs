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
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? AddressLine { get; set; }

    public bool IsValid() => !EmployeeFromImportFileValidation().Any();

    public IDictionary<string, object?> EmployeeFromImportFileValidation()
    {
        var result = new Dictionary<string, object?>();

        if (string.IsNullOrEmpty(Name))
            result.Add($"{nameof(Name)}", "Cannot be empty");
        else if (Name.Length > 50)
            result.Add($"{nameof(Name)}", "Length must be less than or equal to 50");
        if (!string.IsNullOrEmpty(Email) && !MailAddress.TryCreate(Email, out _))
            result.Add($"{nameof(Email)}", $"{Email} is wrong format, e.g: example@gmail.com");
        if (!string.IsNullOrEmpty(Phone) && !RegexHelper.VietNamPhoneNumber.IsMatch(Phone))
            result.Add($"{nameof(Phone)}", $"{Phone} is wrong, phone must start with +84|84|0 and then 3|5|7|8|9 and then 8 digits from 0-9. E.g: 0982335536");
        if (Birthday.HasValue && Birthday.Value.Year > (DateTimeHelper.VNDateTime.Year - 18))
            result.Add($"{Birthday}", "Employee is not 18 years old");
        return result;
    }
}
