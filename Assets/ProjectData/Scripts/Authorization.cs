using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Authorization : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _playFabTitle;
    [SerializeField] UiInformationPanel _uiInformationPanel;

    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = _playFabTitle;
        _uiInformationPanel.loginButton.onClick.AddListener(LoginUser);
        _uiInformationPanel.disconnectButton.onClick.AddListener(Disconnected);
        SetActiveButton(true);
    }

    private void Disconnected()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        else
            DisplayInformation("�� �� ����������", Color.red);
        SetActiveButton(true);

    }
    private void LoginUser()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "TestUser",
            // CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result =>
            {
                DisplayInformation(result.PlayFabId.ToString(), Color.green);
                PhotonNetwork.AuthValues = new AuthenticationValues(result.PlayFabId);
                PhotonNetwork.NickName = result.PlayFabId;
                Connect();
            },
            error => DisplayInformation(error.ToString(), Color.red));
    }

    private void DisplayInformation(string text, Color color)
    {
        var textInfo = _uiInformationPanel.infoText;
        textInfo.text = text;
        textInfo.color = color;
    }
    private void SetActiveButton(bool isActive)
    {
        _uiInformationPanel.disconnectButton.gameObject.SetActive(!isActive);
        _uiInformationPanel.loginButton.gameObject.SetActive(isActive);
    }

    private void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomOrCreateRoom(roomName: $"Room N{Random.Range(0, 9999)}");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = PhotonNetwork.AppVersion;
        }
        SetActiveButton(false);

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("OnConnectedToMaster");
        if (!PhotonNetwork.InRoom)
            PhotonNetwork.JoinRandomOrCreateRoom(roomName: $"Room N{Random.Range(0, 9999)}");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("OnCreatedRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log($"OnJoinedRoom {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        DisplayInformation("�� ���������", Color.green);
        Debug.Log("�� ���������");
    }


}
