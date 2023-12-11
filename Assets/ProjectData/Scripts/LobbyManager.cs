using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private LobbyView _lobbyView;
    
    [SerializeField]
    private ServerSettings _serverSettings;

    private string _playerName;
    private string _playerId;
    private float _playerHp;

    public float PlayerHP => _playerHp;

    private TypedLobby _defaultLobby = new TypedLobby("customDefaultLobby", LobbyType.Default);

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnError);

        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.ConnectUsingSettings(_serverSettings.AppSettings);        

        _lobbyView.CreateRoomForFriendButton.onClick.AddListener(() => CreateRoomForFriends());
        _lobbyView.CloseRoomButton.onClick.AddListener(() => CloseRoom());        
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log(errorMessage);
    }

    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        _playerName = result.AccountInfo.Username;
        _playerId = result.AccountInfo.PlayFabId;
    }

    private void CreateRoomForFriends()
    {
        string roomName = _lobbyView.RoomNameInput.text;

        var roomOptions = new RoomOptions
        {
            MaxPlayers = 12,
            PublishUserId = true
        };

        GUIUtility.systemCopyBuffer = roomName;
        PhotonNetwork.JoinOrCreateRoom(roomName,roomOptions,_defaultLobby);
    }

    private void CloseRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        Debug.Log($"{PhotonNetwork.CurrentRoom.Name} is open: {PhotonNetwork.CurrentRoom.IsOpen}");
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected)
            return;
    }

    public override void OnConnected()
    {
               
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AuthValues = new AuthenticationValues(_playerId);
        PhotonNetwork.NickName = _playerName;
        Debug.Log("OnConnectedToMaster");

        PhotonNetwork.JoinLobby(_defaultLobby);

        _lobbyView.CreateRoomForFriendButton.gameObject.SetActive(true);
        _lobbyView.RoomNameInput.gameObject.SetActive(true);        
    }

    private void GetRoomList() 
    {
        if(cachedRoomList != null)
        {
            foreach (var room in cachedRoomList.Values)
            {
                //var roomButton = Instantiate(_lobbyView.RoomButton, _lobbyView.RoomList.transform);
                //roomButton.GetComponentInChildren<TMP_Text>().text = room.Name;
                var roomButton = _lobbyView.AddRoomButton(room.Name);

                var roomOptions = new RoomOptions
                {
                    MaxPlayers = 12,
                    PublishUserId = true
                };

                roomButton.GetComponent<Button>().onClick.AddListener(() => PhotonNetwork.JoinRoom(room.Name));
            }
        }        
    }

    private void GetUserData(string playFabId, string keyData, float playerProperty)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId,
        }, result =>
        {
            playerProperty = float.Parse(result.Data[keyData].Value);
        }, OnError);
    }

    public override void OnCreatedRoom()
    {
        _lobbyView.RoomList.SetActive(false);
        Debug.Log($"Room created: {PhotonNetwork.CurrentRoom.Name}");
        SceneManager.LoadScene(3);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

    }

    public override void OnCustomAuthenticationFailed(string debugMessage)
    {

    }

    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }

    public override void OnJoinedLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Room joined: {PhotonNetwork.CurrentRoom.Name}");
        _lobbyView.CreateRoomForFriendButton.gameObject.SetActive(false);
        _lobbyView.RoomNameInput.gameObject.SetActive(false);
        _lobbyView.RoomList.SetActive(false);
        SceneManager.LoadScene(3);

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {

    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnLeftRoom()
    {

    }

    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {

    }

    public override void OnRegionListReceived(RegionHandler regionHandler)
    {

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        UpdateCachedRoomList(roomList);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
                _lobbyView.RemoveRoomButton(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
        GetRoomList();
    }

    public override void OnFriendListUpdate(List<Photon.Realtime.FriendInfo> friendList)
    {
        throw new NotImplementedException();
    }
}
