using System.Collections.Concurrent;
using System.Text.Json;
using Core.Domain.Interfaces.Events;
using Core.Domain.Models.Configurations;
using Core.Domain.Models.Consumers;
using Core.Domain.Utilities;

namespace Infrastructure.Observer;

public class HumanCountFileSaverObserver(AiConfiguration configuration) : IHumanCountObserver
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = false };
    private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> Locks = new();

    private readonly string baseOutputDir = configuration.OutputDirectory;
    public Guid ShopId => Guid.Empty;

    public async Task ReceiveData(HumanCountModel model)
    {
        var date = DateOnly.FromDateTime(model.Time);
        // shopId -> date -> time
        var shopDir = Path.Combine(baseOutputDir, model.ShopId.ToString("N"), date.ToDirPath());
        FileHelper.EnsureDirectoryExisted(shopDir);
        var text = JsonSerializer.Serialize(model, Options);
        var @lock = Locks.GetOrAdd(model.ShopId, _ => new SemaphoreSlim(1, 1));
        await @lock.WaitAsync();
        await File.AppendAllLinesAsync(Path.Combine(shopDir, date.Day.ToString()), [text]);
        @lock.Release();
    }
}
