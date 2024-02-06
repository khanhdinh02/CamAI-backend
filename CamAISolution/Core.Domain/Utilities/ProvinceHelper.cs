using System.Text;
using Core.Domain.Entities;

namespace Core.Domain.Utilities;

public static class ProvinceHelper
{
    public static string GetFullAddress(string? addressLine, Ward ward)
    {
        var sb = new StringBuilder(addressLine);
        sb.Append(ward.Name);
        sb.Append(ward.District.Name);
        sb.Append(ward.District.Province.Name);
        return sb.ToString();
    }
}
