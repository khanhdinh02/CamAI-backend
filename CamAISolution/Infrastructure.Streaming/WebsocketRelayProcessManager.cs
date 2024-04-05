using Serilog;

namespace Infrastructure.Streaming;

public static class WebsocketRelayProcessManager
{
    private static readonly List<WebsocketRelayProcess> Processes = [];

    public static RelayInformation Run(string processName)
    {
        Log.Information("Receive websocket relay run request for process {ProcessName}", processName);
        var process = Processes.Find(x => x.Name == processName);
        if (process != null)
        {
            Log.Information(
                "Found running websocket relay, {HttpPort}, {WebSocketPort}",
                process.HttpPort,
                process.WebsocketPort
            );
            return RelayInformation.FromProcess(process);
        }
        process = new WebsocketRelayProcess(processName);
        process.Run();
        Processes.Add(process);
        return RelayInformation.FromProcess(process);
    }

    public static void Kill(string processName)
    {
        Log.Information("Receive websocket relay kill request for process {ProcessName}", processName);
        var process = Processes.Find(x => x.Name == processName);
        if (process == null)
        {
            Log.Information("Websocket relay not found");
            return;
        }
        Log.Information("Killing websocket relay");
        process.Stop();
        Processes.Remove(process);
    }
}

public record RelayInformation(int HttpPort, int WebsocketPort, string Secret)
{
    public static RelayInformation FromProcess(WebsocketRelayProcess process)
    {
        return new RelayInformation(process.HttpPort, process.WebsocketPort, process.Secret);
    }
}
