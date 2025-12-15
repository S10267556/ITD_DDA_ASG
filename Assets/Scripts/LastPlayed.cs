using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System;

public class LastPlayed : MonoBehaviour
{
    public void LogLastPlayed()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser == null)
        {
            Debug.LogWarning("LastPlayedLogger: No user logged in.");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        DatabaseReference userRef = FirebaseDatabase.DefaultInstance
            .GetReference("players")
            .Child(uid);

        userRef.Child("lastPlayed").SetValueAsync(timestamp);

        Debug.Log("lastPlayed written to Firebase: " + timestamp);
    }
}