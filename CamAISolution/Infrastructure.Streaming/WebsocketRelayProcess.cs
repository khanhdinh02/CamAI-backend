using System.Diagnostics;
using System.Text;
using Core.Domain.Utilities;
using Serilog;

namespace Infrastructure.Streaming;

public class WebsocketRelayProcess
{
    public static StreamingConfiguration Configuration;
    public string Name { get; set; }
    public int HttpPort { get; set; }
    public int WebsocketPort { get; set; }
    public string Secret { get; set; } = RandomGenerator.GetAlphanumericString(20);
    private readonly Process process;
    private readonly System.Timers.Timer timer;

    public WebsocketRelayProcess(string name)
    {
        Name = name;

        var ports = NetworkUtil.GetOpenPort(2);
        HttpPort = ports[0];
        WebsocketPort = ports[1];
        Log.Information(
            "Found two open port: HttpPort {HttpPort}, WebSocketPort {WebSocketPort}",
            HttpPort,
            WebsocketPort
        );

        process = new Process();

        process.StartInfo.FileName = Configuration.Filename;
        var strBuilder = new StringBuilder(Configuration.Arguments);
        strBuilder.Replace("{Secret}", Secret);
        strBuilder.Replace("{WebsocketSecret}", Secret);
        strBuilder.Replace("{HttpPort}", HttpPort.ToString());
        strBuilder.Replace("{WebsocketPort}", WebsocketPort.ToString());
        process.StartInfo.Arguments = strBuilder.ToString();
        Log.Information(
            "Running process: {FileName} {FileArguments}",
            process.StartInfo.FileName,
            process.StartInfo.Arguments
        );

        process.StartInfo.RedirectStandardOutput = true;
        process.OutputDataReceived += CheckOutputForConnection;

        timer = new System.Timers.Timer(TimeSpan.FromSeconds(Configuration.Interval));
        timer.Elapsed += (_, _) =>
        {
            Log.Information("Timer elapsed, kill process {ProcessName}", name);
            WebsocketRelayProcessManager.Kill(name);
        };
        timer.AutoReset = false;
    }

    public void Run()
    {
        process.Start();
        process.BeginOutputReadLine();
        timer.Start();
    }

    private void CheckOutputForConnection(object sender, DataReceivedEventArgs e)
    {
        var output = e.Data;
        if (output == null)
            return;

        if (output.StartsWith("New WebSocket Connection"))
        {
            Log.Information("New client connect to websocket relay process {ProcessName}", Name);
            timer.Stop();
        }
        else if (output.StartsWith("Disconnected WebSocket"))
        {
            var numOfConnection = Int32.Parse(output.Split(",")[1]);
            Log.Information(
                "One client disconnected from websocket relay process {ProcessName}, {NumOfClient} left",
                Name,
                numOfConnection
            );
            if (numOfConnection == 0)
            {
                Log.Information("0 remaining connection, start timer");
                timer.Start();
            }
        }
    }

    public void Stop()
    {
        process.Kill();
        timer.Close();
    }
}
