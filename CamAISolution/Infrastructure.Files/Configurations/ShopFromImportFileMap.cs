using Core.Domain.Constants;
using Core.Domain.DTO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Infrastructure.Files.Configurations;

public sealed class ShopFromImportFileMap : ClassMap<ShopFromImportFile>
{
    public ShopFromImportFileMap()
    {
        foreach (var propertyInfo in typeof(ShopFromImportFile).GetProperties())
        {
            var name = propertyInfo.Name.Replace("get_", "");
            Map(typeof(ShopFromImportFile), propertyInfo, false).Name(name, name.PascalCaseToSeparateWords(), name.ToLower(), name.ToUpper());
        }
    }
}