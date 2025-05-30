using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;

public class GoogleFirebaseSignIn : MonoBehaviour
{
    private bool isSigningIn = false;
    private FirebaseAuth auth;

    void Start()
    {
        StartCoroutine(InitializeFirebaseAndSignIn());
    }

    private IEnumerator InitializeFirebaseAndSignIn()
    {
        var checkTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => checkTask.IsCompleted);

        if (checkTask.Result == DependencyStatus.Available)
        {
            Debug.Log("✅ Firebase initialized.");
            auth = FirebaseAuth.DefaultInstance;
            SignInWithGoogle();
        }
        else
        {
            Debug.LogError("❌ Firebase dependencies not resolved: " + checkTask.Result);
        }
    }

    private void SignInWithGoogle()
    {
        if (isSigningIn) return;
        isSigningIn = true;

        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            WebClientId = "239714143159-kj1p05pg5pm01gpq60q5m97lqdhtb6aa.apps.googleusercontent.com"
        };

        Debug.Log("🔁 Starting Google Sign-In...");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleSignInFinished);
    }

    private void OnGoogleSignInFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsCanceled || task.IsFaulted)
        {
            Debug.LogError("❌ Google Sign-In failed: " + task.Exception);
            isSigningIn = false;
            return;
        }

        Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

        Debug.Log("🔁 Exchanging credentials with Firebase...");
        auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
        {
            isSigningIn = false;

            if (authTask.IsCanceled || authTask.IsFaulted)
            {
                Debug.LogError("❌ Firebase Sign-In failed: " + authTask.Exception);
                return;
            }

            FirebaseUser user = authTask.Result;
            Debug.Log($"✅ Firebase Sign-In successful! Welcome {user.DisplayName} ({user.Email})");
        });
    }
}
