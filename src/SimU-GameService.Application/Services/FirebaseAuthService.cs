using Internal;
using System;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using SimU_GameService.Domain;

/// <summary>
/// Used Google's Firebase to handle authentication into the app.
/// Referenced ChatGPT for help with set-up
/// </summary>
namespace SimU_GameService.Application
{
    private readonly FirebaseAuth _firebaseAuth;

    public FirebaseAuthService()
    {
        FirebaseApp.Create(new AppOptions
        {
            //references the json file containing the firebase service credentials
            Credential = GoogleCredential.FromFile("../simu-27c33-firebase-adminsdk-e1wi5-a160ad26d2.json")
        });
        _firebaseAuth = FirebaseAuth.DefaultInstance;
    }
    public async Task<string> SignUpAsync(string email, string password, string username)
    {
        try
        {
            var user = await _firebaseAuth.CreateUserAsync(new UserRecordArgs
            {
                email = email,
                password = password
            });

            await _firebaseAuth.SetCustomUserClaimsAsync(user.Uid, new Dictionary<string, object>
            {
                { "username", username }
            });

            return user.Uid;
        }
        catch (FirebaseAuthException e)
        {
            // Handle signup error
            throw new Exception($"Error creating user: {e.Message}");
        }
    }

    public async Task<string> SignInAsync(string email, string password)
    {
        try
        {
            var user = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
            // this line used to be user.User.Uid. If error is thrown, replace it back to this line.
            return user.Uid;
        }
        catch (FirebaseAuthException e)
        {
            // Handle login error
            throw new Exception($"Error signing in: {e.Message}");
        }
    }


    public async Task<bool> CheckSignedInAsync()
    {
        try
        {
            var user = await _firebaseAuth.GetUserAsync(auth.CurrentUser.Uid);
            return user != null;
        }
        catch (FirebaseAuthException)
        {
            return false;
        }
    }

    public async Task SignOutAsync()
    {
        try
        {
            await _firebaseAuth.SignOutAsync();
        }
        catch (FirebaseAuthException e)
        {
            // Handle sign-out error
            throw new Exception($"Error signing out: {e.Message}");
        }
    }

    public async Task DeleteCurrentUserAsync(string Uid)
    {
        try
        {
            await _firebaseAuth.DeleteUserAsync(Uid);
        }
        catch (FirebaseAuthException e)
        {
            // Handle delete user error
            throw new Exception($"Error deleting user: {e.Message}");
        }
    }
}