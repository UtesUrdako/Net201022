using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
   private void Awake()
   {
      PhotonNetwork.AutomaticallySyncScene = true;
      
   }

   private void Start()
   {
      Connect();
   }

   private void Connect()
   {
      if (PhotonNetwork.IsConnected)
         return;
      
         PhotonNetwork.ConnectUsingSettings();
         PhotonNetwork.GameVersion = Application.version;
      
   }

   public override void OnConnectedToMaster()
   {
      Debug.Log("OnConnectedOnMAster");
      PhotonNetwork.CreateRoom("NewRoom");
   }

   public override void OnJoinedRoom()
   {
      Debug.Log("OnJoinedRoom");
   }
   private void Disconnect(){
      if(PhotonNetwork.IsConnected)
         PhotonNetwork.Disconnect();
      Debug.Log("Disconnect");
   }
}
