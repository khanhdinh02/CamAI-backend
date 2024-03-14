using System.Text.Json.Serialization;

namespace Infrastructure.Notification.Models;

public class GoogleSecret
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; } = null!;

    [JsonPropertyName("private_key_id")]
    public string PrivateKeyId { get; set; } = null!;

    [JsonPropertyName("private_key")]
    public string PrivateKey { get; set; } = null!;

    [JsonPropertyName("client_email")]
    public string ClientEmail { get; set; } = null!;

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = null!;

    [JsonPropertyName("auth_uri")]
    public string AuthUri { get; set; } = null!;

    [JsonPropertyName("token_uri")]
    public string TokenUri { get; set; } = null!;

    [JsonPropertyName("auth_provider_x509_cert_url")]
    public string AuthProviderX509CertUrl { get; set; } = null!;

    [JsonPropertyName("client_x509_cert_url")]
    public string ClientX509CertUrl { get; set; } = null!;

    [JsonPropertyName("universe_domain")]
    public string UniverseDomain { get; set; } = null!;
}
