using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Authorization : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _playFabTitle;
    [SerializeField] UiInformationPanel _uiInformationPanel;

    private const string AUTHENTIFICATION_KEY = "AUTHENTIFICATION_KEY";

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
            DisplayInformation("Вы не подключены", Color.red);
        SetActiveButton(true);

    }
    private void LoginUser()
    {

        var needCreation = !PlayerPrefs.HasKey(AUTHENTIFICATION_KEY);
        var ID = PlayerPrefs.GetString(AUTHENTIFICATION_KEY, Guid.NewGuid().ToString());


        var request = new LoginWithCustomIDRequest
        {
            CustomId = ID,
            CreateAccount = needCreation
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result =>
            {
                PlayerPrefs.SetString(AUTHENTIFICATION_KEY, ID) ;
                DisplayInformation(result.PlayFabId.ToString(), Color.green);
                PhotonNetwork.AuthValues = new AuthenticationValues(result.PlayFabId);
                PhotonNetwork.NickName = result.PlayFabId;
                Connect();
                DisplayDopInformation();
            },
            error => DisplayInformation(error.ToString(), Color.red));
    }

    private void DisplayDopInformation()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
                    result =>
                    {
                        StringBuilder str = new StringBuilder();
                        str.AppendLine($"Username: {result.AccountInfo.Username}");
                        str.AppendLine($"PlayFabId: {result.AccountInfo.PlayFabId}");
                        str.AppendLine($"Created: {result.AccountInfo.Created}");
                        str.AppendLine($"PrivateInfo: {result.AccountInfo.PrivateInfo.Email}");
                        _uiInformationPanel.dopInformation.text = str.ToString();
                    },
                    error => {
                        _uiInformationPanel.dopInformation.text = error.ErrorMessage;
                    });
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
        PhotonNetwork.LocalPlayer.NickName = (string.IsNullOrEmpty( _uiInformationPanel.nameInputField.text)) 
            ? $"User{UnityEngine.Random.Range(0, 9999)}" 
            : _uiInformationPanel.nameInputField.text;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        if (PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.JoinRandomOrCreateRoom(roomName: $"Room N{UnityEngine.Random.Range(0, 9999)}");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = PhotonNetwork.AppVersion;
            PhotonNetwork.AutomaticallySyncScene = true;
           
        }
        SetActiveButton(false);

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        //if (!PhotonNetwork.InRoom)
        //    PhotonNetwork.JoinRandomOrCreateRoom(roomName: $"Room N{UnityEngine.Random.Range(0, 9999)}");
    }

    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //    Debug.Log("OnCreatedRoom");
    //}

    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    Debug.Log($"OnJoinedRoom {PhotonNetwork.CurrentRoom.Name}");
    //}
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        DisplayInformation("Вы отключены", Color.green);
        Debug.Log("Вы отключены");
    }


}
