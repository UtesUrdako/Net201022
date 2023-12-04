using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField]
    private Button _loginButton;
    [SerializeField]
    private TMP_Text _loginResultText;

    private const string AuthGuidKey = "auth_guid_key";

    private void Start()
    {
        _loginButton.onClick.AddListener(LogIn);

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "F6970";       

    }

    private void LogIn()
    {
        var accountExists = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !accountExists
        };

        PlayFabClientAPI.LoginWithCustomID(request, 
            result => 
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                OnLoginSuccess(result);
            }, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login completed");
        _loginResultText.color = Color.green;
        _loginResultText.text = "You have successfully logged in!";
    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
        _loginResultText.color = Color.red;
        _loginResultText.text =  $"Login to account was interrupted due to an error: {errorMessage}";
    }
}
