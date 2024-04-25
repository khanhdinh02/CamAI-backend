using Core.Domain.Constants;
using Core.Domain.DTO;
using CsvHelper.Configuration;

namespace Infrastructure.Files.Configurations;

public class EmployeeFromImportFileMap : ClassMap<EmployeeFromImportFileMap>
{
    public EmployeeFromImportFileMap()
    {
        foreach (var propertyInfo in typeof(EmployeeFromImportFile).GetProperties())
        {
            var name = propertyInfo.Name.Replace("get_", "");
            Map(typeof(EmployeeFromImportFile), propertyInfo, false).Name(name, name.PascalCaseToSeparateWords(), name.ToLower(), name.ToUpper());
        }
    }
}