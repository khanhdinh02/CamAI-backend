using System.Globalization;
using System.Reflection;
using Core.Application.Exceptions;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;
using CsvHelper;
using CsvHelper.Configuration;

namespace Infrastructure.Files;

public class ReadFileService(ICacheService cacheService) : IReadFileService
{
    private static readonly IEnumerable<Type> ClassMaps = Assembly.GetAssembly(typeof(ReadFileService))!.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ClassMap)));
    public IEnumerable<T> ReadFromCsv<T>(Stream stream, bool isCacheCountRecords = false, string cachedKey = "")
    {
        using var reader = new StreamReader(stream);
        using var helper = new CsvReader(reader, CultureInfo.GetCultureInfo("vi-VN"));
        //TODO[Dat]: use factory pattern
        foreach (var classMap in ClassMaps)
            helper.Context.RegisterClassMap(classMap);
        var records = helper.GetRecords<T>().ToList();
        if (isCacheCountRecords)
        {
            if (string.IsNullOrEmpty(cachedKey))
                throw new ServiceUnavailableException("cached key for count records is not set");
            cacheService.Set(cachedKey, records.Count(), TimeSpan.FromMinutes(1));
        }

        foreach (var record in records)
            yield return record;

        if (isCacheCountRecords && !string.IsNullOrEmpty(cachedKey))
            cacheService.Remove(cachedKey);
    }

    public T ReadFromJson<T>(Stream stream)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(stream) ?? throw new BadRequestException("Cannot parse json file");
    }
}
