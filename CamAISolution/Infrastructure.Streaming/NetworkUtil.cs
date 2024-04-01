using System.Net.NetworkInformation;

namespace Infrastructure.Streaming;

public static class NetworkUtil
{
    public static List<int> GetOpenPort(int numOfPort = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(numOfPort, 1);

        const int portStartIndex = 8000;
        const int portEndIndex = 9000;
        var properties = IPGlobalProperties.GetIPGlobalProperties();
        var tcpEndPoints = properties.GetActiveTcpListeners();

        var usedPorts = tcpEndPoints.Select(p => p.Port).ToList();
        return Enumerable
            .Range(portStartIndex, portEndIndex)
            .Where(x => !usedPorts.Contains(x))
            .Take(numOfPort)
            .ToList();
    }
}
