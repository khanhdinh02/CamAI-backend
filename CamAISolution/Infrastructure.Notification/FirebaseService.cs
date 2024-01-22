using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Infrastructure.Notification;

public class FirebaseService
{
    private readonly FirebaseMessaging messaging;
    public FirebaseMessaging Messaging => messaging;

    public FirebaseService()
    {
        var app = FirebaseApp.Create(
            new AppOptions
            {
                Credential = GoogleCredential
                    .FromFile($@"{Directory.GetCurrentDirectory()}/googlesercet.json")
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
            }
        );
        messaging = FirebaseMessaging.GetMessaging(app);
    }
}
