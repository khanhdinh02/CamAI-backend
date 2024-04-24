using System.Globalization;
using System.Reflection;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using CsvHelper;
using CsvHelper.Configuration;
using Ganss.Excel;

namespace Infrastructure.Files;

public class ReadFileService(IAppLogging<ReadFileService> logger) : IReadFileService
{
    private static readonly IEnumerable<Type> classMaps = Assembly.GetAssembly(typeof(ReadFileService))!.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ClassMap)));
    public IEnumerable<T> ReadFile<T>(Stream stream, FileType type)
    {
        try
        {
            return type switch
            {

                FileType.Xlsx => ReadFromXlsx<T>(stream),
                FileType.Csv => ReadFromCsv<T>(stream),
                _ => throw new NotSupportedException()
            };
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
        }

        throw new ServiceUnavailableException("Something wrong");
    }

    private IEnumerable<T> ReadFromCsv<T>(Stream stream)
    {
        using var reader = new StreamReader(stream);
        using var helper = new CsvReader(reader, CultureInfo.InvariantCulture);
        //TODO[Dat]: use factory pattern
        foreach (var classMap in classMaps)
            helper.Context.RegisterClassMap(classMap);

        foreach (var record in helper.GetRecords<T>())
            yield return record;
    }

    public T ReadFromJson<T>(Stream stream)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(stream) ?? throw new BadRequestException("Cannot parse json file");
    }

    private IEnumerable<T> ReadFromXlsx<T>(Stream stream)
    {
        foreach (var record in new ExcelMapper(stream).Fetch<T>())
            yield return record;
    }
}
