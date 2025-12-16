using UnityEngine;
using Firebase.Auth;
using TMPro;
using Firebase.Extensions;

public class AuthManager : MonoBehaviour
{
    [Header("UI Inputs")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    [Header("Optional UI Feedback")]
    public TMP_Text errorText;

    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (errorText != null)
        {
            errorText.text = "";
        }
    }

    // login
    public void OnLoginButtonClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("Invalid email or password. Try again.");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    ShowError("Login failed. Check your details.");
                    Debug.LogError(task.Exception);
                    return;
                }

                FirebaseUser user = task.Result.User;
                Debug.Log("Login successful: " + user.Email);

                ShowError("");
            });
    }

    // sign up
    public void OnSignUpButtonClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("Invalid email or password. Try again.");
            return;
        }

        if (password.Length < 6)
        {
            ShowError("Password must be at least 6 characters.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    ShowError("Sign up failed. Try a different email.");
                    Debug.LogError(task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result.User;
                Debug.Log("Account created: " + newUser.Email);

                ShowError("");
            });
    }

    // UI error handling
    void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
        }
    }
}


// using UnityEngine;
// using Firebase.Auth;
// using TMPro;
// using Firebase.Extensions;
// using UnityEditor;

// public class FirebaseExample : MonoBehaviour
// {
//     public TMP_InputField emailInput;
//     public TMP_InputField passwordInput;

//     public void SignUp()
//     {
//         var createTask = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailInput.text, passwordInput.text);

//         createTask.ContinueWithOnMainThread(task =>
//         {
//             if (task.IsFaulted || task.IsCanceled)
//             {
//                 Debug.Log("Error creating user");
//             }

//             if (task.IsCompleted)
//             {
//                 Debug.Log("User created successfully, please sign in");

//                 var uid = task.Result.User.UserId;
//                 Debug.Log($"Created User UID: {uid}");

//                 var player = new Player(uid, "Name");
//             }
//         });
//     }

//     public void SignIn()
//     {
//         var signinTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text);

//         signinTask.ContinueWithOnMainThread(task =>
//         {
//             if (task.IsFaulted || task.IsCanceled)
//             {
//                 Debug.Log("Error signing in");
//                 return;
//             }

//             if (task.IsCompleted)
//             {
//                 Debug.Log("User signed in successfully");

//                 var uid = task.Result.User.UserId;
//                 Debug.Log($"Signed in user UID: {uid}");
//             }
//         });
//     }

//     void Start()
//     {
        
//     }
// }