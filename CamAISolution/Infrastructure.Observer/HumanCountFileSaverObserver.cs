using System.Text.Json;
using Core.Domain.Interfaces.Events;
using Core.Domain.Models.Configurations;
using Core.Domain.Models.Consumers;
using Core.Domain.Utilities;

namespace Infrastructure.Observer;

public class HumanCountFileSaverObserver(AiConfiguration configuration) : IHumanCountObserver
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = false };

    private readonly string baseOutputDir = configuration.OutputDirectory;
    public Guid ShopId => Guid.Empty;

    public void ReceiveData(HumanCountModel model)
    {
        var date = DateOnly.FromDateTime(model.Time);
        // shopId -> date -> time
        var shopDir = Path.Combine(baseOutputDir, model.ShopId.ToString("N"), date.ToDirPath());
        FileHelper.EnsureDirectoryExisted(shopDir);
        var text = JsonSerializer.Serialize(model, Options);
        // TODO: lock file
        File.AppendAllLines(Path.Combine(shopDir, date.Day.ToString()), [text]);
    }
}
