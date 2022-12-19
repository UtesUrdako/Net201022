using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCreateRoom : MonoBehaviour
{
    [field: SerializeField] public Button _listRoomsButtons { get; private set; }
    [field: SerializeField] public TMP_InputField _roomName { get; private set; }
    [field: SerializeField] public Button _createRoomButton { get; private set; }
    [field: SerializeField] public Button _backButton { get; private set; } 
    [field: SerializeField] public Button _exitRoomButton { get; private set; }
}
