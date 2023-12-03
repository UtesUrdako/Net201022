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

    private void Start()
    {
        _loginButton.onClick.AddListener(LogIn);

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "F6970";       

    }

    private void LogIn()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "Player 1",
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
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
