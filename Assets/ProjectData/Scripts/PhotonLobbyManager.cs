using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobbyManager : MonoBehaviourPunCallbacks
{
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    [field: SerializeField] public GameObject RoomListEntryPrefab { get; private set; }
    [field: SerializeField] public Transform RoomListContent { get; private set; }
    [field: SerializeField] public UiCreateRoom _uiCreateRoom { get; private set; }
    private void Awake()
    {
        _uiCreateRoom._createRoomButton.onClick.AddListener(OnCreateRoom);
        _uiCreateRoom._exitRoomButton.onClick.AddListener(OnLeaveGameButtonClicked);
        _uiCreateRoom._listRoomsButtons.onClick.AddListener(UpdateRoomListView);
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }


    private void OnCreateRoom()
    {
 
        string roomName = _uiCreateRoom._roomName.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        byte maxPlayers = 4;
    
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };
        //PhotonNetwork.JoinRandomOrCreateRoom(roomName:roomName,roomOptions: options);
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        cachedRoomList.Clear();
       // base.OnJoinedRoom();
        Debug.Log($"Connected: {PhotonNetwork.CurrentRoom.Name}");
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        
        foreach (RoomInfo info in roomList)
        {
            
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnJoinedLobby()
    {
       
        // whenever this joins a new lobby, clear any previous room lists
        cachedRoomList.Clear();
        ClearRoomListView();
    }
    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    
    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponentInChildren<TMPro.TMP_Text>().text = info.Name;

            entry.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }

                PhotonNetwork.JoinRoom(info.Name);
            });
         
            roomListEntries.Add(info.Name, entry);
        }
    }
}
