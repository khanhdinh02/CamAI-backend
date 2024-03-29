namespace Host.CamAI.API.Utils;

public static class HttpUtilities
{
    public static string UserIp(HttpContext context) => context.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

    public static bool IsFromMobile(HttpRequest request) => request.Headers.UserAgent.Contains("Mobile");
}
