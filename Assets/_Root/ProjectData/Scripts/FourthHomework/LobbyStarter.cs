using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal sealed class LobbyStarter : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks, IInRoomCallbacks
{
    private const string CUSTOM_PLAYFAB_LOGIN_ID = "TestUser";

    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private NewRoomCanvas _newRoomCanvas;
    [SerializeField] private RoomListCanvas _roomListCanvas;

    private RoomUpdater _roomUpdater;
    private RoomCreator _roomCreator;

    private void LoadLoginScene()
    {
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);

        _roomUpdater = new RoomUpdater(_roomListCanvas);
        _roomCreator = new RoomCreator(_newRoomCanvas);

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            TestConnectToPlayfab();
        }
        else
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinLobby();
            }
            else
            {
                PhotonConnect();
            }
        }
    }

    private void TestConnectToPlayfab()
    {
        Debug.Log("TestConnectToPlayfab");
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest();
        request.CustomId = CUSTOM_PLAYFAB_LOGIN_ID;
        request.CreateAccount = true;

        PlayFabClientAPI.LoginWithCustomID(request, OnCustomIDLoginComplete, OnCustomIDLoginError);
    }

    private void OnCustomIDLoginError(PlayFabError error)
    {
        Debug.Log("OnCustomIDLoginError");
        Debug.LogError(error.ErrorMessage);
        LoadLoginScene(); // return to login scene
    }

    private void OnCustomIDLoginComplete(LoginResult result)
    {
        Debug.Log("OnCustomIDLoginComplete");
        PhotonConnect();
    }

    private void PhotonConnect()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, GetAccountInfoComplete, GetAccountInfoError);
    }

    private void GetAccountInfoError(PlayFabError error)
    {
        Debug.Log("GetAccountInfoError");
        LoadLoginScene(); // return to login scene
    }

    private void GetAccountInfoComplete(GetAccountInfoResult result)
    {
        Debug.Log("GetAccountInfoComplete");

        PhotonNetwork.AuthValues = new AuthenticationValues(result.AccountInfo.PlayFabId);
        PhotonNetwork.NickName = result.AccountInfo.Username;
        PhotonNetwork.GameVersion = PhotonNetwork.AppVersion;

        PhotonNetwork.ConnectUsingSettings(_serverSettings.AppSettings);
    }

    public void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        Debug.Log(PhotonNetwork.AuthValues);
        Debug.Log(PhotonNetwork.NickName);
        Debug.Log(PhotonNetwork.GameVersion);
        PhotonNetwork.JoinLobby();
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed");
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }

    public void OnFriendListUpdate(List<Photon.Realtime.FriendInfo> friendList)
    {
        Debug.Log("OnFriendListUpdate");
    }

    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        SceneManager.LoadScene(2);
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed");
    }

    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("OnLobbyStatisticsUpdate");
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("OnMasterClientSwitched");
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("OnPlayerPropertiesUpdate");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("OnRegionListReceived");
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        _roomUpdater.UpdateRooms(roomList);
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("OnRoomPropertiesUpdate");
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
