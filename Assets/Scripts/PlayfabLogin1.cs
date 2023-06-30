using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;

public class PlayfabLogin1 : MonoBehaviourPunCallbacks
{
    private const string AUTH_GUID_KEY = "AUTH_GUID_KEY";

    [SerializeField] private string _playFabTitle;

    private string _guidID;
    private bool _keyPreset;

    private void Start()
    {
        _keyPreset = PlayerPrefs.HasKey(AUTH_GUID_KEY);
        _guidID = PlayerPrefs.GetString(AUTH_GUID_KEY, Guid.NewGuid().ToString());
    }

    private void ConnectByCustomID()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = _playFabTitle;

        var request = new LoginWithCustomIDRequest
        {
            CustomId = _guidID,
            CreateAccount = !_keyPreset
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginComplete, OnLoginError);
    }

    private void OnLoginComplete(LoginResult result)
    {
        PlayerPrefs.SetString(AUTH_GUID_KEY, _guidID);
        Debug.Log($"Complete login!!! ID: {result.PlayFabId}");

        _keyPreset = true;
    }

    private void OnLoginError(PlayFabError error)
    {
        string errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
    }

    private void RemoveID()
    {
        PlayerPrefs.DeleteKey(AUTH_GUID_KEY);
        _keyPreset = false;
        _guidID = PlayerPrefs.GetString(AUTH_GUID_KEY, Guid.NewGuid().ToString());
    }

    void OnGUI()
    {
        string buttonIDText = "<new ID>";
        if (_keyPreset)
        {
            buttonIDText = _guidID;
        }
        if (GUI.Button(new Rect(10, 10, 400, 50), "Connect by custom ID " + buttonIDText))
        {
            ConnectByCustomID();
        }
        if (GUI.Button(new Rect(10, 70, 400, 50), "Delete custom ID info"))
        {
            RemoveID();
        }
    }

}
