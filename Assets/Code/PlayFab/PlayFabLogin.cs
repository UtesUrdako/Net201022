using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayFabLogin : MonoBehaviour
{
    const string TitleId = "C822B";

    //public UnityEvent <LoginResult> OnLoginSuccesEvent;
    //public UnityEvent <PlayFabError> OnLoginErrorEvent;
    
    
    public UnityEvent <string> OnLoginSuccesEvent;
    public UnityEvent <string> OnLoginErrorEvent;

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = TitleId;
        }
    }


    public void LogIn()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
            return;

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "PlayerTest",
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucces, OnLoginError);
    }

    private void OnLoginSucces(LoginResult result)
    {
        var message = $"Complete Login";

        Debug.Log(message);
        OnLoginSuccesEvent?.Invoke(message);

    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessge = error.GenerateErrorReport();
        Debug.Log(errorMessge);
        OnLoginErrorEvent?.Invoke(errorMessge);
    }
}
