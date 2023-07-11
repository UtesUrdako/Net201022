using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomStarter : MonoBehaviour
{
    [SerializeField] private Button _buttonOpenRoom;
    [SerializeField] private Button _buttonCloseRoom;
    [SerializeField] private Button _buttonLeaveRoom;

    private bool _currentRoomIsOpen;

    private void Start()
    {
        _currentRoomIsOpen = false;
        UpdateButtonsStates();

        _buttonOpenRoom.onClick.AddListener(OpenRoom);
        _buttonCloseRoom.onClick.AddListener(CloseRoom);
        _buttonLeaveRoom.onClick.AddListener(Disconnect);

        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    private void CloseRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        Debug.Log("CloseRoom");
    }

    private void OpenRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        Debug.Log("OpenRoom");
    }

    private void UpdateButtonsStates()
    {
        if (_currentRoomIsOpen)
        {
            _buttonOpenRoom.interactable = false;
            _buttonCloseRoom.interactable = true;
        }
        else
        {
            _buttonOpenRoom.interactable = true;
            _buttonCloseRoom.interactable = false;
        }
    }

    private void Update()
    {
        bool connected = PhotonNetwork.IsConnected;

        if (connected)
        {
            UpdateRoomButtons();
        }
    }

    private void UpdateRoomButtons()
    {
        if (PhotonNetwork.CurrentRoom.IsOpen != _currentRoomIsOpen)
        {
            _currentRoomIsOpen = PhotonNetwork.CurrentRoom.IsOpen;
            UpdateButtonsStates();
        }
    }

    private void OnDestroy()
    {
        _buttonOpenRoom.onClick.RemoveAllListeners();
        _buttonCloseRoom.onClick.RemoveAllListeners();
        _buttonLeaveRoom.onClick.RemoveAllListeners();
    }

    private void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(1); //lobby
    }

}