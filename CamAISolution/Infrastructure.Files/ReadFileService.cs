using System.Globalization;
using System.Reflection;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Interfaces.Services;
using CsvHelper;
using CsvHelper.Configuration;

namespace Infrastructure.Files;

public class ReadFileService(IAppLogging<ReadFileService> logger) : IReadFileService
{
    private static readonly IEnumerable<Type> ClassMaps = Assembly.GetAssembly(typeof(ReadFileService))!.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ClassMap)));
    public IEnumerable<T> ReadFromCsv<T>(Stream stream)
    {
        using var reader = new StreamReader(stream);
        using var helper = new CsvReader(reader, CultureInfo.GetCultureInfo("vi-VN"));
        //TODO[Dat]: use factory pattern
        foreach (var classMap in ClassMaps)
            helper.Context.RegisterClassMap(classMap);

        foreach (var record in helper.GetRecords<T>())
            yield return record;
    }

    public T ReadFromJson<T>(Stream stream)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(stream) ?? throw new BadRequestException("Cannot parse json file");
    }
}
