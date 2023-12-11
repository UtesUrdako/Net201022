using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [SerializeField]
    private Button _createRoomForFriendButton;

    [SerializeField]
    private Button _closeRoomButton;

    [SerializeField]
    private GameObject _roomButton;

    [SerializeField]
    private List<GameObject> _roomButtons = new List<GameObject>();

    [SerializeField]
    private TMP_InputField _roomNameInput;

    [SerializeField]
    private GameObject _roomList;

    public Button CreateRoomForFriendButton => _createRoomForFriendButton;
    public Button CloseRoomButton => _closeRoomButton;
    public TMP_InputField RoomNameInput => _roomNameInput;
    public GameObject RoomButton => _roomButton;
    public List<GameObject> RoomButtons => _roomButtons;
    public GameObject RoomList => _roomList;

    public GameObject AddRoomButton(string roomName)
    {
        var roomButton = Instantiate(_roomButton, _roomList.transform);
        roomButton.GetComponentInChildren<TMP_Text>().text = roomName;
        _roomButtons.Add(roomButton);
        return roomButton;
    }

    public void RemoveRoomButton(string roomName)
    {
        var roomButton = _roomButtons.Find(q => q.name == roomName);
        _roomButtons.Remove(roomButton);
        roomButton.GetComponent<Button>().onClick.RemoveAllListeners();
        Destroy(roomButton);
    }

}
