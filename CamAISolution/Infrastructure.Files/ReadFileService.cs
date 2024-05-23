using System.Globalization;
using System.Reflection;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;
using CsvHelper;
using CsvHelper.Configuration;

namespace Infrastructure.Files;

public class ReadFileService(ICacheService cacheService, IAppLogging<ReadFileService> logger) : IReadFileService
{
    private static readonly IEnumerable<Type> ClassMaps = Assembly.GetAssembly(typeof(ReadFileService))!.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ClassMap)));
    public IEnumerable<T> ReadFromCsv<T>(Stream stream, string cachedFaledRecordsKey, bool isCacheCountRecords = false, string cachedProgressKey = "")
    {
        var unhandleFailedRecords = new List<string>();
        bool isBadRecord = false;
        using var reader = new StreamReader(stream);
        var csvReaderConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("vi-VN"))
        {
            ReadingExceptionOccurred = re =>
            {
                logger.Error(re.Exception.Message, re.Exception);
                return false;
            },
            BadDataFound = context =>
            {
                isBadRecord = true;
                logger.Warn($"{context.RawRecord}");
                unhandleFailedRecords.Add(context.RawRecord);
            }
        };
        var rowCount = reader.ReadToEnd().Split('\n').Count() - 1; // exclude header
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        using var helper = new CsvReader(reader, csvReaderConfig);
        //TODO[Dat]: use factory pattern
        foreach (var classMap in ClassMaps)
            helper.Context.RegisterClassMap(classMap);
        if (isCacheCountRecords)
        {
            if (string.IsNullOrEmpty(cachedProgressKey))
                throw new ServiceUnavailableException("cached key for count records is not set");
            cacheService.Set(cachedProgressKey, rowCount > 0 ? rowCount : 1, TimeSpan.FromMinutes(1));
        }
        while (helper.Read())
        {
            if (!isBadRecord)
            {
                yield return helper.GetRecord<T>();
            }
            isBadRecord = false;
        }

        cacheService.Set(cachedFaledRecordsKey, unhandleFailedRecords, TimeSpan.FromMinutes(1));

        if (isCacheCountRecords && !string.IsNullOrEmpty(cachedProgressKey))
            cacheService.Remove(cachedProgressKey);
    }

    public T ReadFromJson<T>(Stream stream)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(stream) ?? throw new BadRequestException("Cannot parse json file");
    }
}
