using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal sealed class PlayfabUIController
{
    private const string PLAYFAB_TITLE_ID = "5FB76";
    private const string PLAYFAB_CUSTOM_USER_ID = "TestUser";

    private Button _playfabButtonRequest;
    private TMP_Text _playfabStatus;
    private TMP_Text _playfabMessage;

    public PlayfabUIController(Button playfabButtonRequest, TMP_Text playfabStatus, TMP_Text playfabMessage)
    {
        _playfabButtonRequest = playfabButtonRequest;
        _playfabStatus = playfabStatus;
        _playfabMessage = playfabMessage;

        ClearTextLabels();
        _playfabButtonRequest.onClick.AddListener(ButtonRequestPressed);
        ChangeButtonInteractable(true);
    }

    public void Destroy()
    {
        _playfabButtonRequest.onClick.RemoveAllListeners();
    }

    private void ClearTextLabels()
    {
        _playfabStatus.text = string.Empty;
        _playfabMessage.text = string.Empty;
    }

    private void ButtonRequestPressed()
    {
        ChangeButtonInteractable(false);
        InitTitleID();
        LoginWithCustomIDRequest playfabRequest = GetCustomIDRequest();
        PlayfabLogin(playfabRequest, LoginComplete, LoginError);
    }

    private LoginWithCustomIDRequest GetCustomIDRequest()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest();
        request.CustomId = PLAYFAB_CUSTOM_USER_ID;
        request.CreateAccount = true;
        return request;
    }

    private void PlayfabLogin(LoginWithCustomIDRequest request, Action<LoginResult> ActionLoginComplete, Action<PlayFabError> ActionLoginError)
    {
        PlayFabClientAPI.LoginWithCustomID(request, ActionLoginComplete, ActionLoginError);
    }

    private void InitTitleID()
    {
        PlayFabSettings.staticSettings.TitleId = PLAYFAB_TITLE_ID;
    }

    private void ChangeButtonInteractable(bool interactable)
    {
        _playfabButtonRequest.interactable = interactable;
    }

    private void LoginComplete(LoginResult result)
    {
        Debug.Log(result.PlayFabId);
        _playfabStatus.text = "Ok";
        _playfabStatus.color = Color.green;
        _playfabMessage.text = "PlayfabID: " + result.PlayFabId;
        ChangeButtonInteractable(true);
    }

    private void LoginError(PlayFabError error)
    {
        Debug.LogError(error);
        _playfabStatus.text = "Error";
        _playfabStatus.color = Color.red;
        _playfabMessage.text = error.ToString();
        ChangeButtonInteractable(true);
    }
}
