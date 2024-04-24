using Core.Domain.Constants;
using CsvHelper.Configuration;

namespace Infrastructure.Files.Configurations;

public class EmployeeFromImportFile : ClassMap<EmployeeFromImportFile>
{
    public EmployeeFromImportFile()
    {
        foreach (var propertyInfo in typeof(EmployeeFromImportFile).GetProperties())
        {
            var name = propertyInfo.Name.Replace("get_", "");
            Map(typeof(EmployeeFromImportFile), propertyInfo, false).Name(name, name.PascalCaseToSeparateWords(), name.ToLower(), name.ToUpper());
        }
    }
}