using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Notification.Models;

namespace Infrastructure.Notification;

public class FirebaseService
{
    private readonly FirebaseMessaging messaging;
    public FirebaseMessaging Messaging => messaging;

    public FirebaseService(GoogleSecret secret)
    {
        var app = FirebaseApp.Create(
            new AppOptions
            {
                Credential = GoogleCredential
                    .FromJson(System.Text.Json.JsonSerializer.Serialize(secret))
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
            }
        );
        messaging = FirebaseMessaging.GetMessaging(app);
    }
}
