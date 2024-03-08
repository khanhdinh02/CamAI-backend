using System.ComponentModel.DataAnnotations;
using Core.Application.Exceptions;
using Core.Domain.Models.Configurations;

namespace Host.CamAI.API.Attributes;
/// <summary>
///
/// </summary>
/// <param name="size">Integer number dedicate file's size</param>
/// <param name="sizeUnit">Unit of size</param>
[AttributeUsage(AttributeTargets.Property)]
public class FileSizeLimitAttribute(int maxSize, SizeUnit sizeUnit = SizeUnit.KB) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var maxSizeInByteUnit = maxSize * GetUnitMultiplierToByte();
        var file = value as IFormFile;
        if (file!.Length <= maxSizeInByteUnit)
            return ValidationResult.Success;
        throw new BadRequestException(GetErrorMessage());
    }
    private string GetErrorMessage() => $"Image file's size cannot greater than {maxSize}{sizeUnit}";

    private long GetUnitMultiplierToByte()
    {
        switch (sizeUnit)
        {
            case SizeUnit.MB : return (long)Math.Pow(1024, 2);
            case SizeUnit.GB : return (long)Math.Pow(1024, 3);
            default: return 1024; //KB size
        }
    }
}

public enum SizeUnit
{
    KB,
    MB,
    GB
}