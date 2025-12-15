using UnityEngine;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public GameObject loginPanel;
    public GameObject startPanel;
    public TextMeshProUGUI errorText;

    // Called when Login button is pressed
    public void OnLoginButtonClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (email == "" || password == "")
        {
            ShowError("Invalid email or password. Try again.");
            return;
        }

        // TEMP SUCCESS (Firebase later)
        Debug.Log("Login successful (temporary)");
        LoginSuccess();
    }

    // Called when Sign Up button is pressed
    public void OnSignUpButtonClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (email == "" || password == "")
        {
            ShowError("Invalid email or password. Try again.");
            return;
        }

        // TEMP SUCCESS (Firebase later)
        Debug.Log("Sign up successful (temporary)");
        LoginSuccess();
    }

    void LoginSuccess()
    {
        loginPanel.SetActive(false);
        startPanel.SetActive(true);
        errorText.gameObject.SetActive(false);
    }

    void ShowError(string message)
    {
        errorText.text = message;
        errorText.gameObject.SetActive(true);
    }
}
