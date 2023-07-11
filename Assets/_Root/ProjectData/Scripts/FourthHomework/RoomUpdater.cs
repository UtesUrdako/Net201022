using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

internal sealed class RoomUpdater
{
    private const string ROOM_BUTTON_PREFAB = "RoomButton";
    private const float ROOM_BUTTON_HEIGHT = 30f;

    private RoomListCanvas _roomListCanvas;
    private GameObject _roomButtonPrefab;

    private Dictionary<string, GameObject> _buttonList;

    public RoomUpdater(RoomListCanvas roomListCanvas)
    {
        _buttonList = new Dictionary<string, GameObject>();
        _roomButtonPrefab = Resources.Load<GameObject>(ROOM_BUTTON_PREFAB);
        _roomListCanvas = roomListCanvas;
    }

    public void UpdateRooms(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            UpdateRoomData(roomInfo);
        }
    }

    private void UpdateRoomData(RoomInfo roomInfo)
    {
        if (roomInfo.RemovedFromList)
        {
            RemoveButton(roomInfo);
            return;
        }

        if (_buttonList.ContainsKey(roomInfo.Name))
        {
            return;
        }

        AddButton(roomInfo);
    }

    private void AddButton(RoomInfo roomInfo)
    {
        GameObject roomButton = GameObject.Instantiate(_roomButtonPrefab, _roomListCanvas.contextRectTransform);
        RoomButton view = roomButton.GetComponent<RoomButton>();
        view.text.text = roomInfo.Name;

        _buttonList.Add(roomInfo.Name, roomButton);

        Vector2 sizeDelta = _roomListCanvas.contextRectTransform.sizeDelta;
        Vector2 newSizeDelta = new Vector2(sizeDelta.x, sizeDelta.y + ROOM_BUTTON_HEIGHT);
        _roomListCanvas.contextRectTransform.sizeDelta = newSizeDelta;
    }

    private void RemoveButton(RoomInfo roomInfo)
    {
        if (!_buttonList.ContainsKey(roomInfo.Name))
        {
            return;
        }
        GameObject roomButton = _buttonList[roomInfo.Name];
        GameObject.Destroy(roomButton);
        _buttonList.Remove(roomInfo.Name);
        Vector2 sizeDelta = _roomListCanvas.contextRectTransform.sizeDelta;
        Vector2 newSizeDelta = new Vector2(sizeDelta.x, sizeDelta.y - ROOM_BUTTON_HEIGHT);
        _roomListCanvas.contextRectTransform.sizeDelta = newSizeDelta;
    }
}
