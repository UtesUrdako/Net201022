using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.Events;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string RoomName = "MyRoom";

    public UnityEvent<string> OnConnectedToMasterEvent;
    public UnityEvent<string> OnJoinedRoomEvent;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
    
    }

    public void Connect()
    {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();

            PhotonNetwork.GameVersion = Application.version;
        }


    }

    public override void OnConnectedToMaster()
    {
        var message = $" Connected To Master \n version: {PhotonNetwork.GameVersion}";
        Debug.Log(message);
        OnConnectedToMasterEvent?.Invoke(message);
        PhotonNetwork.CreateRoom(RoomName);

    }


    public override void OnJoinedRoom()
    {
        var message = $"Joined Room {PhotonNetwork.CurrentRoom}";
        Debug.Log(message);
        OnJoinedRoomEvent?.Invoke(message);
        
    }
}
