using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ConnectAndJoinDefaultLobby : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField]
    private ServerSettings _serverSettings;

    [SerializeField]
    private TMP_Text _stateUiText;

    [SerializeField]
    private TMP_InputField _roomNameInput;

    [SerializeField]
    private Button _createRoomForFriendButton;

    [SerializeField]
    private Button _closeRoomButton;

    [SerializeField]
    private GameObject _roomButton;

    [SerializeField]
    private GameObject _roomList;

    private LoadBalancingClient _lbc;

    private TypedLobby _defaultLobby = new TypedLobby("customDefaultLobby", LobbyType.Default);

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    private void Start()
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this); 

        _lbc.ConnectUsingSettings(_serverSettings.AppSettings);        

        _createRoomForFriendButton.onClick.AddListener(() => CreateRoomForFriends());
        _closeRoomButton.onClick.AddListener(() => CloseRoom());        
    }

    private void CreateRoomForFriends() //lssn6 task3
    {
        string roomName = _roomNameInput.text;

        var roomOptions = new RoomOptions
        {
            MaxPlayers = 12,
            PublishUserId = true
        };

        var enterRoomParams = new EnterRoomParams
        {
            RoomName = roomName,
            RoomOptions = roomOptions,
            Lobby = _defaultLobby
        };

        GUIUtility.systemCopyBuffer = roomName;
        _lbc.OpJoinOrCreateRoom(enterRoomParams);
    }

    private void CloseRoom() //lssn6 task2 
    {
        _lbc.CurrentRoom.IsOpen = false;
        Debug.Log($"{_lbc.CurrentRoom.Name} is open: {_lbc.CurrentRoom.IsOpen}");
    }

    private void OnDestroy()
    {
        _lbc.RemoveCallbackTarget(this);
    }

    private void Update()
    {
        if (_lbc == null)
            return;

        _lbc.Service();

        var state = _lbc.State.ToString();
        _stateUiText.text = state;
    }

    public void OnConnected()
    {

    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");

        _lbc.OpJoinLobby(_defaultLobby);

        _createRoomForFriendButton.gameObject.SetActive(true);
        _roomNameInput.gameObject.SetActive(true);        
    }

    private void GetRoomList() //lssn6 task1
    {
        if(cachedRoomList != null)
        {
            foreach (var room in cachedRoomList.Values)
            {
                var roomButton = Instantiate(_roomButton, _roomList.transform);
                roomButton.GetComponentInChildren<TMP_Text>().text = room.Name;
                Debug.Log(room.Name);

                var roomOptions = new RoomOptions
                {
                    MaxPlayers = 12,
                    PublishUserId = true
                };

                var enterRoomParams = new EnterRoomParams
                {
                    RoomName = room.Name,
                    RoomOptions = roomOptions,
                    Lobby = _defaultLobby
                };

                roomButton.GetComponent<Button>().onClick.AddListener(() => _lbc.OpJoinRoom(enterRoomParams));
            }
        }        
    }

    public void OnCreatedRoom()
    {
        _roomList.SetActive(false);
        Debug.Log($"Room created: {_lbc.CurrentRoom.Name}");
        SceneManager.LoadScene(3);
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {

    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {

    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {

    }

    public void OnJoinedLobby()
    {
        cachedRoomList.Clear();
    }

    public void OnJoinedRoom()
    {
        Debug.Log($"Room joined: {_lbc.CurrentRoom.Name}");
        _createRoomForFriendButton.gameObject.SetActive(false);
        _roomNameInput.gameObject.SetActive(false);
        _roomList.SetActive(false);
        SceneManager.LoadScene(3);

    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        _lbc.OpCreateRoom(new EnterRoomParams());
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {

    }

    public void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public void OnLeftRoom()
    {

    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {

    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {

    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
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
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
        GetRoomList();
    }
}
