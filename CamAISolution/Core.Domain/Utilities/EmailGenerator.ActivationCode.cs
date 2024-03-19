using System.Net;

namespace Core.Domain.Utilities;

public static partial class EmailGenerator
{
    public static string GenerateActivationCodeEmail(string name, string activationCode)
    {
        return $"""
                    <p>Dear {WebUtility.HtmlEncode(name)},</p>
                    <p>Thank you for registering. Your activation code is:</p>
                    <p>{activationCode[..4]} {activationCode.Substring(4, 4)} {activationCode.Substring(8, 4)} {activationCode.Substring(12, 4)}</p>
                    <p>Best regards,</p>
                    <p>CameraAi</p>
                """;
    }
}
