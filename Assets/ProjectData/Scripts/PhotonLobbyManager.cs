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

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
        _uiCreateRoom._createRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);
        _uiCreateRoom._exitRoomButton.onClick.AddListener(OnLeaveGameButtonClicked);
        _uiCreateRoom._listRoomsButtons.onClick.AddListener(UpdateRoomListView);
    }

 

    #region PUN CALLBACKS


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

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError(message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 8 };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
     
        cachedRoomList.Clear();
        OnStartGameButtonClicked();

       // SetActivePanel(InsideRoomPanel.name);


    }

    public override void OnLeftRoom()
    {
      //  SetActivePanel(SelectionPanel.name);

        
    }



 

    #endregion

    #region UI CALLBACKS

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

       // SetActivePanel(SelectionPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = _uiCreateRoom._roomName.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        RoomOptions options = new RoomOptions { MaxPlayers = 4/*, PlayerTtl = 10000*/ };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        //SetActivePanel(JoinRandomRoomPanel.name);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }



    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        //SetActivePanel(RoomListPanel.name);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel(/*"DemoAsteroids-GameScene"*/1);
    }

    #endregion


    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }


    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
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

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            //entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }


    //public void OnLeaveGameButtonClicked()
    //{
    //    PhotonNetwork.LeaveRoom();
    //}


    //private void OnCreateRoom()
    //{

    //    string roomName = _uiCreateRoom._roomName.text;
    //    roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

    //    byte maxPlayers = 4;

    //    RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };
    //    //PhotonNetwork.JoinRandomOrCreateRoom(roomName:roomName,roomOptions: options);
    //    PhotonNetwork.CreateRoom(roomName, options, null);
    //}

    //public override void OnJoinedRoom()
    //{
    //    cachedRoomList.Clear();
    //   // base.OnJoinedRoom();
    //    Debug.Log($"Connected: {PhotonNetwork.CurrentRoom.Name}");
    //}
    //private void UpdateCachedRoomList(List<RoomInfo> roomList)
    //{

    //    foreach (RoomInfo info in roomList)
    //    {

    //        if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
    //        {
    //            if (cachedRoomList.ContainsKey(info.Name))
    //            {
    //                cachedRoomList.Remove(info.Name);
    //            }

    //            continue;
    //        }

    //        if (cachedRoomList.ContainsKey(info.Name))
    //        {
    //            cachedRoomList[info.Name] = info;
    //        }
    //        // Add new room info to cache
    //        else
    //        {
    //            cachedRoomList.Add(info.Name, info);
    //        }
    //    }
    //}

    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    ClearRoomListView();

    //    UpdateCachedRoomList(roomList);
    //    UpdateRoomListView();
    //}

    //public override void OnJoinedLobby()
    //{

    //    // whenever this joins a new lobby, clear any previous room lists
    //    cachedRoomList.Clear();
    //    ClearRoomListView();
    //}
    //private void ClearRoomListView()
    //{
    //    foreach (GameObject entry in roomListEntries.Values)
    //    {
    //        Destroy(entry.gameObject);
    //    }

    //    roomListEntries.Clear();
    //}


    //private void UpdateRoomListView()
    //{
    //    foreach (RoomInfo info in cachedRoomList.Values)
    //    {
    //        GameObject entry = Instantiate(RoomListEntryPrefab);
    //        entry.transform.SetParent(RoomListContent.transform);
    //        entry.transform.localScale = Vector3.one;
    //        entry.GetComponentInChildren<TMPro.TMP_Text>().text = info.Name;

    //        entry.GetComponent<Button>().onClick.AddListener(() =>
    //        {
    //            if (PhotonNetwork.InLobby)
    //            {
    //                PhotonNetwork.LeaveLobby();
    //            }

    //            PhotonNetwork.JoinRoom(info.Name);
    //        });

    //        roomListEntries.Add(info.Name, entry);
    //    }
    //}
}
