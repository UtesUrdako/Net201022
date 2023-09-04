using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class RoomCreator
{
    private const string CONTROL_NUMERIC_STRING = "0123456789";

    private NewRoomCanvas _newRoomCanvas;

    public RoomCreator(NewRoomCanvas newRoomCanvas)
    {
        _newRoomCanvas = newRoomCanvas;
        _newRoomCanvas.buttonCreate.onClick.AddListener(ButtonCreatePressed);
    }

    private void ButtonCreatePressed()
    {
        if (!Check()) return;
        CreateRoom();
    }

    private bool StringContainsChar(string s, char c)
    {
        return s.IndexOf(c) != -1;
    }

    private string CorrectInputFieldText(string text)
    {

        string result = string.Empty;
        foreach (char c in text)
        {
            if (StringContainsChar(CONTROL_NUMERIC_STRING, c)) result += c;
        }

        return result;
    }

    private void CreateRoom()
    {
        string roomName = _newRoomCanvas.nameInputField.text;
        RoomOptions roomOptions = new RoomOptions();
        string maxPlayersString = CorrectInputFieldText(_newRoomCanvas.maxPlayersInputField.text);
        roomOptions.MaxPlayers = 0;
        if (maxPlayersString.Length != 0)
        {
            roomOptions.MaxPlayers = byte.Parse(maxPlayersString);
        }
        
        roomOptions.IsVisible = _newRoomCanvas.toggleVisible.isOn;
        roomOptions.IsOpen = _newRoomCanvas.toggleOpen.isOn;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    private bool Check()
    {
        return true;
    }
}
