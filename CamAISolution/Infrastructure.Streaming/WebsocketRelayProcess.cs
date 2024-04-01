using System.Diagnostics;
using System.Text;
using Core.Domain.Utilities;

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

        process = new Process();

        process.StartInfo.FileName = Configuration.Filename;
        var strBuilder = new StringBuilder(Configuration.Arguments);
        strBuilder.Replace("{Secret}", Secret);
        strBuilder.Replace("{WebsocketSecret}", Secret);
        strBuilder.Replace("{HttpPort}", HttpPort.ToString());
        strBuilder.Replace("{WebsocketPort}", WebsocketPort.ToString());
        process.StartInfo.Arguments = strBuilder.ToString();

        process.StartInfo.RedirectStandardOutput = true;
        process.OutputDataReceived += CheckOutputForConnection;

        timer = new System.Timers.Timer(TimeSpan.FromSeconds(30));
        timer.Elapsed += (_, _) => WebsocketRelayProcessManager.Kill(name);
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
            timer.Stop();
        else if (output.StartsWith("Disconnected WebSocket"))
        {
            var numOfConnection = Int32.Parse(output.Split(",")[1]);
            if (numOfConnection == 0)
                timer.Start();
        }
    }

    public void Stop()
    {
        process.Kill();
        timer.Close();
    }
}
